using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoNotaConselhoClasseCommand : IRequest
    {
        public ExcluirWfAprovacaoNotaConselhoClasseCommand(long wfAprovacaoConselhoClasseNotaId)
        {
            WfAprovacaoConselhoClasseNotaId = wfAprovacaoConselhoClasseNotaId;
        }

        public long WfAprovacaoConselhoClasseNotaId { get; }
    }

    public class ExcluirWfAprovacaoNotaConselhoClasseCommandValidator : AbstractValidator<ExcluirWfAprovacaoNotaConselhoClasseCommand>
    {
        public ExcluirWfAprovacaoNotaConselhoClasseCommandValidator()
        {
            RuleFor(a => a.WfAprovacaoConselhoClasseNotaId)
                .NotEmpty()
                .WithMessage("O id do workflow de aprovação de nota do conselho de classe deve ser informado para exclusão");
        }
    }
}
