using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoParecerConclusivoCommand : IRequest
    {
        public AprovarWorkflowAlteracaoParecerConclusivoCommand(long workflowId, string turmaCodigo, string criadorRf, string criadorNome)
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
}
