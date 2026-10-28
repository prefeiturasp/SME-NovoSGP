using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralTurmaPAPMap : SimpleMap<RelatorioSemestralTurmaPAP>
    {
        public RelatorioSemestralTurmaPAPMap()
        {
            ToTable("relatorio_semestral_turma_pap");

            Map(nameof(RelatorioSemestralTurmaPAP.TurmaId), "turma_id");
            Map(nameof(RelatorioSemestralTurmaPAP.Semestre), "semestre");
        }
    }
}