using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioCommandHandler : IRequestHandler<GerarRelatorioCommand, bool>
    {
        private readonly IServicoFila servicoFila;
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;
        private readonly IConfiguration configuration;

        public GerarRelatorioCommandHandler(IServicoFila servicoFila, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio, IConfiguration configuration)
        {
            this.servicoFila = servicoFila ?? throw new System.ArgumentNullException(nameof(servicoFila));
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new System.ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));
            this.configuration = configuration;
        }

        public Task<bool> Handle(GerarRelatorioCommand request, CancellationToken cancellationToken)
        {
            var correlacao = new RelatorioCorrelacao(request.TipoRelatorio, request.IdUsuarioLogado);
            repositorioCorrelacaoRelatorio.Salvar(correlacao);

            servicoFila.PublicaFilaWorkerServidorRelatorios(new PublicaFilaRelatoriosDto(RotasRabbit.RotaRelatoriosSolicitados, request.Filtros, request.TipoRelatorio.Name(), correlacao.Codigo));

            SentrySdk.CaptureMessage("2 - GerarRelatorioCommandHandler");

            return Task.FromResult(true);
        }
    }
}
