namespace BibliotecaApi.Domain.Entities;

public class EmprestimoEntity
{
    public int Id { get; set; }
    public int IdUsuario { get; private set; }
    public int IdLivro { get; private set; }
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataPrevistaDevolucao { get; private set; }
    public DateTime? DataDevolucao { get; private set; }
    public Decimal Valor { get; private set; }
    public Decimal Multa { get; private set; }
    public Decimal Total { get; private set; }

    public void Cadastrar(int idUsuario, int idLivro, DateTime dataPrevista)
    {
        if (idUsuario <= 0)
            throw new Exception("Usuário inválido.");

        if (idLivro <= 0)
            throw new Exception("Livro inválido.");

        if (dataPrevista <= DateTime.Now)
            throw new Exception("A data prevista de devolução deve ser futura.");

        IdUsuario = idUsuario;
        IdLivro = idLivro;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = dataPrevista;
        DataDevolucao = null;
        Valor = 5.00m; 
    }

    public void RegistrarDevolucao()
    {
        DataDevolucao = DateTime.Now;
        Multa = CalcularMulta();
        Total = Valor + Multa;
    }

    private decimal CalcularMulta()
    {
        if (DataDevolucao == null)
            throw new Exception("Empréstimo ainda não devolvido.");

        
        if (DataDevolucao.Value <= DataPrevistaDevolucao)
            return 0m;

        TimeSpan atraso = DataDevolucao.Value - DataPrevistaDevolucao;
        int diasAtraso = atraso.Days;

        if (diasAtraso <= 0)
            return 0m;


        decimal valorMulta;

        if (diasAtraso <= 3)
        {
            valorMulta = diasAtraso * 2.00m;
        }
        else
        {
            valorMulta = (3 * 2.00m) + ((diasAtraso - 3) * 3.50m);
        }

        if (valorMulta > 50.00m)
            valorMulta = 50.00m;

        return valorMulta;
    }

    public bool EstaEmAtraso()
    {
        return DataDevolucao == null && DataPrevistaDevolucao < DateTime.Now;
    }
}
