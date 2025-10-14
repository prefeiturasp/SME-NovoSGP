using Dapper.FluentMap.Mapping;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalAbandonoMap : EntityMap<PainelEducacionalAbandono>
    {
        public PainelEducacionalAbandonoMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.AnoLetivo).ToColumn("ano_letivo");
            Map(p => p.CodigoDre).ToColumn("codigo_dre");
            Map(p => p.CodigoUe).ToColumn("codigo_ue");
            Map(p => p.Ano).ToColumn("ano");
            Map(p => p.QuantidadeDesistencias).ToColumn("quantidade_desistencias");
            Map(p => p.Modalidade).ToColumn("modalidade");
            Map(p => p.Turma).ToColumn("turma");
            Map(p => p.CriadoEm).ToColumn("criado_em");
        }
    }
}
