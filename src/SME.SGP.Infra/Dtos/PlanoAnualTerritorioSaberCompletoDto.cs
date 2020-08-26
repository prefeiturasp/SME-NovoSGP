using System;

namespace SME.SGP.Infra
{
    public class PlanoAnualTerritorioSaberCompletoDto
    {
        public DateTime AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public int? AnoLetivo { get; set; }

        public int Bimestre { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public string Desenvolvimento { get; set; }
        public string Reflexao { get; set; }

        public string EscolaId { get; set; }
        public long Id { get; set; }

        public long TerritorioExperienciaId { get; set; }
        public string TurmaId { get; set; }
    }
}
