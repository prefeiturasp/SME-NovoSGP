using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AvaliacoesBimestraisAlunoDto
    {
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public IEnumerable<AvaliacaoBimestreDto> AvaliacoesBimestrais { get; set; }
        public AvaliacaoFinalDto AvaliacaoFinal { get; set; }
        public bool PossuiConselhoClasse { get; set; }
    }

    public class AvaliacaoBimestreDto
    {
        public string Bimestre { get; set; }
        public IEnumerable<IndicadorAvaliacaoDto> Indicadores { get; set; }
    }

    public class AvaliacaoFinalDto
    {
        public IEnumerable<IndicadorAvaliacaoDto> Indicadores { get; set; }
    }

    public class IndicadorAvaliacaoDto
    {
        public string Componente { get; set; }
        public string Nota { get; set; }
        public double? PercentualFrequencia { get; set; }
    }
}