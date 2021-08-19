namespace SME.SGP.Infra.Dtos
{
    public class FechamentoSituacaoPorEstudanteDto
    {
        public int Ordem { get;  set; }
        public string Descricao { get; private set; }
        public string LegendaSemRegistro { get; private set; }
        public int QuantidadeSemRegistro { get; private set; }
        public string LegendaParcial { get; private set; }
        public int QuantidadeParcial { get; private set; }
        public int QuantidadeCompleto { get; private set; }
        public string LegendaCompleto  { get; private set; }

        public FechamentoSituacaoPorEstudanteDto()
        {
            Ordem = 0;
            LegendaSemRegistro = "Sem registro";
            QuantidadeSemRegistro = 0;
            LegendaParcial = "Parcial";
            QuantidadeParcial = 0;
            LegendaCompleto = "Completo";
            QuantidadeCompleto = 0;
            
        }

        public void AdicionarQuantidadeSemRegistro(int quantidade)
        {
            QuantidadeSemRegistro = quantidade;
        }

        public void AdicionarQuantidadeParcial(int quantidade)
        {
            QuantidadeParcial = quantidade;
        }

        public void AdicionarQuantidadeCompleto(int quantidade)
        {
            QuantidadeCompleto = quantidade;
        }

        public void MontarDescricao(string modalidade, string nome)
        {
            Descricao = $"{modalidade} - {nome}";
        }
    }
}