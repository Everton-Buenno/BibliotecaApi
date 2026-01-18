using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Usuario.DTO;

namespace BibliotecaApi.UseCases.Usuario;

public class CadastrarUsuarioUC
{
    private UsuarioEntity _usuario = new UsuarioEntity();
    private UsuarioRepository _repository = new UsuarioRepository();

    public async Task<int> Execute(CadastrarUsuarioInputDTO input)
    {
        bool cpfExiste = await _repository.ExisteCpfAsync(input.CPF);
        if (cpfExiste)
        {
            throw new Exception("Usuário com este CPF já está cadastrado.");
        }

        _usuario.Cadastrar(input.Nome, input.CPF, input.Email);
        int idNovoUsuario = await _repository.Cadastrar(_usuario);
        return idNovoUsuario;
    }
}