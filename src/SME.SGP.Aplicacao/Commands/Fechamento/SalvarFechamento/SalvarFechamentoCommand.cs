using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoCommand : IRequest<AuditoriaPersistenciaDto>
    {
        public SalvarFechamentoCommand (FechamentoFinalTurmaDisciplinaDto fechamentoFinalTurmaDisciplina)
        {
            FechamentoFinalTurmaDisciplina = fechamentoFinalTurmaDisciplina;
        }

        public FechamentoFinalTurmaDisciplinaDto FechamentoFinalTurmaDisciplina { get; }
    }
}
