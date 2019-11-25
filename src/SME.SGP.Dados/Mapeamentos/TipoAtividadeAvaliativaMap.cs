using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoAtividadeAvaliativaMap : BaseMap<TipoAtividadeAvaliativa>
    {
        public TipoAtividadeAvaliativaMap()
        {
            ToTable("tipo_atividade_avalativa");
            Map(t => t.Nome).ToColumn("nome");
            Map(t => t.Descricao).ToColumn("descricao");
            Map(t => t.Excluido).ToColumn("excluido");
            Map(t => t.Situacao).ToColumn("situacao");
        }
    }
}