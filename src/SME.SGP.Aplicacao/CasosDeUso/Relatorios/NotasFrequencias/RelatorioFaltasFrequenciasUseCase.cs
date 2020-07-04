using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioFaltasFrequenciasUseCase : IRelatorioFaltasFrequenciasUseCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public RelatorioFaltasFrequenciasUseCase(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(FiltroRelatorioFaltasFrequenciasDto filtroRelatorioFaltasFrequenciasDto)
        {

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Não foi possível localizar o usuário.");


            using (SentrySdk.Init(configuration.GetValue<string>("Sentry:DSN")))
            {
                SentrySdk.CaptureMessage("1 - RelatorioFaltasFrequencias");
            }

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FaltasFrequencias, filtroRelatorioFaltasFrequenciasDto, usuario));
        }
    }
}
