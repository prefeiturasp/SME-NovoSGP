using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoTipoMap : BaseMap<EventoTipo>
    {
        public EventoTipoMap()
        {
            ToTable("evento_tipo");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.Concomitancia).ToColumn("concomitancia");
            Map(c => c.Dependencia).ToColumn("dependencia");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Letivo).ToColumn("letivo");
            Map(c => c.LocalOcorrencia).ToColumn("local_ocorrencia");
            Map(c => c.TipoData).ToColumn("tipo_data");
            Map(c => c.SomenteLeitura).ToColumn("somente_leitura");
            Map(c => c.EventoEscolaAqui).ToColumn("evento_escolaaqui");
        }
    }
}
