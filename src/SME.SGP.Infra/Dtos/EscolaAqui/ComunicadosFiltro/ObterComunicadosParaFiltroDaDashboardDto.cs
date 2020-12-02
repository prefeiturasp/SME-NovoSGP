using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int[] GruposIds { get; set; }
        public Modalidade? Modalidade { get; set; }
        public short? Semestre { get; set; }
        public string AnoEscolar { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime? DataEnvioInicial { get; set; }
        public DateTime? DataEnvioFinal { get; set; }
        public string Descricao { get; set; }
    }
}
