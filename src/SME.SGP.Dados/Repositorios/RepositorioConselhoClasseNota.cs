using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseNota : RepositorioBase<ConselhoClasseNota>, IRepositorioConselhoClasseNota
    {
        public RepositorioConselhoClasseNota(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(long conselhoClasseId, string alunoCodigo)
        {
            var query = @"select ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.conceito_id as ConceitoId, ccn.nota
                          from conselho_classe_aluno cca 
                         inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id
                          where cca.aluno_codigo = @alunoCodigo
                            and cca.conselho_classe_id = @conselhoClasseId ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { conselhoClasseId, alunoCodigo });
        }
    }
}