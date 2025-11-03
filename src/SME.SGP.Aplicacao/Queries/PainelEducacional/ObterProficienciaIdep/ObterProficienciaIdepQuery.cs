using FluentValidation;
using MediatR;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQuery : IRequest<IEnumerable<PainelEducacionalProficienciaIdepDto>>
    {
        public ObterProficienciaIdepQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }
        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterProficienciaIdepQueryValidator : AbstractValidator<ObterProficienciaIdepQuery>
    {
        public ObterProficienciaIdepQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("Informe a unidade escolar para consultar a proficiência IDEP.");

            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O ano letivo deve ser maior ou igual a 0.")
                .Must(ano => ano == 0 || ano >= PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .WithMessage($"O ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE} quando informado.");
        }
    }
}
