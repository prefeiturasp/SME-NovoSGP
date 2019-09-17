using System;
using SME.SGP.Dominio;

namespace SME.SGP.Dto
{
    public class NotificacaoBasicaDto
    {
        public NotificacaoCategoria Categoria { get; set; }
        public string Codigo { get; set; }
        public string Data { get; set; }
        public string DescricaoStatus { get; set; }
        public long Id { get; set; }
        public NotificacaoStatus Status { get; set; }
        public string Tipo { get; set; }

        public string Titulo { get; set; }
        public bool PodeRemover { get; set; }
        public Action PodeMarcarComoLida { get; set; }
    }
}