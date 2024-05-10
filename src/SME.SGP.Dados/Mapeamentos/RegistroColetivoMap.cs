using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RegistroColetivoMap : BaseMap<RegistroColetivo>
    {
        public RegistroColetivoMap()
        {
            ToTable("registrocoletivo");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TipoReuniaoId).ToColumn("tipo_reuniao_id");
            Map(c => c.DataRegistro).ToColumn("data_registro");
            Map(c => c.QuantidadeParticipantes).ToColumn("quantidade_participantes");
            Map(c => c.QuantidadeEducadores).ToColumn("quantidade_educadores");
            Map(c => c.QuantidadeEducandos).ToColumn("quantidade_educandos");
            Map(c => c.QuantidadeCuidadores).ToColumn("quantidade_cuidadores");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
