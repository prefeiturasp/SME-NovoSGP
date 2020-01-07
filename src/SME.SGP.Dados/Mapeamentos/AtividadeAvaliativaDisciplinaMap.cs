using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaDisciplinaMap : BaseMap<AtividadeAvaliativaDisciplina>
    {
        public AtividadeAvaliativaDisciplinaMap()
        {
            ToTable("atividade_avaliativa_disciplina");
            Map(t => t.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(t => t.DisciplinaId).ToColumn("disciplina_id");
        }
    }
}