using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery : IRequest<long>
    {
        public ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery(string turmaCodigo, int bimestre, int anoLetivo)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
            AnoLetivo = anoLetivo;
        }

        public string TurmaCodigo { get; }
        public int Bimestre { get; }
        public int AnoLetivo { get; }
    }

    public class ObterPeriodoEscolarIdPorTurmaBimestreQueryValidator : AbstractValidator<ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery>
    {
        public ObterPeriodoEscolarIdPorTurmaBimestreQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da turma deve ser informado para consulta do período escolar.");

            RuleFor(c => c.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O bimestre deve ser maior ou igual a zero para consulta do período escolar.");

            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser maior que zero para consulta do período escolar.");
        }
    }
}
