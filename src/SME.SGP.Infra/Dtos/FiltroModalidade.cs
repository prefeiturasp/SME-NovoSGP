using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroModalidade
    {
        public FiltroModalidade(Modalidade modalidade, string[] anosTurma = null)
        {
            Modalidade = modalidade;
            AnosTurma = anosTurma;
        }

        public Modalidade Modalidade { get; set; }
        public string[] AnosTurma { get; set; }
    }
}
