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

            var ue = await mediator.Send(new ObterUeComDrePorIdQuery(itinerancia.UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível encontrar a UE informada");
            
            await NotificarItineranciaRecusada(ue, itinerancia.CriadoRF, itinerancia.DataVisita, itinerancia.Alunos, request.Observacoes);
            
            return true;
        }

        private async Task NotificarItineranciaRecusada(Ue ue, string criadoRF, DateTime dataVisita, IEnumerable<ItineranciaAluno> estudantes, string observacao)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Registro de Itinerância devolvido - {descricaoUe} - {dataVisita:dd/MM/yyyy}";
            var mensagem = new StringBuilder($"Registro de Itinerância da {descricaoUe} no dia {dataVisita:dd/MM/yyyy} para os estudantes abaixo foi devolvido pelos gestores da UE");

            await MontarTabelaDeEstudantes(estudantes, mensagem, dataVisita);

            mensagem.AppendLine($"<br><br>Motivo: {observacao}");

            var funcionarios = await ObtemUsuariosCEFAIDRE(ue.Dre.CodigoDre);
            
            funcionarios.Add(criadoRF);
            var usuarios = await ObterUsuariosId(funcionarios) ;

            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.AEE, usuarios));
        }

        private async Task<List<string>> ObtemUsuariosCEFAIDRE(string codigoDre)
        {
            var cefais = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return cefais.ToList();
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            List<long> usuarios = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                usuarios.Add(usuario);
            }
            return usuarios;
        }


        private async Task MontarTabelaDeEstudantes(IEnumerable<ItineranciaAluno> estudantes, StringBuilder mensagem, DateTime dataVisita)
        {
            List<Turma> turmas = new List<Turma>();
            mensagem.AppendLine("<br/><br/><table border=2><tr style='font-weight: bold'><td>Estudante</td><td>Turma Regular</td></tr>");

            var estudantesMapeados = await MapearEstudantesItinerancia(estudantes, dataVisita);

            foreach (var estudante in estudantesMapeados.OrderBy(a => a.AlunoNome))
            {
                var turma = turmas.FirstOrDefault(a => a.Id == estudantes.FirstOrDefault(e => e.CodigoAluno == estudante.AlunoCodigo.ToString()).TurmaId);
                if (turma == null)
                {
                    turma = await mediator.Send(new ObterTurmaPorIdQuery(estudantes.FirstOrDefault(e => e.CodigoAluno == estudante.AlunoCodigo.ToString()).TurmaId));
                    turmas.Add(turma);
                }
                mensagem.AppendLine($"<tr><td>{estudante.AlunoNome} ({estudante.AlunoCodigo})</td><td>{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}</td></tr>");
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine();

        }
        private async Task<List<ItineranciaAlunoDto>> MapearEstudantesItinerancia(IEnumerable<ItineranciaAluno> estudantes, DateTime dataVisita)
        {
            List<ItineranciaAlunoDto> itineranciaAlunos = new List<ItineranciaAlunoDto>();

            var estudantesEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(estudantes.Select(a => Convert.ToInt64(a.CodigoAluno)).ToArray(), dataVisita.Year));

            foreach(var estudante in estudantes)
            {
                itineranciaAlunos.Add(new ItineranciaAlunoDto()
                {
                    AlunoCodigo = estudante.CodigoAluno,
                    AlunoNome = estudantesEol.FirstOrDefault(e => e.CodigoAluno.ToString() == estudante.CodigoAluno).NomeAluno
                });
            }

            return itineranciaAlunos;
        }
    }
}
