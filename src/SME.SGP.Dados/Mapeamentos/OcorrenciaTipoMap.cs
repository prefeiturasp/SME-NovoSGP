using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaTipoMap : BaseEntityMap<OcorrenciaTipo>
    {
        public OcorrenciaTipoMap()
        {
            ToTable("ocorrencia_tipo");
            Map(nameof(OcorrenciaTipo.Descricao), "descricao");
            Map(nameof(OcorrenciaTipo.Excluido), "excluido");
        }
    }
}