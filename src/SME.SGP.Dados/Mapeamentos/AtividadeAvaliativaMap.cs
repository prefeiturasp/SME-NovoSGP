using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaMap : BaseMap<AtividadeAvaliativa>
    {
        public AtividadeAvaliativaMap()
        {
            ToTable("atividade_avaliativa");
            Map(nameof(AtividadeAvaliativa.Categoria), "categoria_id");
            Map(nameof(AtividadeAvaliativa.DataAvaliacao), "data_avaliacao");
            Map(nameof(AtividadeAvaliativa.DescricaoAvaliacao), "descricao_avaliacao");
            Map(nameof(AtividadeAvaliativa.DreId), "dre_id");
            Map(nameof(AtividadeAvaliativa.EhCj), "eh_cj");
            Map(nameof(AtividadeAvaliativa.EhRegencia), "eh_regencia");
            Map(nameof(AtividadeAvaliativa.Excluido), "excluido");
            Map(nameof(AtividadeAvaliativa.NomeAvaliacao), "nome_avaliacao");
            Map(nameof(AtividadeAvaliativa.ProfessorRf), "professor_rf");
            Map(nameof(AtividadeAvaliativa.TipoAvaliacaoId), "tipo_avaliacao_id");
            Map(nameof(AtividadeAvaliativa.TurmaId), "turma_id");
            Map(nameof(AtividadeAvaliativa.UeId), "ue_id");
            Map(nameof(AtividadeAvaliativa.AtividadeClassroomId), "atividade_classroom_id");
        }
    }
}