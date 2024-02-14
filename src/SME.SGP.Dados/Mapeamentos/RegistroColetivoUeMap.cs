using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoUeMap : BaseMap<RegistroColetivoUe>
    {
        public RegistroColetivoUeMap()
        {
            ToTable("registrocoletivo_ue");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.RegistroColetivoId).ToColumn("registrocoletivo_id");
        }
    }
}
