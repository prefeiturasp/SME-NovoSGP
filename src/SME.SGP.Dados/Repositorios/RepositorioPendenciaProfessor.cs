using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<bool> ExistePendenciaProfessorPorPendenciaId(long pendenciaId)
        {
            var query = "select 1 from pendencia_professor where pendencia_id = @pendenciaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { pendenciaId });
        }

        public async Task<bool> ExistePendenciaProfessorPorTurmaEComponente(long turmaId, long componenteCurricularId, long? periodoEscolarId, string professorRf, TipoPendencia tipoPendencia)
        {
            var query = new StringBuilder(@"select 1
                         from pendencia_professor pp
                        inner join pendencia p on p.id = pp.pendencia_id
                        where pp.turma_id = @turmaId
                          and pp.componente_curricular_id = @componenteCurricularId
                          and pp.professor_rf = @professorRf
                          and p.tipo = @tipoPendencia ");

            if (periodoEscolarId.HasValue)
                query.Append("and pp.periodo_escolar_id = @periodoEscolarId");
            else
                query.Append("and pp.periodo_escolar_id is null");


            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query.ToString(), new { turmaId, componenteCurricularId, periodoEscolarId, professorRf, tipoPendencia });
        }

        public async Task<long> Inserir(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf, long? periodoEscolarId)
            => (long)database.Conexao.Insert(new PendenciaProfessor(pendenciaId, turmaId, componenteCurricularId, professorRf, periodoEscolarId));

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

        public async Task<long> ObterPendenciaIdPorTurmaCCPeriodoEscolar(long turmaId, long componenteCurricularId, long periodoEscolarId, TipoPendencia tipoPendencia)
        {
            var query = @"select pp.pendencia_id
                         from pendencia_professor pp
                        inner join pendencia p on p.id = pp.pendencia_id
                        where not p.excluido
                          and pp.turma_id = @turmaId
                          and pp.periodo_escolar_id = @componenteCurricularId
                          and pp.componente_curricular_id = @periodoEscolarId
                          and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, componenteCurricularId, periodoEscolarId, tipoPendencia });
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

        public async Task<IEnumerable<PendenciaProfessor>> ObterPendenciasProfessorPorTurmaEComponente(long turmaId, long[] componentesCurriculares, long? periodoEscolarId, TipoPendencia tipoPendencia)
        {
            var condicaoPeriodoEscolar = periodoEscolarId.HasValue ?
                "and pp.periodo_escolar_id = @periodoEscolarId" :
                "and pp.periodo_escolar_id is null";

            var query = $@"select pp.* 
                          from pendencia_professor pp 
                          inner join pendencia p on p.id = pp.pendencia_id
                          where not p.excluido
                            and p.tipo = @tipoPendencia
                            and pp.turma_id = @turmaId
                            and pp.componente_curricular_id = any(@componentesCurriculares)
                            {condicaoPeriodoEscolar}";

            return await database.Conexao.QueryAsync<PendenciaProfessor>(query, new { turmaId, componentesCurriculares, tipoPendencia, periodoEscolarId });
        }

        public async Task<Turma> ObterTurmaDaPendencia(long pendenciaId)
        {
            var query = @"select t.* 
                          from pendencia_professor pp
                         inner join turma t on t.id = pp.turma_id
                         where pp.pendencia_id = @pendenciaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { pendenciaId });
        }

        public async Task Remover(PendenciaProfessor pendenciaProfessor)
        {
            await database.Conexao.DeleteAsync(pendenciaProfessor);
            Auditar(pendenciaProfessor.Id, "E");
        }

        private void Auditar(long identificador, string acao)
        {
            database.Conexao.Insert<Auditoria>(new Auditoria()
            {
                Data = DateTime.Now,
                Entidade = "pendenciaprofessor",
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Acao = acao
            });
        }


    }
}
