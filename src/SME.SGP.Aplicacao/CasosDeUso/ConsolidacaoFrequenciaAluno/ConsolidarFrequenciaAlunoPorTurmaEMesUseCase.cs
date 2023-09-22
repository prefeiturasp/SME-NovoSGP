using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaAlunoPorTurmaEMesUseCase : AbstractUseCase, IConsolidarFrequenciaAlunoPorTurmaEMesUseCase
    {
        public readonly IUnitOfWork unitOfWork;

        public ConsolidarFrequenciaAlunoPorTurmaEMesUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaAlunoMensal>();

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaCodigo));
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma informada!");

            var frequenciasAlunosTurmaEMes = await mediator.Send(new ObterFrequenciaAlunosPorTurmaEMesQuery(turma.CodigoTurma, filtro.Mes));
            var consolidacoesExistentesDaTurma = await mediator.Send(new ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery(turma.Id, filtro.Mes));

            unitOfWork.IniciarTransacao();
            try
            {
                await AtualizaAlunos(consolidacoesExistentesDaTurma, frequenciasAlunosTurmaEMes, filtro.Mes, turma.Id);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
            
            var comandoConsolidacaoFrequenciaTurmaEvasao = new FiltroConsolidacaoFrequenciaTurmaEvasao(turma.Id, filtro.Mes);
            var comandoConsolidacaoFrequenciaTurmaEvasaoAcumulado = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(turma.AnoLetivo, turma.Id);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasao, comandoConsolidacaoFrequenciaTurmaEvasao, Guid.NewGuid(), null));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasaoAcumulado, comandoConsolidacaoFrequenciaTurmaEvasaoAcumulado, Guid.NewGuid(), null));            
            
            return true;
        }

        private async Task AtualizaAlunos(IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto> consolidacoesExistentes, IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto> frequenciasAtuais, int mes, long turmaId)
        {
            foreach (var frequencia in frequenciasAtuais)
            {
                var dadosAlteradosAluno = consolidacoesExistentes.FirstOrDefault(a => a.AlunoCodigo.Contains(frequencia.AlunoCodigo) &&
                    (a.QuantidadeAulas != frequencia.QuantidadeAulas || a.QuantidadeAusencias != frequencia.QuantidadeAusencias || a.QuantidadeCompensacoes != frequencia.QuantidadeCompensacoes)
                );

                if (dadosAlteradosAluno != null)
                {
                    await mediator.Send(new AlterarConsolidacaoFrequenciaAlunoMensalCommand(dadosAlteradosAluno.Id, frequencia.Percentual, frequencia.QuantidadeAulas,
                        frequencia.QuantidadeAusencias, frequencia.QuantidadeCompensacoes));
                }
                else if (!consolidacoesExistentes.Any(a => a.AlunoCodigo.Contains(frequencia.AlunoCodigo)) || !consolidacoesExistentes.Any())
                {
                    await mediator.Send(new RegistrarConsolidacaoFrequenciaAlunoMensalCommand(turmaId, frequencia.AlunoCodigo, mes, frequencia.Percentual, frequencia.QuantidadeAulas,
                        frequencia.QuantidadeAusencias, frequencia.QuantidadeCompensacoes));
                }
            }

            foreach (var consolidacao in consolidacoesExistentes)
            {
                var validaAlunoAtivo = frequenciasAtuais.Any(a => a.AlunoCodigo == consolidacao.AlunoCodigo);
                if (!validaAlunoAtivo)
                    await mediator.Send(new RemoverConsolidacaoAlunoFrequenciaMensalCommand() { ConsolidacaoId = consolidacao.Id });
            }
        }
    }
}
