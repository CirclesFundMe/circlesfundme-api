namespace CirclesFundMe.Application.Services
{
    public interface IOTPService
    {
        Task<(bool result, string message)> ValidateOtp(string email, string otp, CancellationToken cancellation);
        Task<(bool result, string message)> SendOtp(string email, string otp, string name, CancellationToken cancellation);
        Task<bool> VerifyOtp(string email, string otp, CancellationToken cancellation);
    }

    public class OTPService(IUnitOfWork unitOfWork, IQueueService queueService, EncryptionService encryptionService) : IOTPService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IQueueService _queueService = queueService;
        private readonly EncryptionService _encryptionService = encryptionService;

        public async Task<(bool result, string message)> SendOtp(string email, string otp, string name, CancellationToken cancellation)
        {
            UserOtp? userOtp = await _unitOfWork.UserOtps.GetByPrimaryKey(email, cancellation);

            if (userOtp == null)
            {
                userOtp = new UserOtp
                {
                    Email = email,
                    Otp = _encryptionService.Encrypt(otp),
                    ExpiryDate = DateTime.UtcNow.AddMinutes(5),
                    GeneratedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    IsUsed = false
                };

                await _unitOfWork.UserOtps.AddAsync(userOtp, cancellation);
            }
            else
            {
                userOtp.Otp = _encryptionService.Encrypt(otp);
                userOtp.ExpiryDate = DateTime.UtcNow.AddMinutes(5);
                userOtp.IsUsed = false;

                _unitOfWork.UserOtps.Update(userOtp);
            }

            await _unitOfWork.SaveChangesAsync(cancellation);

            _queueService.EnqueueFireAndForgetJob<CFMJobs>(job => job.SendOTP(email, otp, name));

            return (true, "OTP sent successfully");
        }

        public async Task<(bool result, string message)> ValidateOtp(string email, string otp, CancellationToken cancellation)
        {
            UserOtp? userOtp = await _unitOfWork.UserOtps.GetByPrimaryKey(email, cancellation);

            if (userOtp == null)
            {
                return (false, "OTP not found");
            }

            if (userOtp.ExpiryDate < DateTime.UtcNow)
            {
                return (false, "OTP expired");
            }

            if (userOtp.IsUsed)
            {
                return (false, "OTP already used");
            }

            string decryptedOtp = _encryptionService.Decrypt(userOtp.Otp);

            if (decryptedOtp != otp)
            {
                return (false, "Invalid OTP");
            }

            userOtp.IsUsed = true;
            userOtp.ExpiryDate = DateTime.UtcNow;

            _unitOfWork.UserOtps.Update(userOtp);
            await _unitOfWork.SaveChangesAsync(cancellation);

            return (true, "OTP validated successfully");
        }

        public async Task<bool> VerifyOtp(string email, string otp, CancellationToken cancellation)
        {
            UserOtp? userOtp = await _unitOfWork.UserOtps.GetByPrimaryKey(email, cancellation);

            if (userOtp == null)
            {
                return false;
            }

            if (userOtp.ExpiryDate < DateTime.UtcNow)
            {
                return false;
            }

            if (userOtp.IsUsed)
            {
                return false;
            }

            string decryptedOtp = _encryptionService.Decrypt(userOtp.Otp);

            if (decryptedOtp != otp)
            {
                return false;
            }

            return true;
        }
    }
}
