using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class Notificacao :EntidadeBase
    {
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string UsuarioId { get; set; }
        public NotificacaoStatus Status { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public string EscolaId { get; set; }
        public string DreId { get; set; }
        public bool PodeRemover { get; set; }

    }
}
