using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.ProficienciaIdep
{
    public class ObterProficienciaIdepPorAnoLetivoQuery : IRequest<IEnumerable<Dominio.ProficienciaIdep>>
    {
        public ObterProficienciaIdepPorAnoLetivoQuery(int anoLetivo, List<string> codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public List<string> CodigoUe { get; set; }
    }

    public class ObterProficienciaIdepPorAnoLetivoQueryValidator : AbstractValidator<ObterProficienciaIdepPorAnoLetivoQuery>
    {
        public ObterProficienciaIdepPorAnoLetivoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
