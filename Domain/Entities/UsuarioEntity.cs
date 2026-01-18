namespace BibliotecaApi.Domain.Entities;

public class UsuarioEntity
{
    public int Id { get; set; }
    public string Nome { get; private set; }
    public string CPF { get; private set; }
    public string Email { get; private set; }

    public void Cadastrar(string nome, string cpf, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome do usuário é obrigatório.");

        if (string.IsNullOrWhiteSpace(cpf))
            throw new Exception("O CPF é obrigatório.");

        if (cpf.Length != 11)
            throw new Exception("CPF deve conter 11 dígitos.");

        Nome = nome.Trim();
        CPF = cpf.Trim();
        Email = email?.Trim();
    }
}
