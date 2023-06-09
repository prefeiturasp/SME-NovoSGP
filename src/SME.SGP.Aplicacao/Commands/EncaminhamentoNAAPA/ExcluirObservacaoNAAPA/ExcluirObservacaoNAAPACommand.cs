using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoNAAPACommand : IRequest<bool>
    {
        public ExcluirObservacaoNAAPACommand(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }

        public long ObservacaoId { get; set; }
    }
    public class ExcluirObservacaoNAAPACommandValidator : AbstractValidator<ExcluirObservacaoNAAPACommand>
    {
        public ExcluirObservacaoNAAPACommandValidator()
        {
            RuleFor(a => a.ObservacaoId)
                .GreaterThan(0)
                .WithMessage("O Id da Observacao deve ser informado");
        }
    }
}