using System.ComponentModel.DataAnnotations;

namespace Review.Api.Validators;

public class FileTypeValidation : ValidationAttribute
{
    private readonly List<string> _allowedFileTypes =
    [
        "image/bmp",
        "image/gif",
        "image/jpeg",
        "image/png",
        "image/tiff",
        "image/webp",
        "video/avi",
        "video/flv",
        "video/mov",
        "video/mp4",
        "video/wmv"
    ];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? fileType = value as string;
        if (fileType == null) return new ValidationResult("Filetype must not be null");


        var result = _allowedFileTypes.Contains(fileType);
        return result
            ? ValidationResult.Success
            : new ValidationResult(
                "Invalid FileType. Must be one of the following mime types: " +
                string.Join(", ", _allowedFileTypes));
    }
}