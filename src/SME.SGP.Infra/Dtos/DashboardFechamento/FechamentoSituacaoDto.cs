namespace SME.SGP.Infra.Dtos
{
    public class FechamentoSituacaoDto
    {
        public int Ordem { get;  set; }
        public string Descricao { get; private set; }
        public string LegendaProcessadoPendencia { get; private set; }
        public int QuantidadeProcessadoPendencia { get; private set; }
        public string LegendaProcessadoSucesso { get; private set; }
        public int QuantidadeProcessadoSucesso { get; private set; }

        public string LegendaNaoIniciado  { get; private set; }

        public FechamentoSituacaoDto()
        {
            Ordem = 0;
            LegendaProcessadoPendencia = "Processado com Pendências";
            QuantidadeProcessadoPendencia = 0;
            LegendaProcessadoSucesso = "Processado com sucesso";
            QuantidadeProcessadoSucesso = 0;
            QuantidadeNaoIniciado = 0;
            LegendaNaoIniciado = "Não Iniciado";
        }



        public int QuantidadeNaoIniciado { get; private set; }


        public void AdicionarValorProcessadoNaoIniciado(int quantidade)
        {
            QuantidadeNaoIniciado = quantidade;
        }

        public void AdicionarValorProcessadoPendente(int quantidade)
        {
            QuantidadeProcessadoPendencia = quantidade;
        }

        public void AdicionarValorProcessadoSucesso(int quantidade)
        {
            QuantidadeProcessadoSucesso = quantidade;
        }

        public void MontarDescricao(string modalidade, int ano)
        {
            Descricao = $"{modalidade} - {ano}";
        }
    }
}