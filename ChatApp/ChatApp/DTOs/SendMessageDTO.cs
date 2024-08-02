namespace ChatApp.DTOs
{
    public sealed record SendMessageDTO(
       int UserId,
       int toUserId,
       string Message,
      
       IFormFile? File);

}