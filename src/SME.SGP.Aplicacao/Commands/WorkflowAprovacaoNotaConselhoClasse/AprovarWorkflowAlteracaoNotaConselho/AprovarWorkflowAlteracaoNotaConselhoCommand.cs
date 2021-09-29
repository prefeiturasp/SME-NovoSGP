using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoNotaConselhoCommand : IRequest
    {
        public AprovarWorkflowAlteracaoNotaConselhoCommand(long workflowId, string turmaCodigo, string criadorRf, string criadorNome)
        {
            WorkflowId = workflowId;
            TurmaCodigo = turmaCodigo;
            CriadorRf = criadorRf;
            CriadorNome = criadorNome;
        }

        public long WorkflowId { get; }
        public string TurmaCodigo { get; }
        public string CriadorRf { get; }
        public string CriadorNome { get; }
    }

    public class AprovarWorkflowAlteracaoNotaConselhoCommandValidator : AbstractValidator<AprovarWorkflowAlteracaoNotaConselhoCommand>
    {
        public AprovarWorkflowAlteracaoNotaConselhoCommandValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O id do workflow deve ser informado para aprovar a alteração de nota do conselho de classe");
        }
    }
}
