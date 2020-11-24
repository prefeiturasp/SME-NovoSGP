using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaProfessor : IRepositorioPendenciaProfessor
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaProfessor(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<bool> ExistePendenciaProfessorPorTurmaEComponente(long turmaId, long componenteCurricularId, string professorRf, TipoPendencia tipoPendencia)
        {
            var query = @"select 1
                         from pendencia_professor pp
                        inner join pendencia p on p.id = pp.pendencia_id
                        where pp.turma_id = @turmaId
                          and pp.componente_curricular_id = @componenteCurricularId
                          and pp.professor_rf = @professorRf
                          and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { turmaId, componenteCurricularId, professorRf, tipoPendencia });
        }

        public async Task<long> Inserir(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf)
            => (long)database.Conexao.Insert(new PendenciaProfessor(pendenciaId, turmaId, componenteCurricularId, professorRf));

        public async Task<long> ObterPendenciaIdPorTurma(long turmaId, TipoPendencia tipoPendencia)
        {
            var query = @"select pp.pendencia_id
                         from pendencia_professor pp
                        inner join pendencia p on p.id = pp.pendencia_id
                        where not p.excluido
                          and pp.turma_id = @turmaId
                          and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, tipoPendencia });
        }

        public async Task<IEnumerable<PendenciaProfessorDto>> ObterPendenciasPorPendenciaId(long pendenciaId)
        {
            var query = @"select coalesce(cc.descricao_sgp, cc.descricao) as ComponenteCurricular, u.rf_codigo as ProfessorRf, u.nome as Professor
                          from pendencia_professor pp 
                         inner join componente_curricular cc on cc.id = pp.componente_curricular_id
                         inner join usuario u on u.rf_codigo = pp.professor_rf
                         where pp.pendencia_id = @pendenciaId";

            return await database.Conexao.QueryAsync<PendenciaProfessorDto>(query, new { pendenciaId });
        }
    }
}
