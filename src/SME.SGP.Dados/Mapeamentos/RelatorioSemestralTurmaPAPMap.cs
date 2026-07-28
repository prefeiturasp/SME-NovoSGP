using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralTurmaPAPMap : SimpleEntityMap<RelatorioSemestralTurmaPAP>
    {
        public RelatorioSemestralTurmaPAPMap()
        {
            ToTable("relatorio_semestral_turma_pap");

            Map(nameof(RelatorioSemestralTurmaPAP.TurmaId), "turma_id");
            Map(nameof(RelatorioSemestralTurmaPAP.Semestre), "semestre");
        }
    }
}