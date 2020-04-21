using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAluno : RepositorioBase<ConselhoClasseAluno>, IRepositorioConselhoClasseAluno
    {
        public RepositorioConselhoClasseAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo)
        {
            var query = @"select cca.*, cc.*
                            from conselho_classe_aluno cca
                          inner join conselho_classe cc on cc.id = cca.conselho_classe_id
                          where cca.conselho_classe_id = @conselhoClasseId
                            and cca.aluno_codigo = @alunoCodigo";

            return (await database.Conexao.QueryAsync<ConselhoClasseAluno, ConselhoClasse, ConselhoClasseAluno>(query
                , (conselhoClasseAluno, conselhoClasse) =>
                {
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    return conselhoClasseAluno;
                }
                , new { conselhoClasseId, alunoCodigo })).FirstOrDefault();
        }

        public async Task<ConselhoClasseAluno> ObterPorFechamentoAsync(long fechamentoTurmaId, string alunoCodigo)
        {
            var query = @"select cca.* 
                          from conselho_classe cc
                         inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id
                         where cc.fechamento_turma_id = @fechamentoTurmaId
                           and cca.aluno_codigo = @alunoCodigo";

            return await database.QueryFirstOrDefaultAsync<ConselhoClasseAluno>(query, new { fechamentoTurmaId, alunoCodigo });
        }

        public async Task<ConselhoClasseAluno> ObterPorFiltrosAsync(string codigoTurma, string codigoAluno, int bimestre, bool EhFinal)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select cca.*, cc.* from fechamento_turma ft");
            query.AppendLine("inner");
            query.AppendLine("join conselho_classe cc");
            query.AppendLine("on cc.fechamento_turma_id = ft.id");
            query.AppendLine("inner");
            query.AppendLine("join conselho_classe_aluno cca");
            query.AppendLine("on cca.conselho_classe_id = cc.id");
            query.AppendLine("inner");
            query.AppendLine("join turma t");
            query.AppendLine("on t.id = ft.turma_id");

            if (!EhFinal)
            {
                query.AppendLine("inner join periodo_escolar p");
                query.AppendLine("on ft.periodo_escolar_id = p.id");
            }

            query.AppendLine("where t.turma_id = @codigoTurma");
            query.AppendLine("and cca.aluno_codigo = @codigoAluno");

            if (EhFinal)
                query.AppendLine("and ft.periodo_escolar_id is null");
            else
                query.AppendLine("and p.bimestre = @bimestre");

            return (await database.Conexao.QueryAsync<ConselhoClasseAluno, ConselhoClasse, ConselhoClasseAluno>(query.ToString()
                , (conselhoClasseAluno, conselhoClasse) =>
                {
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    return conselhoClasseAluno;
                }
                , new { codigoTurma, codigoAluno, bimestre })).FirstOrDefault();
        }
    }
}