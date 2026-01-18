namespace BibliotecaApi.Domain.Entities;

public sealed class LivroEntity
{
    public int? Id { get; private set; }
    public string Titulo { get; private set; }
    public string Autor { get; private set; }
    public string ISBN { get; private set; }

    public LivroEntity()
    {
        
    }
    public LivroEntity(int? id, string titulo, string autor, string isbn)
    {
        if (id != null)
        {
            if(string.IsNullOrEmpty(id.ToString()))
                throw new ArgumentException("Id não pode ser vazio.");
            if (id <= 0)
                throw new ArgumentException("Id deve ser maior que zero.");
            Id = id.GetValueOrDefault();
        }

        ValidarDados(titulo, autor, isbn);

        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
    }

    public void Cadastrar( string titulo, string autor, string isbn)
    {
        
        ValidarDados(titulo, autor, isbn); 

        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
    }

    private void ValidarDados(string titulo, string autor, string isbn)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("Autor não pode ser vazio.");
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN não pode ser vazio.");
        if (isbn.Length != 13)
            throw new ArgumentException("ISBN deve conter exatamente 13 dígitos numéricos.");
        if (isbn.Any(c => !char.IsDigit(c)))
            throw new ArgumentException("ISBN deve conter apenas números.");
    }


}