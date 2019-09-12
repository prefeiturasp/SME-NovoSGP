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
        public int Nivel { get; set; }
        public WorkflowAprovacaoNivelStatus Status { get; set; }
        public WorkflowAprovacao Workflow { get; set; }
        public long WorkflowId { get; set; }
        public Cargo Cargo { get; set; }
        private IList<Notificacao> notificacoes { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes;  }  }
        public IEnumerable<Usuario> Usuarios { get { return usuarios; } }
        public IList<Usuario> usuarios { get; set; }
        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }
        public void Adicionar(Usuario usuario)
        {
            if (usuario != null)
                usuarios.Add(usuario);
        }

    }
}