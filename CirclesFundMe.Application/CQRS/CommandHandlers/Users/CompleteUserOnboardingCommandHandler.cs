namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class CompleteUserOnboardingCommandHandler(IUnitOfWork unitOfWork, IFileUploadService fileUploadService, IImageService imageService, ICurrentUserService currentUserService, UserManager<AppUser> userManager) : IRequestHandler<CompleteUserOnboardingCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileUploadService _fileUploadService = fileUploadService;
        private readonly IImageService _imageService = imageService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<BaseResponse<bool>> Handle(CompleteUserOnboardingCommand request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            bool userContributionSchemeExists = await _unitOfWork.UserContributionSchemes.ExistAsync([x => x.UserId == userId && x.ContributionSchemeId == request.ContributionSchemeId], cancellationToken);

            if (userContributionSchemeExists)
            {
                return BaseResponse<bool>.BadRequest("You have already completed your onboarding for this contribution scheme.");
            }

            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.NotFound("User not found.");
            }

            if (user.OnboardingStatus == OnboardingStatusEnums.Completed)
            {
                return BaseResponse<bool>.BadRequest("You have already completed your onboarding.");
            }

            string[] nameParts = request.FullName?.Split(' ') ?? [];
            user.FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            if (nameParts.Length == 2)
            {
                user.LastName = nameParts[1];
                user.MiddleName = string.Empty;
            }
            else if (nameParts.Length > 2)
            {
                user.LastName = nameParts[^1];
                user.MiddleName = string.Join(" ", nameParts[1..^1]);
            }
            else
            {
                user.LastName = string.Empty;
                user.MiddleName = string.Empty;
            }

            user.PhoneNumber = request.PhoneNumber;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;

            if (request.GovernmentIssuedID != null)
            {
                string governmentIssuedIdUrl = await _fileUploadService.UploadAsync(request.GovernmentIssuedID);
                string govtFileName = "Govt-Issued-ID-" + userId + '.' + Path.GetExtension(request.GovernmentIssuedID.FileName);

                UserDocument govtIssuedDoc = new()
                {
                    DocumentType = UserDocumentTypeEnums.GovernmentIssuedId,
                    DocumentUrl = governmentIssuedIdUrl,
                    DocumentName = govtFileName,
                    UserId = user.Id
                };

                await _unitOfWork.UserDocuments.AddAsync(govtIssuedDoc, cancellationToken);
            }

            UserKYC userKYC = new()
            {
                UserId = user.Id,
                BVN = request.BVN
            };
            await _unitOfWork.UserKYC.AddAsync(userKYC, cancellationToken);

            if (!string.IsNullOrEmpty(request.Selfie))
            {
                string selfieFileName = "Selfie-" + userId;
                string selfieUrl = await _imageService.UploadImageFromBase64(request.Selfie, selfieFileName);

                UserDocument selfieDoc = new()
                {
                    DocumentType = UserDocumentTypeEnums.Selfie,
                    DocumentUrl = selfieUrl,
                    DocumentName = selfieFileName,
                    UserId = user.Id
                };
                await _unitOfWork.UserDocuments.AddAsync(selfieDoc, cancellationToken);

                user.ProfilePictureUrl = selfieUrl;
            }

            UserAddress userAddress = new()
            {
                FullAddress = request.Address,
                UserId = user.Id
            };
            await _unitOfWork.UserAddresses.AddAsync(userAddress, cancellationToken);

            if (request.UtilityBill != null)
            {
                string utilityBillUrl = await _fileUploadService.UploadAsync(request.UtilityBill);
                string utilityFileName = "Utility-Bill-" + userId + '.' + Path.GetExtension(request.UtilityBill.FileName);

                UserDocument utilityDoc = new()
                {
                    DocumentType = UserDocumentTypeEnums.UtilityBill,
                    DocumentUrl = utilityBillUrl,
                    DocumentName = utilityFileName,
                    UserId = user.Id
                };
                await _unitOfWork.UserDocuments.AddAsync(utilityDoc, cancellationToken);
            }

            UserContributionScheme userContributionScheme = new()
            {
                UserId = user.Id,
                ContributionSchemeId = request.ContributionSchemeId,
                ContributionAmount = request.ContributionAmount,
                IncomeAmount = request.Income
            };
            await _unitOfWork.UserContributionSchemes.AddAsync(userContributionScheme, cancellationToken);

            user.OnboardingStatus = OnboardingStatusEnums.Completed;
            user.ModifiedDate = DateTime.UtcNow;
            user.ModifiedBy = _currentUserService.UserId;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.Success(true, "Onboarding completed successfully.");
        }
    }
}
