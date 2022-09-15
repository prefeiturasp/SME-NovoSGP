using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoCommand : IRequest<AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto>
    {
        public SalvarFechamentoCommand (FechamentoFinalTurmaDisciplinaDto fechamentoFinalTurmaDisciplina)
        {
            FechamentoFinalTurmaDisciplina = fechamentoFinalTurmaDisciplina;
        }

        public FechamentoFinalTurmaDisciplinaDto FechamentoFinalTurmaDisciplina { get; }
    }
}
