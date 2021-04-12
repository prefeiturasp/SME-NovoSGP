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
    public class NotificacaoUsusarioCriadorRegistroItineranciaCommandHandler : IRequestHandler<NotificacaoUsusarioCriadorRegistroItineranciaCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificacaoUsusarioCriadorRegistroItineranciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificacaoUsusarioCriadorRegistroItineranciaCommand request, CancellationToken cancellationToken)
        {
            var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(request.UeCodigo));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");
            await NotificacaoUsusarioCriadorRegistroItinerancia(ue, request.CriadoRF, request.CriadoPor, request.DataVisita, request.Estudantes);

            return true;
        }

        private async Task NotificacaoUsusarioCriadorRegistroItinerancia(Ue ue, string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Registro de Itinerância validado - {descricaoUe} no dia {dataVisita:dd/MM/yyyy}";
            var mensagem = new StringBuilder($"Registro de Itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os seguintes estudantes, abaixo foi validado pelos gestores da UE.");

            List<Turma> turmas = new List<Turma>();
            mensagem.AppendLine();
            mensagem.AppendLine("<br/><br/><table border=2><tr style='font-weight: bold'><td>Estudante</td><td>Turma Regular</td></tr>");
            
            await MontarTabelaEstudantes(estudantes, mensagem, turmas);

            mensagem.AppendLine("</table>");

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.AEE, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));

        }

        private async Task MontarTabelaEstudantes(IEnumerable<ItineranciaAlunoDto> estudantes, StringBuilder mensagem, List<Turma> turmas)
        {
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
        }

        private Cargo[] ObterCargosGestaoEscola()
            => new[] { Cargo.CP, Cargo.Diretor };
    }
}
