namespace BibliotecaApi.UseCases.Emprestimo.DTO;

public class CadastrarEmprestimoInputDTO
{
    public int IdUsuario { get; set; }
    public int IdLivro { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
}