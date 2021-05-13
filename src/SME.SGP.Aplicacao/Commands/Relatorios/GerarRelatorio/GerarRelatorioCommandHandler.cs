using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarRelatorioCommandHandler : IRequestHandler<GerarRelatorioCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;        

        public GerarRelatorioCommandHandler(IMediator mediator, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new System.ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));            
        }

        public async Task<bool> Handle(GerarRelatorioCommand request, CancellationToken cancellationToken)
        {
            var correlacao = new RelatorioCorrelacao(request.TipoRelatorio, request.IdUsuarioLogado, request.Formato);
            repositorioCorrelacaoRelatorio.Salvar(correlacao);

            await mediator.Send(new PublicaFilaWorkerServidorRelatoriosCommand(RotasRabbitRelatorios.RotaRelatoriosSolicitados, request.Filtros, request.TipoRelatorio.Name(), correlacao.Codigo, request.UsuarioLogadoRf, false, request.PerfilUsuario));
            SentrySdk.CaptureMessage("2 - GerarRelatorioCommandHandler");

            return true;
        }
    }
}
