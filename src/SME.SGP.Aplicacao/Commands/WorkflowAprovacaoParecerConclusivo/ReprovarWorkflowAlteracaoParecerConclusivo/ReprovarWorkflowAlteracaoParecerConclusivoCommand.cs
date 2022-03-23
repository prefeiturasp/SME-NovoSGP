using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ReprovarWorkflowAlteracaoParecerConclusivoCommand : IRequest
    {
        public ReprovarWorkflowAlteracaoParecerConclusivoCommand(long workflowId, string turmaCodigo, string criadorRF, string criadorNome, string motivo)
        {
            WorkflowId = workflowId;
            TurmaCodigo = turmaCodigo;
            CriadorRF = criadorRF;
            CriadorNome = criadorNome;
            Motivo = motivo;
        }

        public long WorkflowId { get; }
        public string TurmaCodigo { get; }
        public string CriadorRF { get; }
        public string CriadorNome { get; }
        public string Motivo { get; }
    }

    public class ReprovarWorkflowAlteracaoParecerConclusivoCommandValidator : AbstractValidator<ReprovarWorkflowAlteracaoParecerConclusivoCommand>
    {
        public ReprovarWorkflowAlteracaoParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O identificador do workflow de aprovação deve ser informado para realizar a reprovação do mesmo");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma do workflow de aprovação deve ser informado para realizar a reprovação do mesmo");

            RuleFor(a => a.CriadorRF)
                .NotEmpty()
                .WithMessage("O RF do criador do workflow de aprovação deve ser informado para realizar a reprovação do mesmo");

            RuleFor(a => a.CriadorNome)
                .NotEmpty()
                .WithMessage("O nome do criador do workflow de aprovação deve ser informado para realizar a reprovação do mesmo");
        }
    }
}
