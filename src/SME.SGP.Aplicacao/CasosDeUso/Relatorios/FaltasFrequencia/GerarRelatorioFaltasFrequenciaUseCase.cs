using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioFaltasFrequenciaUseCase : IGerarRelatorioFaltasFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public GerarRelatorioFaltasFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(FiltroRelatorioFaltasFrequenciaDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FaltasFrequencia, filtro, usuarioLogado));
            return await Task.FromResult(true);
        }
    }
}
