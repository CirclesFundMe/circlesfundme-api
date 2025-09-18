namespace CirclesFundMe.Application.CQRS.Validators.Utility
{
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        private const long _maxFileSize = 3 * 1024 * 1024; // 3MB

        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotNull()
                .WithMessage("Document is required.")
                .Must(file => file != null && file.Length > 0)
                .WithMessage("Document cannot be empty.")
                .Must(file => file != null && file.Length <= _maxFileSize)
                .WithMessage("Document must not exceed 3MB.");
        }
    }
}
