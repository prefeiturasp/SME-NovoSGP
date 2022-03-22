using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaDisciplinaMap : BaseMap<AtividadeAvaliativaDisciplina>
    {
        public AtividadeAvaliativaDisciplinaMap()
        {
            ToTable("atividade_avaliativa_disciplina");
            Map(c => c.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}