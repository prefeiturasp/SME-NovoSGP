using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoOcorrenciaMap : BaseMap<TipoOcorrencia>
    {
        public TipoOcorrenciaMap()
        {
            ToTable("tipo_ocorrencia");
            Map(x => x.Descricao).ToColumn("descricao");
        }
    }
}