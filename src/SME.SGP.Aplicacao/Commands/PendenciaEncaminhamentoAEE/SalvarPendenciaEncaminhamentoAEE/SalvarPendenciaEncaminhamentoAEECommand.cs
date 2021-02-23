using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaEncaminhamentoAEECommand : IRequest<long>
    {
        public SalvarPendenciaEncaminhamentoAEECommand(long pendenciaId, long turmaId, long encaminhamentoAEEId, string professorRf, long? periodoEscolarId)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
            EncaminhamentoAEEId = EncaminhamentoAEEId;
            PeriodoEscolarId = periodoEscolarId;
            ProfessorRf = professorRf;
        }

        public long PendenciaId { get; set; }
        public long TurmaId { get; set; }
        public long EncaminhamentoAEEId { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public string ProfessorRf { get; set; }
    }
}
