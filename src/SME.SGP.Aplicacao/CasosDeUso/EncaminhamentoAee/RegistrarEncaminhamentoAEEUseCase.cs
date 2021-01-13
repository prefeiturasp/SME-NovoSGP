using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RegistrarEncaminhamentoAEEUseCase : IRegistrarEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public RegistrarEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoEncaminhamentoAEEDto> Executar(EncaminhamentoAEEDto encaminhamentoAEEDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEEDto.TurmaId));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEEDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException("O aluno informado não foi encontrado");

            if (encaminhamentoAEEDto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id.GetValueOrDefault()));
                if (encaminhamentoAEE != null)
                {

                }
            }

            if (!encaminhamentoAEEDto.Secoes.Any())
                throw new NegocioException("Nenhuma seção foi encontrada");

            var encaminhamentoConcluido = encaminhamentoAEEDto.Secoes.Where(c => !c.Concluido).Any();

            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand(
                encaminhamentoAEEDto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
                encaminhamentoConcluido ? SituacaoAEE.Rascunho : SituacaoAEE.Encaminhado));

            foreach (var secao in encaminhamentoAEEDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException($"Nenhuma questão foi encontrada na Seção {secao.SecaoId}");

                var resultadoEncaminhamentoSecao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoCommand(resultadoEncaminhamento.Id, secao.SecaoId, secao.Concluido));

                foreach (var questao in secao.Questoes)
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(resultadoEncaminhamentoSecao, questao.QuestaoId));
                    var resultadoEncaminhamentoResposta = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand(questao.Resposta, resultadoEncaminhamentoQuestao, questao.TipoQuestao));
                }
            }

            // TODO: Atualiza a situação da seção no encaminhamento, quando todas as questões obrigatórias forem respondidas

            // TODO: Retornar para o front a situação da seção salva

            return resultadoEncaminhamento;
        }
    }
}


