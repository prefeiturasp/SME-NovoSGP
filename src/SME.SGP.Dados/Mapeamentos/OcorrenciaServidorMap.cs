using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaServidorMap : BaseMap<OcorrenciaServidor>
    {
        public OcorrenciaServidorMap()
        {
            ToTable("ocorrencia_servidor");
            Map(nameof(OcorrenciaServidor.CodigoServidor), "rf_codigo");
            Map(nameof(OcorrenciaServidor.OcorrenciaId), "ocorrencia_id");
        }
    }
}