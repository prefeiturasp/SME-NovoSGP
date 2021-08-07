using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.EscolaAqui.Anos
{
    public class AnosPorCodigoUeModalidadeEscolaAquiResult
    {   
        public string Ano { get; set; }
        public string Descricao
        {
            get => $"{Ano}º ano";
        }
    }
}
