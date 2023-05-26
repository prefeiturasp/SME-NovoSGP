using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery : IRequest<IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery(int anoLetivo, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
    }

    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator : AbstractValidator<ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
