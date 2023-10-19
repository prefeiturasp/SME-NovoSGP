using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class SalvarCadastroAcessoABAEUseCase: AbstractUseCase, ISalvarCadastroAcessoABAEUseCase
{
    public SalvarCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
    {}

    public async Task<CadastroAcessoABAE> Executar(CadastroAcessoABAEDto cadastroAcessoABAEDto)
    {
        var cadastroAcessoABAE = cadastroAcessoABAEDto.Id.EhMaiorQueZero() ? await mediator.Send(new ObterCadastroAcessoABAEPorIdQuery(cadastroAcessoABAEDto.Id)): new CadastroAcessoABAE();
        
        cadastroAcessoABAE.Nome = cadastroAcessoABAEDto.Nome;
        cadastroAcessoABAE.Email = cadastroAcessoABAEDto.Email;
        cadastroAcessoABAE.Telefone = cadastroAcessoABAEDto.Telefone;
        cadastroAcessoABAE.Situacao = cadastroAcessoABAEDto.Situacao;
        cadastroAcessoABAE.Cep = cadastroAcessoABAEDto.Cep;
        cadastroAcessoABAE.Endereco = cadastroAcessoABAEDto.Endereco;
        cadastroAcessoABAE.Numero = cadastroAcessoABAEDto.Numero;
        cadastroAcessoABAE.Complemento = cadastroAcessoABAEDto.Complemento;
        cadastroAcessoABAE.Cidade = cadastroAcessoABAEDto.Cidade;
        cadastroAcessoABAE.Estado = cadastroAcessoABAEDto.Estado;
        cadastroAcessoABAE.Excluido = cadastroAcessoABAEDto.Excluido;

        if (cadastroAcessoABAEDto.Id.EhIgualZero())
        {
            cadastroAcessoABAE.UeId = cadastroAcessoABAEDto.UeId;
            cadastroAcessoABAE.Cpf = cadastroAcessoABAEDto.Cpf;
        }

        cadastroAcessoABAE.Id = await mediator.Send(new SalvarCadastroAcessoABAECommand(cadastroAcessoABAE));
        
        await mediator.Send(new PublicarFilaApiEOLCommand(RotasRabbitApiEOL.RotaManutencaoUsuarioABAECoreSSO, cadastroAcessoABAE));

        return cadastroAcessoABAE;
    }
}