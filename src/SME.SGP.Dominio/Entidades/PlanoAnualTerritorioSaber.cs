using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PlanoAnualTerritorioSaber : EntidadeBase
    {
        public int Ano { get; set; }
        public long Bimestre { get; set; }
        public long TerritorioExperienciaId { get; set; }
        public string Desenvolvimento { get; set; }
        public string Reflexao { get; set; }
        public string EscolaId { get; set; }
        public long TurmaId { get; set; }
    }
}
