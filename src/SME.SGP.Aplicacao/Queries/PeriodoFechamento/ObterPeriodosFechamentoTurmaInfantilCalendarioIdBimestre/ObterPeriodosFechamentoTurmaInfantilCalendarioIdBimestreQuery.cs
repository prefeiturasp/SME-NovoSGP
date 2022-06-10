using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery : IRequest<IEnumerable<PeriodoFechamentoBimestre>>
    {
        public ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery(long tipoCalendarioId, int bimestre)
        {
            TipoCalendarioId = tipoCalendarioId;
            Bimestre = bimestre;
        }

        public long TipoCalendarioId { get; set; }
        public int Bimestre { get; set; }
    }
    public class ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryValidator : AbstractValidator<ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery>
    {
        public ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendario deve ser informado para consulta de período de fechamento");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de período de fechamento");
        }
    }
}
