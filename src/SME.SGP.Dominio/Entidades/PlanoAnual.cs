namespace SME.SGP.Dominio
{
    public class PlanoAnual : EntidadeBase
    {
        public int Ano { get; set; }
        public long Bimestre { get; set; }
        public long ComponenteCurricularEolId { get; set; }
        public string Descricao { get; set; }
        public string EscolaId { get; set; }
        public bool Migrado { get; set; }
        public long TurmaId { get; set; }
        public bool ObjetivosAprendizagemOpcionais { get; set; }
    }
}