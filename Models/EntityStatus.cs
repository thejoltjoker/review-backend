namespace Review.Api.Models;

public enum EntityStatus
{
    Updated,
    NoChanges,
    Deleted,
    NotFound,
    Forbidden,
    InvalidReference
}