using FluentValidation;
using MediatR;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdeb
{
    public class ObterProficienciaIdebQuery : IRequest<IEnumerable<PainelEducacionalProficienciaIdebDto>>
    {
        public ObterProficienciaIdebQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }
        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterProficienciaIdebQueryValidator : AbstractValidator<ObterProficienciaIdebQuery>
    {
        public ObterProficienciaIdebQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("Informe a unidade escolar para consultar a proficiência IDEB.");

            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O ano letivo deve ser maior ou igual a 0.")
                .Must(ano => ano == 0 || ano >= PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .WithMessage($"O ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE} quando informado.");
        }
    }
}
