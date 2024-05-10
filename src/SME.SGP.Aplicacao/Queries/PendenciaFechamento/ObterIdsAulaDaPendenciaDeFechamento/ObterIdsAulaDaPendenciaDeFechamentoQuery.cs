using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsAulaDaPendenciaDeFechamentoQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsAulaDaPendenciaDeFechamentoQuery(IEnumerable<long> idsPendenciaFechamento)
        {
            IdsPendenciaFechamento = idsPendenciaFechamento;
        }

        public IEnumerable<long> IdsPendenciaFechamento { get; set; }
    }

    public class ObterIdsAulaDaPendenciaDeFechamentoQueryValidator : AbstractValidator<ObterIdsAulaDaPendenciaDeFechamentoQuery>
    {
        public ObterIdsAulaDaPendenciaDeFechamentoQueryValidator()
        {
            RuleFor(c => c.IdsPendenciaFechamento)
            .NotEmpty()
            .WithMessage("O id da pendência do fechamento deve ser informado.");
        }
    }
}
