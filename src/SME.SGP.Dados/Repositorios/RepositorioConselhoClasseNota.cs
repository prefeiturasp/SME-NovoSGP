using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseNota : RepositorioBase<ConselhoClasseNota>, IRepositorioConselhoClasseNota
    {
        public RepositorioConselhoClasseNota(ISgpContext database) : base(database)
        {
        }

        public async Task<ConselhoClasseNota> ObterPorConselhoClasseAlunoComponenteCurricularAsync(long conselhoClasseAlunoId, long componenteCurricularCodigo)
        {
            var query = @"select * 
                        from conselho_classe_nota 
                        where conselho_classe_aluno_id = @conselhoClasseAlunoId
                        and componente_curricular_codigo = @componenteCurricularCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseNota>(query.ToString(), new { conselhoClasseAlunoId, componenteCurricularCodigo });
        }
    }
}