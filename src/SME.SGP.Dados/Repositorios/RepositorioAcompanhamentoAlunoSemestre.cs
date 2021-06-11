using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAlunoSemestre : RepositorioBase<AcompanhamentoAlunoSemestre>, IRepositorioAcompanhamentoAlunoSemestre
    {
        public RepositorioAcompanhamentoAlunoSemestre(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<int> ObterAnoPorId(long acompanhamentoAlunoSemestreId)
        {
            var query = @"select ano_letivo
                          from acompanhamento_aluno_semestre aas 
                         inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id 
                         inner join turma t on t.id = aa.turma_id 
                         where aas.id = @acompanhamentoAlunoSemestreId";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { acompanhamentoAlunoSemestreId });
        }
    }
}
