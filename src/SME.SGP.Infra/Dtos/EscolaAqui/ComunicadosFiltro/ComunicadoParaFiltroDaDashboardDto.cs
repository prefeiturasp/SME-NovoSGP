using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro
{
    public class ComunicadoParaFiltroDaDashboardDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEnvio { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Modalidade { get; set; }

    }
}
