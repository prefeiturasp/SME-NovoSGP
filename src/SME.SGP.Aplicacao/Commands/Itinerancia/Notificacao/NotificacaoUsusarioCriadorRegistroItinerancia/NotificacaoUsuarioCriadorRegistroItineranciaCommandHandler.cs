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
    public class NotificacaoUsuarioCriadorRegistroItineranciaCommandHandler : IRequestHandler<NotificacaoUsuarioCriadorRegistroItineranciaCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificacaoUsuarioCriadorRegistroItineranciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificacaoUsuarioCriadorRegistroItineranciaCommand request, CancellationToken cancellationToken)
        {
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(request.ItineranciaId));
            if (itinerancia == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");

            var ue = await mediator.Send(new ObterUeComDrePorIdQuery(itinerancia.UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");

            await NotificacaoUsuarioCriadorRegistroItinerancia(ue, itinerancia.CriadoRF, itinerancia.DataVisita, itinerancia.Alunos);
            return true;
        }

        private async Task NotificacaoUsuarioCriadorRegistroItinerancia(Ue ue, string criadoRF, DateTime dataVisita, IEnumerable<ItineranciaAluno> estudantes)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Registro de Itinerância validado - {descricaoUe} no dia {dataVisita:dd/MM/yyyy}";

            StringBuilder mensagem = new StringBuilder();
            if (estudantes != null && estudantes.Any())
                mensagem = new StringBuilder($"Registro de Itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os seguintes estudantes, abaixo foi validado pelos gestores da UE.");
            else
                mensagem = new StringBuilder($"Registro de Itinerância para a {descricaoUe} no dia {dataVisita:dd/MM/yyyy} foi validado pelos gestores da UE.");

            var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(criadoRF));

            if (estudantes != null && estudantes.Any())
                await MontarTabelaEstudantes(estudantes, mensagem, dataVisita);

            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.AEE, new long[] { usuario }));
        }

        private async Task MontarTabelaEstudantes(IEnumerable<ItineranciaAluno> estudantes, StringBuilder mensagem, DateTime dataVisita)
        {
            List<Turma> turmas = new List<Turma>();
            mensagem.AppendLine();
            mensagem.AppendLine("<br/><br/><table border=2><tr style='font-weight: bold'><td>Estudante</td><td>Turma Regular</td></tr>");

            var estudantesEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(estudantes.Select(a => Convert.ToInt64(a.CodigoAluno)).ToArray(), dataVisita.Year));

            foreach (var estudante in estudantesEol.OrderBy(a => a.NomeAluno).DistinctBy(a => a.CodigoAluno))
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(estudantes.FirstOrDefault(e => e.CodigoAluno == estudante.CodigoAluno.ToString()).TurmaId));
                if (turma != null)
                {
                    turmas.Add(turma);
                    mensagem.AppendLine($"<tr><td>{estudante.NomeAluno} ({estudante.CodigoAluno})</td><td>{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}</td></tr>");
                }

            }
            mensagem.AppendLine("</table>");
        }
    }
}