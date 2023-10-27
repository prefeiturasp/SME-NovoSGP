using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCadastroAcessoABAEUseCase : AbstractUseCase, IExcluirCadastroAcessoABAEUseCase
    {
        public ExcluirCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long id)
        {
            var cadastroAcessoABAE = await mediator.Send(new ObterCadastroAcessoABAEPorIdQuery(id));

            if (cadastroAcessoABAE.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.CADASTRO_ACESSO_ABAE_NAO_ENCONTRADO);

            cadastroAcessoABAE.ExcluirLogicamente();

            await mediator.Send(new SalvarCadastroAcessoABAECommand(cadastroAcessoABAE));
            await mediator.Send(new PublicarFilaApiEOLCommand(RotasRabbitApiEOL.RotaManutencaoUsuarioABAECoreSSO,
                cadastroAcessoABAE.toManutencaoUsuarioABAECoreSSOAPIEolDto(await mediator.Send(new ObterUeCodigoPorIdQuery(cadastroAcessoABAE.UeId)))));

            return true;
        }
    }
}