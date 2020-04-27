using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralMap: DommelEntityMap<RelatorioSemestral>
    {
        public RelatorioSemestralMap()
        {
            ToTable("relatorio_semestral");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
