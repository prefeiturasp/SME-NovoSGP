using Dapper.FluentMap.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalAbandonoUeMap : EntityMap<PainelEducacionalAbandonoUe>
    {
        public PainelEducacionalAbandonoUeMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.AnoLetivo).ToColumn("ano_letivo");
            Map(p => p.CodigoDre).ToColumn("codigo_dre");
            Map(p => p.CodigoUe).ToColumn("codigo_ue");
            Map(p => p.CodigoTurma).ToColumn("codigo_turma");
            Map(p => p.NomeTurma).ToColumn("nome_turma");
            Map(p => p.Modalidade).ToColumn("modalidade");
            Map(p => p.QuantidadeDesistencias).ToColumn("quantidade_desistencias");
            Map(p => p.CriadoEm).ToColumn("criado_em");
        }
    }
}
