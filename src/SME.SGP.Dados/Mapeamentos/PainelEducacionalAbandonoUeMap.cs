using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalAbandonoUeMap : SimpleEntityMap<PainelEducacionalAbandonoUe>
    {
        public PainelEducacionalAbandonoUeMap()
        {
            ToTable("painel_educacional_consolidacao_abandono_ue");
            Map(nameof(PainelEducacionalAbandonoUe.AnoLetivo), "ano_letivo");
            Map(nameof(PainelEducacionalAbandonoUe.CodigoDre), "codigo_dre");
            Map(nameof(PainelEducacionalAbandonoUe.CodigoUe), "codigo_ue");
            Map(nameof(PainelEducacionalAbandonoUe.CodigoTurma), "codigo_turma");
            Map(nameof(PainelEducacionalAbandonoUe.NomeTurma), "nome_turma");
            Map(nameof(PainelEducacionalAbandonoUe.Modalidade), "modalidade");
            Map(nameof(PainelEducacionalAbandonoUe.QuantidadeDesistencias), "quantidade_desistencias");
            Map(nameof(PainelEducacionalAbandonoUe.CriadoEm), "criado_em");
        }
    }
}