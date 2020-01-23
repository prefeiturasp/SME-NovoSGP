using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaRegenciaMap : BaseMap<AtividadeAvaliativaRegencia>
    {
        public AtividadeAvaliativaRegenciaMap()
        {
            ToTable("atividade_avaliativa_regencia");
            Map(t => t.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(t => t.DisciplinaContidaRegenciaId).ToColumn("disciplina_contida_regencia_id");
            Map(t => t.DisciplinaContidaRegenciaNome).Ignore();
        }
    }
}