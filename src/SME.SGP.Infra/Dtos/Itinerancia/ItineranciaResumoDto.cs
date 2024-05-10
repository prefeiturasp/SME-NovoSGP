using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaResumoDto
    {
        public long Id { get; set; }
        public string DataVisita { get; set; }
        public string UeNome { get; set; }
        public string EstudanteNome { get; set; }
        public string TurmaNome { get; set; }
        public string Situacao { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long ArquivoId { get; set; }
        public string ArquivoNome { get; set; }
        public Guid ArquivoCodigo { get; set; }
        public string CriadoPor { get; set; }
    }
}
