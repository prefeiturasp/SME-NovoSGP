using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro
{
    public class FiltroObterComunicadosParaFiltroDaDashboardDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }        
        public int[] Modalidades { get; set; }
        public short? Semestre { get; set; }
        public string AnoEscolar { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime? DataEnvioInicial { get; set; }
        public DateTime? DataEnvioFinal { get; set; }
        public string Titulo { get; set; }


    }
}
