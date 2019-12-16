using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroPoaMap : BaseMap<RegistroPoa>
    {
        public RegistroPoaMap()
        {
            ToTable("registro_poa");
            Map(r => r.CodigoRf).ToColumn("codigo_rf");
            Map(r => r.DreId).ToColumn("dre_id");
            Map(r => r.UeId).ToColumn("ue_id");
            Map(r => r.AnoLetivo).ToColumn("ano_letivo");
        }
    }
}