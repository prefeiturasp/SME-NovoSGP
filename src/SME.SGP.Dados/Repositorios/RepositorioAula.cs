using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplina(string turma, string disciplina)
        {
            var query = @"select professor_id, quantidade, data_aula 
                 from aula 
                where turma_id = @turma 
                  and disciplina_id = @disciplina";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                disciplina
            });
        }
    }
}
