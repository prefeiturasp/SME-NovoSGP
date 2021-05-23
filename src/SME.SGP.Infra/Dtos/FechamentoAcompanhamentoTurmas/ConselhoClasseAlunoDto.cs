using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoDto
    {
        public int NumeroChamada { get; set; }
        public string AlunoCodigo { get; set; }
        public string NomeAluno { get; set; }
        public SituacaoConselhoClasse SituacaoConselhoClasse { get; set; }
        public float FrequenciaGlobal { get; set; }
        public bool PodeExpandir { get; set; }
    }
}
