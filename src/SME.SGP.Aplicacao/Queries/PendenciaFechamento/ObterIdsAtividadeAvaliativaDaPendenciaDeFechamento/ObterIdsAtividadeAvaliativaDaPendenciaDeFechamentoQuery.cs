using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery(IEnumerable<long> idsPendenciaFechamento)
        {
            IdsPendenciaFechamento = idsPendenciaFechamento;
        }

        public IEnumerable<long> IdsPendenciaFechamento { get; set; }
    }

    public class ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQueryValidator : AbstractValidator<ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery>
    {
        public ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQueryValidator()
        {
            RuleFor(c => c.IdsPendenciaFechamento)
            .NotEmpty()
            .WithMessage("O id da pendência do fechamento deve ser informado.");
        }
    }
}
