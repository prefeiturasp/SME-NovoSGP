using SME.SGP.Dominio;
namespace SME.SGP.Dados
{
    public class SecaoMapeamentoEstudanteMap : BaseMap<SecaoMapeamentoEstudante>
    {
        public SecaoMapeamentoEstudanteMap()
        {
            ToTable("secao_mapeamento_estudante");
            Map(c => c.QuestionarioId).ToColumn("questionario_id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.NomeComponente).ToColumn("nome_componente");
        }
    }
}
