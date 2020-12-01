using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<long> GruposIds { get; set; }
        public Modalidade? Modalidade { get; set; }
        public short? Semestre { get; set; }
        public short? AnoEscolar { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime? DataEnvioInicial { get; set; }
        public DateTime? DataEnvioFinal { get; set; }
        public string Descricao { get; set; }
    }
}
