using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UeMap : SimpleMap<Ue>
    {
        public UeMap()
        {
            ToTable("ue");
            Map(nameof(Ue.CodigoUe), "ue_id");
            Map(nameof(Ue.DataAtualizacao), "data_atualizacao");
            Map(nameof(Ue.DreId), "dre_id");
            Map(nameof(Ue.Nome), "nome");
            Map(nameof(Ue.TipoEscola), "tipo_escola");
        }
    }
}