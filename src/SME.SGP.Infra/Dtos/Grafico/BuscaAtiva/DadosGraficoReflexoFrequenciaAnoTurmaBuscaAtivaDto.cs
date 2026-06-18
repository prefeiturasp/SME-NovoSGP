using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto
    {
        public string Ano { get; set; }
        public string Turma { get; set; }
        public string ReflexoFrequencia { get; set; }
        public int Quantidade { get; set; }   
        public Modalidade Modalidade { get; set; }
    }
}