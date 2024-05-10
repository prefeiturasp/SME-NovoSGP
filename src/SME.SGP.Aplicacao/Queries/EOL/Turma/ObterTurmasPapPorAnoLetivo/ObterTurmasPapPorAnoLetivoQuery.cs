using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPapPorAnoLetivoQuery : IRequest<IEnumerable<TurmasPapDto>>
    {
        public ObterTurmasPapPorAnoLetivoQuery(long anoLetivo,string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public long AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterTurmasPapPorAnoLetivoQueryValidator : AbstractValidator<ObterTurmasPapPorAnoLetivoQuery>
    {
        public ObterTurmasPapPorAnoLetivoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o ano letivo para consultas as turmas pap");
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("Informe o código da UE para consultas as turmas pap");
        }
    }
}