using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class GraficoAusenciasComJustificativaDto : GraficoBaseDto
    {
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public string NomeTurma { get; set; }
    }

    public class GraficoAusenciasComJustificativaResultadoDto : GraficoBaseDto
    {
        public string ModalidadeAno { get; set; }
    }
}
