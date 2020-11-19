using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaAulaCommand : IRequest<bool>
    {
        public ExcluirPendenciaAulaCommand(long aulaId, TipoPendencia tipoPendenciaAula)
        {
            AulaId = aulaId;
            TipoPendenciaAula = tipoPendenciaAula;
        }

        public long AulaId { get; set; }
        public TipoPendencia TipoPendenciaAula { get; set; }
    }


    public class ExcluirPendenciaAulaCommandValidator : AbstractValidator<ExcluirPendenciaAulaCommand>
    {
        public ExcluirPendenciaAulaCommandValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("A Aula deve ser informada.");

            RuleFor(c => c.TipoPendenciaAula)
                .NotEmpty()
                .WithMessage("O tipo de pendência deve ser informado.");
        }
    }
}
