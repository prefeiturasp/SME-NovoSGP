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
        public readonly IUnitOfWork _unitOfWork;

        public ConsolidarFrequenciaAlunoPorTurmaEMesUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaAlunoMensal>();
            var frequenciasAlunosTurmasEMeses = await mediator.Send(new ObterFrequenciaAlunosPorTurmaEMesQuery(filtro.TurmaCodigo, filtro.Mes));

            _unitOfWork.IniciarTransacao();
            try
            {
                var turmasIds = frequenciasAlunosTurmasEMeses.Select(c => c.TurmaId).Distinct().ToArray();
                var meses = frequenciasAlunosTurmasEMeses.Select(c => c.Mes).Distinct().ToArray();

                await mediator.Send(new LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand(turmasIds, meses));

                foreach (var frequencia in frequenciasAlunosTurmasEMeses)
                    await RegistrarConsolidacaoFrequenciaAlunoMensal(frequencia);

                _unitOfWork.PersistirTransacao();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            return true;
        }

        private async Task RegistrarConsolidacaoFrequenciaAlunoMensal(RegistroFrequenciaAlunoPorTurmaEMesDto frequencia)
        {            
            await mediator.Send(new RegistrarConsolidacaoFrequenciaAlunoMensalCommand(frequencia.TurmaId, frequencia.AlunoCodigo,
                frequencia.Mes, frequencia.Percentual, frequencia.QuantidadeAulas, frequencia.QuantidadeAusencias, frequencia.QuantidadeCompensacoes));
        }
    }
}
