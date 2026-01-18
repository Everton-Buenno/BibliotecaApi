namespace BibliotecaApi.Application.Api.Responses;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public T? Conteudo { get; set; }
    public string? MensagemErro { get; set; }

    public static ApiResponse<T> Ok(T conteudo) => new() { Sucesso = true, Conteudo = conteudo };
    public static ApiResponse<T> Falha(string erro) => new() { Sucesso = false, MensagemErro = erro };
}
