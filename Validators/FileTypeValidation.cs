using System.ComponentModel.DataAnnotations;

namespace Review.Api.Validators;

public class FileTypeValidation : ValidationAttribute
{
    private readonly Dictionary<string, string> _allowedFileTypes = new()
    {
        { ".bmp", "image/bmp" },
        { ".gif", "image/gif" },
        { ".jpeg", "image/jpeg" },
        { ".jpg", "image/jpeg" },
        { ".png", "image/png" },
        { ".tiff", "image/tiff" },
        { ".webp", "image/webp" },
        { ".avi", "video/avi" },
        { ".flv", "video/flv" },
        { ".mov", "video/mov" },
        { ".mp4", "video/mp4" },
        { ".wmv", "video/wmv" }
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? fileType = value as string;
        if (string.IsNullOrWhiteSpace(fileType))
            return new ValidationResult("Filetype must not be empty");

        var fileNameProp = validationContext.ObjectType.GetProperty("FileName");
        string? fileName = fileNameProp?.GetValue(validationContext.ObjectInstance) as string;

        string fileExtension = Path.GetExtension(fileName)?.Trim().ToLowerInvariant() ?? string.Empty;
        string normalizedFileType = fileType.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(fileExtension))
            return new ValidationResult("FileName must include a valid file extension.");

        if (!_allowedFileTypes.TryGetValue(fileExtension, out var expectedFileType))
            return new ValidationResult($"Unsupported file extension '{fileExtension}'.");

        if (!string.Equals(normalizedFileType, expectedFileType, StringComparison.OrdinalIgnoreCase))
            return new ValidationResult(
                $"FileType '{fileType}' does not match extension '{fileExtension}'. Expected '{expectedFileType}'."
            );
        return ValidationResult.Success;
    }
}