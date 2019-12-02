using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaRegenciaDisciplinaMap : BaseMap<AtividadeAvaliativaRegenciaDisciplina>
    {
        public AtividadeAvaliativaRegenciaDisciplinaMap()
        {
            ToTable("atividade_avaliativa_regencia_disciplina");
            Map(t => t.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(t => t.DisciplinaId).ToColumn("disciplina_id");
        }
    }
}