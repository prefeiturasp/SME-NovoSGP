using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanalUe
{
    public class ObterFrequenciaSemanalUeQuery : IRequest<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>>
    {
        public ObterFrequenciaSemanalUeQuery(string codigoUe, int anoLetivo)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
        }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterFrequenciaSemanalUeQueryValidator : AbstractValidator<ObterFrequenciaSemanalUeQuery>
    {
        public ObterFrequenciaSemanalUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código UE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
