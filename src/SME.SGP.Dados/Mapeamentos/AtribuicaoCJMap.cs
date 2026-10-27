using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AtribuicaoCJMap : BaseMap<AtribuicaoCJ>
    {
        public AtribuicaoCJMap()
        {
            ToTable("atribuicao_cj");
            Map(nameof(AtribuicaoCJ.DisciplinaId), "disciplina_id");
            Map(nameof(AtribuicaoCJ.DreId), "dre_id");
            Map(nameof(AtribuicaoCJ.Migrado), "migrado");
            Map(nameof(AtribuicaoCJ.Modalidade), "modalidade");
            Map(nameof(AtribuicaoCJ.ProfessorRf), "professor_rf");
            Map(nameof(AtribuicaoCJ.Substituir), "substituir");
            Map(nameof(AtribuicaoCJ.TurmaId), "turma_id");
            Map(nameof(AtribuicaoCJ.UeId), "ue_id");
        }
    }
}