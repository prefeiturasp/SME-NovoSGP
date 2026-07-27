using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoMap : BaseEntityMap<Informativo>
    {
        public InformativoMap()
        {
            ToTable("informativo");
            Map(nameof(Informativo.DreId), "dre_id");
            Map(nameof(Informativo.UeId), "ue_id");
            Map(nameof(Informativo.Titulo), "titulo");
            Map(nameof(Informativo.Texto), "texto");
            Map(nameof(Informativo.DataEnvio), "data_envio");
            Map(nameof(Informativo.Excluido), "excluido");
        }
    }
}