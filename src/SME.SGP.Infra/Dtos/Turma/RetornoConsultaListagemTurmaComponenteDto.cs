using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RetornoConsultaListagemTurmaComponenteDto
    {
        public long? Id { get; set; }
        public long TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string NomeTurma { get; set; }
        public string Ano { get; set; }
        public string NomeComponenteCurricular { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long ComponenteCurricularPaiCodigo { get; set; }
        public long ComponenteCurricularTerritorioSaberCodigo { get; set; }
        public TipoTurnoEOL Turno { get; set; }
        public bool TerritorioSaber { get; set; }
        public string ComplementoTurmaEJA { get; set; }
        public string SerieEnsino { get; set; }
        public string NomeFiltro { get; set; }
        public string NomeTurmaFormatado(string componenteCurricularNome)
        {
            var complementoTurma = string.IsNullOrEmpty(ComplementoTurmaEJA) ? $"{Ano}ºAno" : ComplementoTurmaEJA.TrimEnd();

            return $"{Modalidade.ShortName()} - {NomeTurma} - {complementoTurma} - {componenteCurricularNome}";
        }

        public string NomeTurmaFiltroFormatado(string componenteCurricularNome)
        {
            return $"{Modalidade.ShortName()} - {NomeFiltro} - {componenteCurricularNome}";
        }

    }
}
