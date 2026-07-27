using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtribuicaoEsporadicaMap : BaseEntityMap<AtribuicaoEsporadica>
    {
        public AtribuicaoEsporadicaMap()
        {
            ToTable("atribuicao_esporadica");
            Map(nameof(AtribuicaoEsporadica.DataFim), "data_fim");
            Map(nameof(AtribuicaoEsporadica.DataInicio), "data_inicio");
            Map(nameof(AtribuicaoEsporadica.DreId), "dre_id");
            Map(nameof(AtribuicaoEsporadica.Excluido), "excluido");
            Map(nameof(AtribuicaoEsporadica.Migrado), "migrado");
            Map(nameof(AtribuicaoEsporadica.ProfessorRf), "professor_rf");
            Map(nameof(AtribuicaoEsporadica.UeId), "ue_id");
            Map(nameof(AtribuicaoEsporadica.AnoLetivo), "ano_letivo");
        }
    }
}