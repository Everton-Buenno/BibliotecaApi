namespace BibliotecaApi.Application.Api.Auth.DTO;

public class LoginOutputDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
