using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class WorkflowAprovacaoNivel : EntidadeBase
    {
        public WorkflowAprovacaoNivel()
        {
            Status = WorkflowAprovacaoNivelStatus.SemStatus;
            notificacoes = new List<Notificacao>();
            usuarios = new List<Usuario>();
        }

        public Cargo? Cargo { get; set; }

        public int Nivel { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        public string Observacao { get; set; }
        public WorkflowAprovacaoNivelStatus Status { get; set; }
        public IEnumerable<Usuario> Usuarios { get { return usuarios; } }
        public WorkflowAprovacao Workflow { get; set; }
        public long WorkflowId { get; set; }
        private IList<Notificacao> notificacoes { get; set; }
        private IList<Usuario> usuarios { get; set; }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null && !notificacoes.Any(a => a.Id == notificacao.Id))
                notificacoes.Add(notificacao);
        }        
        public void Adicionar(Usuario usuario)
        {
            if (usuario != null)
                usuarios.Add(usuario);
        }

        public void PodeAprovar()
        {
            if (Status != WorkflowAprovacaoNivelStatus.AguardandoAprovacao)
                throw new NegocioException($"Não é possível aprovar/reprovar este nível pois o mesmo não está Aguardando Aprovação.");
        }

        internal void ModificaStatus(WorkflowAprovacaoNivelStatus status, string observacao)
        {
            if ((status == WorkflowAprovacaoNivelStatus.Reprovado) &&
                (string.IsNullOrEmpty(observacao)))
                    throw new NegocioException("Para recusar é obrigatório informar uma observação.");

            this.Observacao = observacao;
            this.Status = status;
        }
    }
}