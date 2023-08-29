using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoTurma : RepositorioBase<AcompanhamentoTurma>, IRepositorioAcompanhamentoTurma
    {
        public RepositorioAcompanhamentoTurma(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<AcompanhamentoTurma> ObterApanhadoGeralPorTurmaIdESemestre(long turmaId, int semestre)
        {
            var query = @"select at.*
                    from acompanhamento_turma at
                    where at.turma_id = @turmaId
                        and at.semestre = @semestre ";

            return await database.Conexao.QueryFirstOrDefaultAsync<AcompanhamentoTurma>(query, new { turmaId, semestre });
            
        }
    }
}
