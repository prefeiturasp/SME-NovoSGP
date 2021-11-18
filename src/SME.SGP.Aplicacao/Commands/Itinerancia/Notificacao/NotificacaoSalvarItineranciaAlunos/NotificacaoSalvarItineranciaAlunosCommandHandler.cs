using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaAlunosCommandHandler : IRequestHandler<NotificacaoSalvarItineranciaAlunosCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificacaoSalvarItineranciaAlunosCommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificacaoSalvarItineranciaAlunosCommand request, CancellationToken cancellationToken)
        {
            var ue = await mediator.Send(new ObterUeComDrePorIdQuery(request.UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");
            await NotificarItinerancia(ue, request.CriadoRF, request.CriadoPor, request.DataVisita, request.Estudantes, request.ItineranciaId);

            return true;
        }

        private async Task NotificarItinerancia(Ue ue, string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes, long itineranciaId)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Novo Registro de Itinerância - {descricaoUe} - {dataVisita:dd/MM/yyyy}";

            var mensagem = MontarMensagemComOuSemEstudantes(criadoRF, criadoPor, dataVisita, estudantes, descricaoUe);

            var urlServidorRelatorios = configuration.GetSection("UrlServidorRelatorios").Value;

            List<Turma> turmas = new List<Turma>();
            mensagem.AppendLine();

            await MontarTabelaEstudantes(estudantes, mensagem, turmas);

            mensagem.AppendLine();
            mensagem.AppendLine("<br/>Você precisa validar este registro aceitando esta notificação para que o registro seja considerado válido.");
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
            await mediator.Send(new AlterarSituacaoItineranciaCommand(itineranciaId, Dominio.Enumerados.SituacaoItinerancia.AguardandoAprovacao));
        }

        private static StringBuilder MontarMensagemComOuSemEstudantes(string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes, string descricaoUe)
        {
            StringBuilder mensagem = new StringBuilder();
            if (estudantes != null && estudantes.Any())
            {
                mensagem.AppendLine($"O usuário {criadoPor} ({criadoRF}) inseriu um novo registro de itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os seguintes estudantes:");
            }
            else
            {
                mensagem.AppendLine($"O usuário {criadoPor} ({criadoRF}) inseriu um novo registro de itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy}");
            }

            return mensagem;
        }

        private async Task MontarTabelaEstudantes(IEnumerable<ItineranciaAlunoDto> estudantes, StringBuilder mensagem, List<Turma> turmas)
        {
            if (estudantes != null && estudantes.Any())
            {
                mensagem.AppendLine("<br/><br/><table border=2><tr style='font-weight: bold'><td>Estudante</td><td>Turma Regular</td></tr>");
                foreach (var estudante in estudantes.OrderBy(a => a.AlunoNome))
                {
                    var turma = turmas.FirstOrDefault(a => a.Id == estudante.TurmaId);
                    if (turma == null)
                    {
                        turma = await mediator.Send(new ObterTurmaPorIdQuery(estudante.TurmaId));
                        turmas.Add(turma);
                    }
                    mensagem.AppendLine($"<tr><td>{estudante.AlunoNome} ({estudante.AlunoCodigo})</td><td>{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}</td></tr>");
                }
                mensagem.AppendLine("</table>");
            }
        }

        private Cargo[] ObterCargosGestaoEscola() => new [] { Cargo.CP, Cargo.Diretor };
    }
}
