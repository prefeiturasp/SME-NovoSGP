using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoAulasPrevistrasUseCase : AbstractUseCase, INotificacaoAulasPrevistrasUseCase
    {
        private readonly IConfiguration configuration;

        public NotificacaoAulasPrevistrasUseCase(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turmaDivergente = mensagem.ObterObjetoMensagem<RegistroAulaPrevistaDivergenteDto>();

            // Busca Professor/Gestor/Supervisor da Turma ou Ue
            var usuarioId = await BuscaProfessorAula(turmaDivergente);

            if (usuarioId > 0)
                if (! await UsuarioNotificado(usuarioId, turmaDivergente.Bimestre, turmaDivergente.CodigoTurma, turmaDivergente.DisciplinaId))
                    await NotificaRegistroDivergencia(usuarioId, turmaDivergente);

            return true;
        }

        private async Task<bool> UsuarioNotificado(long usuarioId, int bimestre, string turmaCodigo, string componenteCurricularId)
            => await mediator.Send(new UsuarioNotificadoAulaPrevistaDivergenteQuery(usuarioId, bimestre, turmaCodigo, componenteCurricularId));

        private async Task<long> BuscaProfessorAula(RegistroAulaPrevistaDivergenteDto turma)
        {
            // Buscar professor da ultima aula
            return await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(turma.ProfessorRf));
        }

        private async Task NotificaRegistroDivergencia(long usuarioId, RegistroAulaPrevistaDivergenteDto registroAulaPrevistaDivergente)
        {
            var tituloMensagem = $"Diferença entre aulas previstas e dadas - Turma {registroAulaPrevistaDivergente.NomeTurma}";
            StringBuilder mensagemUsuario = new StringBuilder();
            mensagemUsuario.Append($"O total de aulas previstas não corresponde ao total de aulas dadas no {registroAulaPrevistaDivergente.Bimestre}º bimestre ");
            mensagemUsuario.Append($"para a turma {registroAulaPrevistaDivergente.NomeTurma} da escola {registroAulaPrevistaDivergente.NomeUe} ({registroAulaPrevistaDivergente.NomeDre})");

            var hostAplicacao = configuration["UrlFrontEnd"];
            mensagemUsuario.Append($"<a href='{hostAplicacao}diario-classe/aula-dada-aula-prevista'>Clique aqui para visualizar os detalhes.</a>");

            await mediator.Send(new SalvarNotificacaoAulaPrevistaCommand(
                tituloMensagem,
                mensagemUsuario.ToString(),
                registroAulaPrevistaDivergente.ProfessorRf,
                registroAulaPrevistaDivergente.CodigoDre,
                registroAulaPrevistaDivergente.CodigoUe,
                registroAulaPrevistaDivergente.CodigoTurma,
                usuarioId,
                registroAulaPrevistaDivergente.Bimestre,
                registroAulaPrevistaDivergente.DisciplinaId));
        }
    }
}
