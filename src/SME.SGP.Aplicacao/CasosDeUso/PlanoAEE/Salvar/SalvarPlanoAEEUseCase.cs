using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class SalvarPlanoAEEUseCase : ISalvarPlanoAEEUseCase
    {
        private readonly IMediator mediator;

        public SalvarPlanoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Executar(PlanoAEEPersistenciaDto planoAeeDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAeeDto.TurmaId));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(planoAeeDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException("O aluno informado não foi encontrado");

            var resultado = await mediator.Send(new SalvarPlanoAeeCommand(planoAeeDto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno, aluno.NumeroAlunoChamada.ToString(), planoAeeDto.Situacao));


            // Questoes

            // Respostas

            // Versao
            //foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
            //{
            //    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(secaoEncaminhamento.Id, questoes.FirstOrDefault().QuestaoId));
            //    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
            //}

            return resultado;
        }
    }
}


