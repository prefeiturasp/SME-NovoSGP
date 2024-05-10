using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase : AbstractUseCase, IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroAcompanhamentoAprendizagemAlunoTurmaDTO>();
            var ue = await mediator.Send(new ObterUEPorTurmaIdQuery(filtro.TurmaId));
            if (ue.TipoEscola == TipoEscola.CEIDIRET || ue.TipoEscola == TipoEscola.CEIINDIR || ue.TipoEscola == TipoEscola.CEUCEI)
                return true;

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTime.Today));
            alunos = alunos.Where(s => s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Rematriculado ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.PendenteRematricula ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.SemContinuidade ||
                                    s.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);
            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

            string[] codigosAlunos = alunos.Select(a => a.CodigoAluno).ToArray();

            var totalAlunosComAcompanhamento = await mediator.Send(new ObterTotalAlunosComAcompanhamentoQuery(filtro.TurmaId, filtro.AnoLetivo, filtro.Semestre, codigosAlunos));

            var totalAlunosSemAcompanhamento = (filtro.QuantidadeAlunosTurma - totalAlunosComAcompanhamento) < 0 ? 0 : (filtro.QuantidadeAlunosTurma - totalAlunosComAcompanhamento);

            await mediator.Send(new RegistraConsolidacaoAcompanhamentoAprendizagemCommand(filtro.TurmaId, totalAlunosComAcompanhamento, totalAlunosSemAcompanhamento, filtro.Semestre));

            return true;
        }
    }
}
