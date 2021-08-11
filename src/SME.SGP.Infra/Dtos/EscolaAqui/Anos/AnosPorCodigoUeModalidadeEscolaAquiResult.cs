namespace SME.SGP.Infra.Dtos.EscolaAqui.Anos
{
    public class AnosPorCodigoUeModalidadeEscolaAquiResult
    {
        public string Ano { get; set; }
        public string Descricao
        {
            get => Ano != "-99" ? $"{Ano}º ano" : "Todos";
        }
    }
}
