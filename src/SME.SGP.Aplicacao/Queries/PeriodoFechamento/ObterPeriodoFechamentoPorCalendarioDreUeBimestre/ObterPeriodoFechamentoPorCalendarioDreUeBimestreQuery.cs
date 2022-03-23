using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery : IRequest<PeriodoFechamentoBimestre>
    {
        public ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery(long tipoCalendarioId, int bimestre, long dreId, long ueId)
        {
            TipoCalendarioId = tipoCalendarioId;
            Bimestre = bimestre;
            DreId = dreId;
            UeId = ueId;
        }

        public long TipoCalendarioId { get; }
        public int Bimestre { get; }
        public long DreId { get; }
        public long UeId { get; }
    }

    public class ObterPeriodoFechamentoPorCalendarioDreUeBimestreQueryValidator : AbstractValidator<ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery>
    {
        public ObterPeriodoFechamentoPorCalendarioDreUeBimestreQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendario deve ser informado para consulta de período de fechamento");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de período de fechamento");

            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O identificador da DRE deve ser informado para consulta de período de fechamento");

            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consulta de período de fechamento");
        }
    }
}
