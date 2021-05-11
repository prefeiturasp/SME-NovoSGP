using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaSemAlunosVinculadosCommandHandler : IRequestHandler<NotificacaoSalvarItineranciaSemAlunosVinculadosCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificacaoSalvarItineranciaSemAlunosVinculadosCommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificacaoSalvarItineranciaSemAlunosVinculadosCommand request, CancellationToken cancellationToken)
        {
            var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(request.UeCodigo));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");

            await NotificarItineranciaSemAlunosVinculados(ue, request.CriadoRF, request.CriadoPor, request.DataVisita, request.ItineranciaId);

            return true;
        }

        private async Task NotificarItineranciaSemAlunosVinculados(Ue ue, string criadoRF, string criadoPor, DateTime dataVisita, long itineranciaId)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Novo Registro de Itinerância - {descricaoUe} - {dataVisita:dd/MM/yyyy}";
            var mensagem = new StringBuilder($"O usuário {criadoPor} ({criadoRF}) inseriu um novo registro de itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy}.");
            var urlServidorRelatorios = configuration.GetSection("UrlServidorRelatorios").Value;

            mensagem.AppendLine();
            mensagem.AppendLine("<br/><br/>Clique no botão abaixo para fazer o download do arquivo.");
            mensagem.AppendLine();

            var codigoCorrelacao = await mediator.Send(new SolicitaRelatorioItineranciaCommand(new FiltroRelatorioItineranciaDto()
            {
                Itinerancias = new long[] { itineranciaId },
                UsuarioNome = criadoPor,
                UsuarioRF = criadoRF
            }));

            mensagem.AppendLine($"<br/><br/><a href='{urlServidorRelatorios}api/v1/downloads/sgp/pdf/Itiner%C3%A2ncias.pdf/{codigoCorrelacao}' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>");

            var workflowId = await mediator.Send(new EnviarNotificacaoItineranciaCommand(itineranciaId, titulo, mensagem.ToString(), NotificacaoCategoria.Workflow_Aprovacao, NotificacaoTipo.AEE, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));

            await mediator.Send(new SalvarWorkflowAprovacaoItineranciaCommand(itineranciaId, workflowId));
            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Alerta, NotificacaoTipo.AEE, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));
        }
        private Cargo[] ObterCargosGestaoEscola()
            => new[] { Cargo.CP, Cargo.Diretor };
    }
}
