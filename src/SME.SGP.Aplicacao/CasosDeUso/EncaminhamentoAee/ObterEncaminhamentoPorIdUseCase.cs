using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoPorIdUseCase : IObterEncaminhamentoPorIdUseCase
    {
        private readonly IMediator mediator;

        public ObterEncaminhamentoPorIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<EncaminhamentoAEERespostaDto> Executar(long id)
        {
            var encaminhamentoAee = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(id));

            if(encaminhamentoAee == null)
                throw new NegocioException("Encaminhamento não localizado");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(encaminhamentoAee.AlunoCodigo, encaminhamentoAee.Turma.AnoLetivo));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var podeEditar = await VerificaPodeEditar(encaminhamentoAee, usuarioLogado);

            return new EncaminhamentoAEERespostaDto()
            {
                Aluno = aluno,
                Turma = new TurmaAnoDto()
                {
                    Id = encaminhamentoAee.Turma.Id,
                    Codigo = encaminhamentoAee.Turma.CodigoTurma,
                    AnoLetivo = encaminhamentoAee.Turma.AnoLetivo
                },
                Situacao = encaminhamentoAee.Situacao,
                PodeEditar = podeEditar,
                MotivoEncerramento = encaminhamentoAee.MotivoEncerramento,
                Auditoria = (AuditoriaDto)encaminhamentoAee
            };
        }

        private async Task<bool> VerificaPodeEditar(EncaminhamentoAEE encaminhamento, Usuario usuarioLogado)
        {
            switch (encaminhamento.Situacao)
            {
                case SituacaoAEE.Rascunho:
                    return await EhProfessorDaTurma(usuarioLogado, encaminhamento.Turma) || await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamento.Turma);
                case SituacaoAEE.Encaminhado:
                    return await EhGestorDaEscolaDaTurma(usuarioLogado, encaminhamento.Turma);
                case SituacaoAEE.Analise:
                case SituacaoAEE.Finalizado:
                case SituacaoAEE.Encerrado:
                    return false;
                default:
                    return false;
            }
        }

        private async Task<bool> EhProfessorDaTurma(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhProfessor())
                return false;

            var turmas = await mediator.Send(new ObterTurmasDoProfessorQuery(usuarioLogado.CodigoRf));
            return turmas.Any(a => a.CodTurma.ToString() == turma.CodigoTurma);
        }

        private async Task<bool> EhGestorDaEscolaDaTurma(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhGestorEscolar())
                return false;

            var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
            if (ue == null)
                throw new NegocioException($"Escola da turma [{turma.CodigoTurma}] não localizada.");

            return await mediator.Send(new EhGestorDaEscolaQuery(usuarioLogado.CodigoRf, ue.CodigoUe, usuarioLogado.PerfilAtual));
        }
    }
}
