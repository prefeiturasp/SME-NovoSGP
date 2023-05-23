namespace SME.SGP.Infra
{
    public class ConselhoClasseNotasFrequenciaDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
        public bool ConsideraHistorico { get; set; }

        public ConselhoClasseNotasFrequenciaDto(long conselhoClasseId, long fechamentoTurmaId,string alunoCodigo,string codigoTurma,int bimestre,bool consideraHistorico)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
            ConsideraHistorico = consideraHistorico;
        }
    }
}