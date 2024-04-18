using SME.SGP.Dominio;
namespace SME.SGP.Dados
{
    public class MapeamentoEstudanteSecaoMap : BaseMap<MapeamentoEstudanteSecao>
    {
        public MapeamentoEstudanteSecaoMap()
        {
            ToTable("mapeamento_estudante_secao");
            Map(c => c.MapeamentoEstudanteId).ToColumn("mapeamento_estudante_id");
            Map(c => c.SecaoMapeamentoEstudanteId).ToColumn("secao_mapeamento_estudante_id");
            Map(c => c.Concluido).ToColumn("concluido");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
