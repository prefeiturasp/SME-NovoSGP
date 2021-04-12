using MediatR;
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
    public class NotificacaoRegistroItineranciaRecusadoCommandHandler : IRequestHandler<NotificacaoRegistroItineranciaRecusadoCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificacaoRegistroItineranciaRecusadoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificacaoRegistroItineranciaRecusadoCommand request, CancellationToken cancellationToken)
        {
            var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(request.UeCodigo));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");
            await NotificarItineranciaRecusada(ue, request.CriadoRF, request.CriadoPor, request.DataVisita, request.Estudantes);

            return true;
        }

        private async Task NotificarItineranciaRecusada(Ue ue, string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Registro de Itinerância devolvido - {descricaoUe} - {dataVisita:dd/MM/yyyy}";
            var mensagem = new StringBuilder($"Registro de Itinerância da {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os estudantes abaixo foi devolvido pelos gestores da UE");

            await MontarTabelaDeEstudantes(estudantes, mensagem);

            mensagem.AppendLine("<br/>Você precisa validar este registro aceitando esta notificação para que o registro seja considerado válido.");
            mensagem.AppendLine();
            mensagem.AppendLine("<br/><br/>Clique no botão abaixo para fazer o download do arquivo.");
            mensagem.AppendLine();
            mensagem.AppendLine("<br/><br/><a href='https://dev-novosgp.sme.prefeitura.sp.gov.br' target='_blank' class='btn-baixar-relatorio'><i class='fas fa-arrow-down mr-2'></i>Download</a>");

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Workflow_Aprovacao, NotificacaoTipo.AEE, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));

        }

        private async Task MontarTabelaDeEstudantes(IEnumerable<ItineranciaAlunoDto> estudantes, StringBuilder mensagem)
        {
            List<Turma> turmas = new List<Turma>();
            mensagem.AppendLine();
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
            mensagem.AppendLine();
        }

        private Cargo[] ObterCargosGestaoEscola()
            => new[] { Cargo.CP, Cargo.Diretor };
    }
}
