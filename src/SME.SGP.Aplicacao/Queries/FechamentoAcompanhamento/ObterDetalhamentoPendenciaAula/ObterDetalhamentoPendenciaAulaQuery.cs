using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaAulaQuery : IRequest<DetalhamentoPendenciaAulaDto>
    {
        public ObterDetalhamentoPendenciaAulaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }
    public class ObterDetalhamentoPendenciaAulaQueryValidator : AbstractValidator<ObterDetalhamentoPendenciaAulaQuery>
    {
        public ObterDetalhamentoPendenciaAulaQueryValidator()
        {
            RuleFor(a => a.PendenciaId)
                .NotEmpty()
                .WithMessage("O id da pendência deve ser informado.");
        }
    }
}
