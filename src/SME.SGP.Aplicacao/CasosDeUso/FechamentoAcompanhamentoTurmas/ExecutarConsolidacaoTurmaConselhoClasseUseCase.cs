using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseUseCase
    {
        public ExecutarConsolidacaoTurmaConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurmaConselhoClasse = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurmaConselhoClasse.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma e o bimestre não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }

            if (consolidacaoTurmaConselhoClasse.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma. O id da turma não foi informado", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }
            
            var turma = await mediator
                .Send(new ObterTurmaComUeEDrePorIdQuery(consolidacaoTurmaConselhoClasse.TurmaId));

            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTime.Today));

            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException($"Não foram encontrados alunos para a turma {turma.CodigoTurma} no Eol");

            foreach (var aluno in alunos)
            {
                await PublicarMensagem(aluno, consolidacaoTurmaConselhoClasse, 0, mensagemRabbit.CodigoCorrelacao);                 
            }

            return true;
        }

        private async Task<bool> PublicarMensagem(AlunoPorTurmaResposta aluno, ConsolidacaoTurmaDto consolidacaoTurmaConselhoClasse, long codigoComponenteCurricular, Guid CodigoCorrelacao)
        {
            try
            {
                var mensagemConsolidacaoConselhoClasseAluno = new MensagemConsolidacaoConselhoClasseAlunoDto(aluno.CodigoAluno,
                                                                                                             consolidacaoTurmaConselhoClasse.TurmaId,
                                                                                                             consolidacaoTurmaConselhoClasse.Bimestre,
                                                                                                             aluno.Inativo,
                                                                                                             componenteCurricularId: codigoComponenteCurricular);

                var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemConsolidacaoConselhoClasseAluno);

                var publicarFilaConsolidacaoConselhoClasseAluno = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemParaPublicar, CodigoCorrelacao, null));
                if (!publicarFilaConsolidacaoConselhoClasseAluno)
                {
                    var mensagem = $"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.";
                    await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Critico, LogContexto.ConselhoClasse));
                    throw new NegocioException(mensagem);
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                throw;
            }
        }
    }
}
