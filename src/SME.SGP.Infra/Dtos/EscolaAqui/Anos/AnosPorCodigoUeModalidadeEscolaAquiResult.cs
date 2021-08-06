using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.EscolaAqui.Anos
{
    public class AnosPorCodigoUeModalidadeEscolaAquiResult
    {
        public long Id { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public string AnoComModalidade
        {
            get => $"{Modalidade.ShortName()} - {Ano}º ano";
        }
    }
}
