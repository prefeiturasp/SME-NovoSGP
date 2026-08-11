using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoModalidadeMap : SimpleMap<ComunicadoModalidade>
    {
        public ComunicadoModalidadeMap()
        {
            ToTable("comunicado_modalidade");
            Map(nameof(ComunicadoModalidade.ComunicadoId), "comunicado_id");
            Map(nameof(ComunicadoModalidade.Modalidade), "modalidade");
        }
    }
}
