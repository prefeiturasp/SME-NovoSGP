using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaMap : BaseMap<AtividadeAvaliativa>
    {
        public AtividadeAvaliativaMap()
        {
            ToTable("atividade_avaliativa");
            Map(t => t.Categoria).ToColumn("categoria_id");
            Map(c => c.DataAvaliacao).ToColumn("data_avaliacao");
            Map(c => c.DescricaoAvaliacao).ToColumn("descricao_avaliacao");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.EhCj).ToColumn("eh_cj");
            Map(c => c.EhRegencia).ToColumn("eh_regencia");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.NomeAvaliacao).ToColumn("nome_avaliacao");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.TipoAvaliacaoId).ToColumn("tipo_avaliacao_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.AtividadeClassroomId).ToColumn("atividade_classroom_id");
        }
    }
}