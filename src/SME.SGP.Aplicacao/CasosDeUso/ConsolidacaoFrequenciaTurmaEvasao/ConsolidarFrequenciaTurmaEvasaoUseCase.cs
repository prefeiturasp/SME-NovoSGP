using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmaEvasaoUseCase : AbstractUseCase, IConsolidarFrequenciaTurmaEvasaoUseCase
    {
        public readonly IUnitOfWork unitOfWork;

        public ConsolidarFrequenciaTurmaEvasaoUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurmaEvasao>();            
            var quantidadeAlunosAbaixo50Porcento = 0;
            var quantidadeAlunox0Porcento = 0;

           
            var consolidacoesFrequenciaAlunoMensal = await mediator.Send(new ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery(filtro.TurmaId, filtro.Mes));
            consolidacoesFrequenciaAlunoMensal = consolidacoesFrequenciaAlunoMensal.DistinctBy(c => c.AlunoCodigo);

            foreach (var consolidacao in consolidacoesFrequenciaAlunoMensal)
            {
                if (consolidacao.Percentual < 50 && consolidacao.Percentual > decimal.Zero)
                {
                    quantidadeAlunosAbaixo50Porcento++;
                }
            }

            foreach (var consolidacao in consolidacoesFrequenciaAlunoMensal)
            {
                if (consolidacao.Percentual == decimal.Zero)
                {
                    quantidadeAlunox0Porcento++;
                }
            }

            unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(new long[] { filtro.TurmaId }, new int[] { filtro.Mes }));
                await RegistrarFrequenciaTurmaEvasao(filtro.TurmaId, filtro.Mes, quantidadeAlunosAbaixo50Porcento, quantidadeAlunox0Porcento);
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }

        private async Task RegistrarFrequenciaTurmaEvasao(long turmaId, int mes, int quantidadeAlunosAbaixo50Porcento, int quantidadeAlunos0Porcento)
        {
            await mediator.Send(new RegistrarFrequenciaTurmaEvasaoCommand(turmaId, mes, quantidadeAlunosAbaixo50Porcento, quantidadeAlunos0Porcento));
        }
    }
}
