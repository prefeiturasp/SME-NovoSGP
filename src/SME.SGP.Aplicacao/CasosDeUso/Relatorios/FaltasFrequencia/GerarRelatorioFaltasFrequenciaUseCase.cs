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
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FaltasFrequencia, filtro, usuario, filtro.TipoFormatoRelatorio));
        }
    }
}
