using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe
{
    public class ConsolidacaoFluenciaLeitoraUeDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public string Turma { get; set; }
        public int AnoLetivo { get; set; }
        public int AlunosPrevistos { get; set; }
        public int AlunosAvaliados { get; set; }
        public int TipoAvaliacao { get; set; }
        public int PreLeitorTotal { get; set; }
        public NivelFluenciaLeitoraEnum Fluencia { get; set; }
        public int QuantidadeAlunoFluencia { get; set; }
        public decimal PercentualFluencia { get; set; }
    }
}
