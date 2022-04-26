using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoRecomendacao : RepositorioBase<ConselhoClasseAlunoRecomendacao>, IRepositorioConselhoClasseAlunoRecomendacao
    {
        public RepositorioConselhoClasseAlunoRecomendacao(ISgpContext database) : base(database)
        {
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesDoAlunoPorConselho(string alunoCodigo, int bimestre, long fechamentoTurmaId)
        {
            var sql = new StringBuilder();

            sql.AppendLine("select ccar.id as Id, ccar.recomendacao as Recomendacao, ccar.tipo as Tipo from conselho_classe_aluno_recomendacao ccar");
            sql.AppendLine("inner join conselho_classe_recomendacao ccr on ccr.id = ccar.conselho_classe_recomendacao_id");
            sql.AppendLine("inner join conselho_classe_aluno cca on cca.id = ccar.conselho_classe_aluno_id");
            sql.AppendLine("inner join conselho_classe cc on cc.id = cca.conselho_classe_id");
            sql.AppendLine("inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id");
            sql.AppendLine("inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id");
            sql.AppendLine(@"where cc.aluno_codigo = @alunoCodigo");
            sql.AppendLine(@"and pe.bimestre = @bimestre");
            sql.AppendLine(@"and ft.id = @fechamentoTurmaId");

            return await database.Conexao.QueryAsync<RecomendacoesAlunoFamiliaDto>(sql.ToString(), new { alunoCodigo, bimestre, fechamentoTurmaId});
        }
    }
}
