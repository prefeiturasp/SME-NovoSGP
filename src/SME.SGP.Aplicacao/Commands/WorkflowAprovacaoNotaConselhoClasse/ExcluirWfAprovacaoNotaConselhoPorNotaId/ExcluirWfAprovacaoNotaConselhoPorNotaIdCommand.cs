using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand : IRequest
    {
        public ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand(long conselhoClasseNotaId)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
        }

        public long ConselhoClasseNotaId { get; }
    }

    public class ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandValidator : AbstractValidator<ExcluirWfAprovacaoNotaConselhoPorNotaIdCommand>
    {
        public ExcluirWfAprovacaoNotaConselhoPorNotaIdCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseNotaId)
                .NotEmpty()
                .WithMessage("O identificador da nota do conselho de classe deve ser informado para exclusão dos workflows de aprovação existentes");
        }
    }
}
