using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaFechamentoPorAulaCommand : IRequest<bool>
    {
        public AtualizarPendenciaFechamentoPorAulaCommand(long idAula, TipoPendencia tipoPendencia)
        {
            IdAula = idAula;
            TipoPendencia = tipoPendencia;
        }

        public long IdAula { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class AtualizarPendenciaFechamentoPorAulaCommandValidator : AbstractValidator<AtualizarPendenciaFechamentoPorAulaCommand>
    {
        public AtualizarPendenciaFechamentoPorAulaCommandValidator()
        {
            RuleFor(c => c.IdAula)
            .NotEmpty()
            .WithMessage("O id da aula deve ser informado.");

            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado.");
        }
    }
}
