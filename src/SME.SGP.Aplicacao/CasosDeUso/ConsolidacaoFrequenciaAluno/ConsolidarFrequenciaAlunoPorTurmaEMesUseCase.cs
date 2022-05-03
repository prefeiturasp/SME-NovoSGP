using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaAlunoPorTurmaEMesUseCase : AbstractUseCase, IConsolidarFrequenciaAlunoPorTurmaEMesUseCase
    {
        public readonly IUnitOfWork unitOfWork;

        public ConsolidarFrequenciaAlunoPorTurmaEMesUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaAlunoMensal>();
            var frequenciasAlunosTurmaEMes = await mediator.Send(new ObterFrequenciaAlunosPorTurmaEMesQuery(filtro.TurmaCodigo, filtro.Mes));

            var turmaId = frequenciasAlunosTurmaEMes.Select(c => c.TurmaId).FirstOrDefault();

            unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand(new long[] { turmaId }, new int[] { filtro.Mes }));

                foreach (var frequencia in frequenciasAlunosTurmaEMes)
                    await RegistrarConsolidacaoFrequenciaAlunoMensal(frequencia, filtro.Mes);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            var comandoConsolidacaoFrequenciaTurmaEvasao = new FiltroConsolidacaoFrequenciaTurmaEvasao(turmaId, filtro.Mes);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConsolidacaoFrequenciaTurmaEvasao, comandoConsolidacaoFrequenciaTurmaEvasao, Guid.NewGuid(), null));

            return true;
        }

        private async Task RegistrarConsolidacaoFrequenciaAlunoMensal(RegistroFrequenciaAlunoPorTurmaEMesDto frequencia, int mes)
        {            
            await mediator.Send(new RegistrarConsolidacaoFrequenciaAlunoMensalCommand(frequencia.TurmaId, frequencia.AlunoCodigo,
                mes, frequencia.Percentual, frequencia.QuantidadeAulas, frequencia.QuantidadeAusencias, frequencia.QuantidadeCompensacoes));
        }
    }
}
