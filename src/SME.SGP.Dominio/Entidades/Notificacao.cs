using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Notificacao : EntidadeBase
    {
        public Notificacao()
        {
            Status = NotificacaoStatus.Pendente;
            Excluida = false;            
        }

        public int Ano { get; set; }
        public long Codigo { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public bool DeveAprovar { get { return Categoria == NotificacaoCategoria.Workflow_Aprovacao; } }
        public bool DeveMarcarComoLido { get { return Categoria == NotificacaoCategoria.Alerta; } }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string Mensagem { get; set; }
        public bool PodeRemover { get { return Categoria == NotificacaoCategoria.Aviso; } }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string Titulo { get; set; }
        public string TurmaId { get; set; }
        public bool Excluida { get; set; }
        public Usuario Usuario { get; set; }
        public long? UsuarioId { get; set; }

        public bool MarcarComoLidaAoObterDetalhe()
        {
            if (Categoria == NotificacaoCategoria.Aviso)
            {
                Status = NotificacaoStatus.Lida;
                return true;
            }
            else return false;
        }
    }
}

