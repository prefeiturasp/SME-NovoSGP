using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class AEETurmaDto
    {
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public int AnoTurma { get; set; }
        public string Descricao { get => AnoTurma > 0 ? $"{Modalidade.ShortName()} - {AnoTurma}" : Modalidade.ShortName(); }
    }
}
