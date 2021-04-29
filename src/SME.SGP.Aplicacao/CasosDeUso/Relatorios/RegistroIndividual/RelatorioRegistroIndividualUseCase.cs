using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioRegistroIndividualUseCase : AbstractUseCase, IRelatorioRegistroIndividualUseCase
    {
        public RelatorioRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioRegistroIndividualDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRF = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RegistroIndividual, filtro, usuarioLogado));
        }
    }
}
