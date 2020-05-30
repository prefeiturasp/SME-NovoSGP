using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralAlunoSecaoMap: DommelEntityMap<RelatorioSemestralPAPAlunoSecao>
    {
        public RelatorioSemestralAlunoSecaoMap()
        {
            ToTable("relatorio_semestral_pap_aluno_secao");
            Map(c => c.RelatorioSemestralPAPAlunoId).ToColumn("relatorio_semestral_pap_aluno_id");
            Map(c => c.SecaoRelatorioSemestralPAPId).ToColumn("secao_relatorio_semestral_pap_id");
        }
    }
}
