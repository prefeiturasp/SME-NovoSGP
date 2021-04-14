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
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(request.ItineranciaId));
            if (itinerancia == null)
                throw new NegocioException("Não foi possível encontrar a Itinerância informada");

            var ue = await mediator.Send(new ObterUeComDrePorIdQuery(itinerancia.Ues.FirstOrDefault().UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");


            var workflow = await mediator.Send(new ObterWorkflowPorIdQuery(request.WorkflowId));


            await NotificarItineranciaRecusada(ue, itinerancia.CriadoRF, itinerancia.DataVisita, itinerancia.Alunos, workflow.Niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado).Observacao);
            return true;
        }

        private async Task NotificarItineranciaRecusada(Ue ue, string criadoRF, DateTime dataVisita, IEnumerable<ItineranciaAluno> estudantes, string observacao)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Registro de Itinerância devolvido - {descricaoUe} - {dataVisita:dd/MM/yyyy}";
            var mensagem = new StringBuilder($"Registro de Itinerância da {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os estudantes abaixo foi devolvido pelos gestores da UE");

            await MontarTabelaDeEstudantes(estudantes, mensagem, dataVisita);

            mensagem.AppendLine($"<br><br>Motivo: {observacao}");

            var usuarioIdCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(ue.Dre.CodigoDre));

            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.AEE, new long[] { Convert.ToInt64(criadoRF), usuarioIdCEFAI }));
        }

        private async Task MontarTabelaDeEstudantes(IEnumerable<ItineranciaAluno> estudantes, StringBuilder mensagem, DateTime dataVisita)
        {
            List<Turma> turmas = new List<Turma>();
            var estudantesEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(estudantes.Select(a => Convert.ToInt64(a.CodigoAluno)).ToArray(), dataVisita.Year));

            foreach (var estudante in estudantesEol.OrderBy(a => a.NomeAluno))
            {
                var turma = turmas.FirstOrDefault(a => a.Id == estudantes.FirstOrDefault(e => e.CodigoAluno == estudante.CodigoAluno.ToString()).TurmaId);
                if (turma == null)
                {
                    turma = await mediator.Send(new ObterTurmaPorIdQuery(estudantes.FirstOrDefault(e => e.CodigoAluno == estudante.CodigoAluno.ToString()).TurmaId));
                    turmas.Add(turma);
                }
                mensagem.AppendLine($"<tr><td>{estudante.NomeAluno} ({estudante.CodigoAluno})</td><td>{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}</td></tr>");
                mensagem.AppendLine("</table>");
            }
            

        }

    }
}
