namespace BibliotecaApi.UseCases.Emprestimo.DTO;

public class DevolverEmprestimoInputDTO
{
    public int IdEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; } = DateTime.Now;
}