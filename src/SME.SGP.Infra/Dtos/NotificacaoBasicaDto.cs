using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class NotificacaoBasicaDto
    {
        public NotificacaoCategoria Categoria { get; set; }
        public string DescricaoCategoria { get; set; }
        public long Codigo { get; set; }
        public DateTime Data { get; set; }
        public string DescricaoStatus { get; set; }
        public long Id { get; set; }
        public bool PodeMarcarComoLida { get; set; }
        public bool PodeRemover { get; set; }
        public NotificacaoStatus Status { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
    }
}