using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class WorkflowAprovacao : EntidadeBase
    {
        public WorkflowAprovacao()
        {
            niveis = new List<WorkflowAprovacaoNivel>();
        }

        public int Ano { get; set; }
        public string DreId { get; set; }
        public string EscolaId { get; set; }
        public IEnumerable<WorkflowAprovacaoNivel> Niveis { get { return niveis; } }
        public string NotifacaoMensagem { get; set; }
        public string NotifacaoTitulo { get; set; }
        public NotificacaoTipo NotificacaoTipo { get; set; }
        public string TurmaId { get; set; }
        private List<WorkflowAprovacaoNivel> niveis { get; set; }

        public void Adicionar(WorkflowAprovacaoNivel nivel)
        {
            if (nivel == null)
                throw new NegocioException("Não é possível incluir um nível sem informação");

            nivel.Workflow = this;

            niveis.Add(nivel);
        }
    }
}