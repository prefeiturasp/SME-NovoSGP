using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadoEvento
{
    public class SituacaoComunicadoEADto
    {
        public long NotificacaoId { get; set; }
        public DateTime? DataHoraLeitura { get; set; }
        public string Situacao { get; set; }
    }
}
