using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DropdownTurmaRetornoDto
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public Modalidade Modalidade { get; set; }
        public string DescricaoTurma 
        {
            get => $"{Modalidade.ShortName()} - {Descricao}";
        }

    }
}
