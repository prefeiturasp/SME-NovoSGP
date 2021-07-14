using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class AEETurmaDto
    {
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public int AnoTurma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get => Modalidade == Modalidade.EducacaoInfantil ? 1 :
                Modalidade == Modalidade.Fundamental ? 2 :
                Modalidade == Modalidade.Medio ? 3 :
                4;
        }
    }
}
