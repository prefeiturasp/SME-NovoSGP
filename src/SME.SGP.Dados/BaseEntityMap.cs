using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public abstract class BaseEntityMap<T> : EntityMap<T> where T : EntidadeBase
    {
        protected BaseEntityMap()
        {
            MapBaseProperties();
        }

        protected virtual void MapBaseProperties()
        {
            Map(nameof(EntidadeBase.Id), "id");
            Map(nameof(EntidadeBase.CriadoEm), "criado_em");
            Map(nameof(EntidadeBase.CriadoPor), "criado_por");
            Map(nameof(EntidadeBase.AlteradoEm), "alterado_em");
            Map(nameof(EntidadeBase.AlteradoPor), "alterado_por");
            Map(nameof(EntidadeBase.AlteradoRF), "alterado_rf");
            Map(nameof(EntidadeBase.CriadoRF), "criado_rf");
        }
    }
}
