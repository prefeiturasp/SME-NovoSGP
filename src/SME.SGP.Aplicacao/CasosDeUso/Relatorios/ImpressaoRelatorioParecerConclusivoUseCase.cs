using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImpressaoRelatorioParecerConclusivoUseCase: AbstractUseCase, IImpressaoRelatorioParecerConclusivoUseCase
    {
        public ImpressaoRelatorioParecerConclusivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioParecerConclusivoDto filtro)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = filtro.TurmaCodigo });
            if (turma == null)
                throw new NegocioException("Não foi possivel localizar a turma informada.");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ParecerConclusivo, filtro, usuarioLogado.Id));
        }
    }
}
