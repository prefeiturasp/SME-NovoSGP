using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralAlunoSecaoMap: DommelEntityMap<RelatorioSemestralAlunoSecao>
    {
        public RelatorioSemestralAlunoSecaoMap()
        {
            ToTable("relatorio_semestral_aluno_secao");
            Map(c => c.RelatorioSemestralAlunoId).ToColumn("relatorio_semestral_aluno_id");
            Map(c => c.SecaoRelatorioSemestralId).ToColumn("secao_relatorio_semestral_id");
        }
    }
}
