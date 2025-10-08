using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalConsolidacaoPapUeMap : DommelEntityMap<PainelEducacionalConsolidacaoPapUe>
    {
        public PainelEducacionalConsolidacaoPapUeMap()
        {
            ToTable("painel_educacional_consolidacao_pap_ue");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.TipoPap).ToColumn("tipo_pap");
            Map(c => c.CodigoDre).ToColumn("codigo_dre");
            Map(c => c.CodigoUe).ToColumn("codigo_ue");
            Map(c => c.TotalTurmas).ToColumn("total_turmas");
            Map(c => c.TotalAlunos).ToColumn("total_alunos");
            Map(c => c.TotalAlunosComFrequenciaInferiorLimite).ToColumn("total_alunos_com_frequencia_inferior_limite");
            Map(c => c.TotalAlunosDificuldadeTop1).ToColumn("total_alunos_dificuldade_top_1");
            Map(c => c.TotalAlunosDificuldadeTop2).ToColumn("total_alunos_dificuldade_top_2");
            Map(c => c.TotalAlunosDificuldadeOutras).ToColumn("total_alunos_dificuldade_outras");
            Map(c => c.NomeDificuldadeTop1).ToColumn("nome_dificuldade_top_1");
            Map(c => c.NomeDificuldadeTop2).ToColumn("nome_dificuldade_top_2");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
