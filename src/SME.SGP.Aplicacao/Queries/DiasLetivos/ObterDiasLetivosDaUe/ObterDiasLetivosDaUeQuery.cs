using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasLetivosDaUeQuery : IRequest<int>
    {
        public ObterDiasLetivosDaUeQuery(IEnumerable<DiaLetivoDto> diasLetivosENaoLetivos, string dreCodigo, string ueCodigo)
        {
            DiasLetivosENaoLetivos = diasLetivosENaoLetivos;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public IEnumerable<DiaLetivoDto> DiasLetivosENaoLetivos { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }

    public class ObterDiasLetivosDaUeQueryValidator : AbstractValidator<ObterDiasLetivosDaUeQuery>
    {
        public ObterDiasLetivosDaUeQueryValidator()
        {
            RuleFor(c => c.DiasLetivosENaoLetivos)
               .NotEmpty()
               .WithMessage("Os dias letivos e não letivos devem ser informados para calculo de dias letivos.");

            RuleFor(c => c.DreCodigo)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado para calculo de dias letivos.");

            RuleFor(c => c.UeCodigo)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado para calculo de dias letivos.");
        }
    }
}
