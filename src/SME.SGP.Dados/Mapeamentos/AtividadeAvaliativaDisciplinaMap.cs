using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtividadeAvaliativaDisciplinaMap : BaseMap<AtividadeAvaliativaDisciplina>
    {
        public AtividadeAvaliativaDisciplinaMap()
        {
            ToTable("atividade_avaliativa_disciplina");
            Map(nameof(AtividadeAvaliativaDisciplina.AtividadeAvaliativaId), "atividade_avaliativa_id");
            Map(nameof(AtividadeAvaliativaDisciplina.DisciplinaId), "disciplina_id");
            Map(nameof(AtividadeAvaliativaDisciplina.Excluido), "excluido");
        }
    }
}