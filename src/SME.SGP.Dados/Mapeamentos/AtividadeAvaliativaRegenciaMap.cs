using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaRegenciaMap : BaseMap<AtividadeAvaliativaRegencia>
    {
        public AtividadeAvaliativaRegenciaMap()
        {
            ToTable("atividade_avaliativa_regencia");
            Map(nameof(AtividadeAvaliativaRegencia.AtividadeAvaliativaId), "atividade_avaliativa_id");
            Map(nameof(AtividadeAvaliativaRegencia.DisciplinaContidaRegenciaId), "disciplina_contida_regencia_id");
            Map(nameof(AtividadeAvaliativaRegencia.Excluido), "excluido");
        }
    }
}