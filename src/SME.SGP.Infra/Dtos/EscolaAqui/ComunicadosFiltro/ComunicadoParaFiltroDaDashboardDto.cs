using System;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro
{
    public class ComunicadoParaFiltroDaDashboardDto
    {
        public DateTime DataEnvio { get; set; }
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string TituloFiltro => $"{Titulo} - {DataEnvio.ToShortDateString()}";
    }
}
