using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroPoaMap : BaseMap<RegistroPoa>
    {
        public RegistroPoaMap()
        {
            ToTable("registro_poa");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.CodigoRf).ToColumn("codigo_rf");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.UeId).ToColumn("ue_id");
        }
    }
}