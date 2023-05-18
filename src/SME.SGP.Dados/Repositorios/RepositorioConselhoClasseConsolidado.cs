using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidado : RepositorioBase<ConselhoClasseConsolidadoTurmaAluno>, IRepositorioConselhoClasseConsolidado
    {
        public RepositorioConselhoClasseConsolidado(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> ExcluirLogicamenteConsolidacaoConselhoClasseAlunoTurmaPorIdConsolidacao(long[] idsConsolidacoes)
        {
            var sqlQuery = @"update consolidado_conselho_classe_aluno_turma
                             set excluido = true
                             where id = any(@idsConsolidacoes);";

            await database.Conexao
                .ExecuteAsync(sqlQuery, new { idsConsolidacoes });

            return true;
        }
    }
}
