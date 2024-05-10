using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class AEETurmaDto
    {
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public string AnoTurma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get => ObterOrdem(); }

        private int ObterOrdem()
        {
            switch (Modalidade)
            {
                case Modalidade.EducacaoInfantil: return 1;
                case Modalidade.Fundamental: return 2;
                case Modalidade.Medio: return 3;
                default: return 4;
            }
        }
    }
}
