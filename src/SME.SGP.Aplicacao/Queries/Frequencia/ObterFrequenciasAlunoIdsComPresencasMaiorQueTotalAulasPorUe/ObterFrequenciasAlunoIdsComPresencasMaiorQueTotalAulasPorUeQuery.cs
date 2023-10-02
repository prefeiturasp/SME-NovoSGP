using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery : IRequest<long[]>
    {
        public ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;                
        }
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterFrequenciaAlunoComPresencasMaiorQueTotalAulasPorUeQueryValidator : AbstractValidator<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>
    {
        public ObterFrequenciaAlunoComPresencasMaiorQueTotalAulasPorUeQueryValidator()
        {
            RuleFor(x => x.UeId)
                .GreaterThan(0)
                .WithMessage("O id da UE deve ser informado.");

            RuleFor(x => x.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
