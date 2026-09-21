using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoUeMap : BaseMap<RegistroColetivoUe>
    {
        public RegistroColetivoUeMap()
        {
            ToTable("registrocoletivo_ue");
            Map(nameof(RegistroColetivoUe.UeId), "ue_id");
            Map(nameof(RegistroColetivoUe.RegistroColetivoId), "registrocoletivo_id");
        }
    }
}