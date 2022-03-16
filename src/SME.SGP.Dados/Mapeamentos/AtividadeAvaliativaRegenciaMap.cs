using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaRegenciaMap : BaseMap<AtividadeAvaliativaRegencia>
    {
        public AtividadeAvaliativaRegenciaMap()
        {
            ToTable("atividade_avaliativa_regencia");
            Map(c => c.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(c => c.DisciplinaContidaRegenciaId).ToColumn("disciplina_contida_regencia_id");
            Map(c => c.DisciplinaContidaRegenciaNome).Ignore();
            Map(c => c.Excluido).ToColumn("excluido");

        }
    }
}