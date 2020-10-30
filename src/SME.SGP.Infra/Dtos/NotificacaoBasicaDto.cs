using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class NotificacaoBasicaDto : ICloneable
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

        public string ObtemTituloRudizoParaCaixaNotificacao()
        {
            if (Titulo.Length > 26)
                return $"{Titulo.Substring(0,26)}...";
            else return Titulo;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}