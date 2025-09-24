using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.ProficienciaIdeb
{
    public class ObterProficienciaIdebPorAnoLetivoQuery : IRequest<IEnumerable<Dominio.Entidades.ProficienciaIdeb>> 
    {
        public ObterProficienciaIdebPorAnoLetivoQuery(int anoLetivo, List<string> codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public List<string> CodigoUe { get; set; }
    }

    public class ObterProficienciaIdebPorAnoLetivoQueryValidator : AbstractValidator<ObterProficienciaIdebPorAnoLetivoQuery>
    {
        public ObterProficienciaIdebPorAnoLetivoQueryValidator()
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
