namespace ChatApp.DTOs
{
    public sealed record SendImageDTO(
         int UserId,
         int toUserId,
         IFormFile File);
}