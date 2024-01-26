using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoConsulta : RepositorioBase<ConselhoClasseAluno>, IRepositorioConselhoClasseAlunoConsulta
    {
        public RepositorioConselhoClasseAlunoConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<ConselhoClasseAluno> ObterPorConselhoClasseAlunoCodigoAsync(long conselhoClasseId, string alunoCodigo)
        {
            var query = @"select cca.*, cc.*, pc.*
                            from conselho_classe_aluno cca
                          inner join conselho_classe cc on cc.id = cca.conselho_classe_id
                           left join conselho_classe_parecer pc on pc.id = cca.conselho_classe_parecer_id
                          where cca.conselho_classe_id = @conselhoClasseId
                            and cca.aluno_codigo = @alunoCodigo";

            return (await database.Conexao.QueryAsync<ConselhoClasseAluno, ConselhoClasse, ConselhoClasseParecerConclusivo, ConselhoClasseAluno>(query
                , (conselhoClasseAluno, conselhoClasse, parecerConclusivo) =>
                {
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    conselhoClasseAluno.ConselhoClasseParecer = parecerConclusivo;
                    return conselhoClasseAluno;
                }
                , new { conselhoClasseId, alunoCodigo })).FirstOrDefault();
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

        public async Task<ConselhoClasseAluno> ObterPorPeriodoAsync(string alunoCodigo, long turmaId, long periodoEscolarId)
        {
            var query = @"select cca.* 
                          from fechamento_turma ft
                         inner join conselho_classe cc on cc.fechamento_turma_id = ft.id 
                         inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id
                         where ft.periodo_escolar_id = @periodoEscolarId
                           and ft.turma_id = @turmaId
                           and cca.aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseAluno>(query, new { alunoCodigo, turmaId, periodoEscolarId });
        }

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> ObterNotasConselhoAlunoTurma(string alunoCodigo, string[] turmasCodigos, long? periodoEscolarId = null)
        {
            var condicaoPeriodoEscolar = periodoEscolarId.HasValue ? "ft.periodo_escolar_id = @periodoEscolarId" : "ft.periodo_escolar_id is null";
            var query = $@"select ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.conceito_id as ConceitoId, ccn.nota as Nota
                  from fechamento_turma ft
                 inner join turma t on t.id = ft.turma_id
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                 where {condicaoPeriodoEscolar}                   
                   and t.turma_id = ANY(@turmasCodigos)
                   and cca.aluno_codigo = @alunoCodigo ";

            return await database.Conexao.QueryAsync<NotaConceitoFechamentoConselhoFinalDto>(query, new { alunoCodigo, turmasCodigos, periodoEscolarId });
        }
        
        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo)
        {
            var query = $@"select distinct * from (SELECT 0                AS ConselhoClasseAlunoId,
                                   fn.disciplina_id AS ComponenteCurricularCodigo,
                                   fn.conceito_id   AS ConceitoId,
                                   fn.nota          AS Nota,
                                   fn.id            AS fechamentoNotaId
                            FROM   fechamento_turma ft
                                   LEFT JOIN periodo_escolar pe
                                          ON pe.id = ft.periodo_escolar_id
                                   INNER JOIN turma t
                                           ON t.id = ft.turma_id
                                   INNER JOIN fechamento_turma_disciplina ftd
                                           ON ftd.fechamento_turma_id = ft.id
                                   INNER JOIN fechamento_aluno fa
                                           ON fa.fechamento_turma_disciplina_id = ftd.id
                                   INNER JOIN fechamento_nota fn
                                           ON fn.fechamento_aluno_id = fa.id and not fn.excluido
                                   inner join componente_curricular ccr on fn.disciplina_id = ccr.id
                            WHERE  t.turma_id = ANY(@turmasCodigos)
                                   AND fa.aluno_codigo = @alunoCodigo
                                   AND pe.bimestre IS NULL
                                   and ccr.permite_lancamento_nota
                            UNION
                            SELECT cca.id                                    AS ConselhoClasseAlunoId,
                                   ccn.componente_curricular_codigo          AS ComponenteCurricularCodigo,
                                   coalesce (ccn.conceito_id, fn.conceito_id) AS ConceitoId,
                                   coalesce (ccn.nota, fn.nota)        AS Nota,
                                   fn.id AS fechamentoNotaId
                            FROM   fechamento_turma ft
                                   LEFT JOIN periodo_escolar pe
                                          ON pe.id = ft.periodo_escolar_id
                                   INNER JOIN turma t
                                           ON t.id = ft.turma_id
                                   INNER JOIN conselho_classe cc
                                           ON cc.fechamento_turma_id = ft.id
                                   INNER JOIN conselho_classe_aluno cca
                                           ON cca.conselho_classe_id = cc.id
                                   INNER JOIN conselho_classe_nota ccn
                                           ON ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido 
                                   inner join componente_curricular ccr on ccn.componente_curricular_codigo  = ccr.id 
                                   LEFT JOIN fechamento_turma_disciplina ftd
                                          ON ftd.fechamento_turma_id = ft.id
                                   LEFT JOIN fechamento_aluno fa
                                          ON fa.fechamento_turma_disciplina_id = ftd.id
                                             AND cca.aluno_codigo = fa.aluno_codigo
                                   LEFT JOIN fechamento_nota fn
                                          ON fn.fechamento_aluno_id = fa.id
                                             AND ccn.componente_curricular_codigo = fn.disciplina_id
                            WHERE  t.turma_id = ANY(@turmasCodigos)
                                   AND cca.aluno_codigo = @alunoCodigo
                                   AND bimestre IS NULL 
                                   and ccr.permite_lancamento_nota)  x";

            return await database.Conexao.QueryAsync<NotaConceitoFechamentoConselhoFinalDto>(query, new { turmasCodigos, alunoCodigo });
        }

        public async Task<IEnumerable<long>> ObterComponentesPorAlunoTurmaBimestreAsync(string alunoCodigo, int bimestre, long turmaId)
        {
            var query = new StringBuilder(@"select coalesce(ccn.componente_curricular_codigo, ftd.disciplina_id) as ComponenteCurricularId 
                            from conselho_classe_aluno cca 
                            left join conselho_classe_nota ccn
                                on ccn.conselho_classe_aluno_id  = cca.id and not ccn.excluido
                            inner join conselho_classe cc 
                                on cca.conselho_classe_id = cc.id
                            inner join fechamento_turma ft 
                                on cc.fechamento_turma_id  = ft.id 
                            inner join fechamento_turma_disciplina ftd
                                on ft.id = ftd.fechamento_turma_id
                            left join periodo_escolar pe 
                                on ft.periodo_escolar_id = pe.id 
                            where cca.aluno_codigo = @alunoCodigo
                            and ft.turma_id  = @turmaId");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");


            return await database.Conexao.QueryAsync<long>(query.ToString(), new { alunoCodigo, turmaId, bimestre });
        }
        public async Task<long> ObterConselhoClasseAlunoIdAsync(long conselhoClasseId, string alunoCodigo)
        {
            var query = @"select id 
                            from conselho_classe_aluno 
                            where conselho_classe_id = @conselhoClasseId 
                              and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(), new { conselhoClasseId, alunoCodigo });
        }

        public Task<IEnumerable<ConselhoClasseFechamentoAlunoDto>> ObterConselhoClasseAlunosPorTurma(string turmaCodigo)
        {
            string sqlQuery = @";with tmp_conselhos_ajuste_parecer as
                                (select distinct cca.conselho_classe_id,
                                    cc.fechamento_turma_id,
                                    cca.aluno_codigo
                                from conselho_classe_aluno cca
                                inner join conselho_classe cc
                                    on cca.conselho_classe_id = cc.id
                                inner join fechamento_turma ft
                                    on cc.fechamento_turma_id = ft.id
                                inner join turma t
                                    on ft.turma_id = t.id
                                inner join ue
                                    on t.ue_id = ue.id
                                where t.turma_id = @turmaCodigo)

                            select conselho_classe_id as ConselhoClasseId,
                                fechamento_turma_id as FechamentoTurmaId,
                                aluno_codigo as AlunoCodigo
                            from tmp_conselhos_ajuste_parecer";

            return database.Conexao.QueryAsync<ConselhoClasseFechamentoAlunoDto>(sqlQuery, new { turmaCodigo });
        }

        public async Task<IEnumerable<long>> ObterComponentesPorAlunoTurmaBimestreAsync(string alunoCodigo, int? bimestre, long turmaId)
        {
            var query = new StringBuilder(@"select distinct coalesce(ccn.componente_curricular_codigo, ftd.disciplina_id) as ComponenteCurricularId 
                            from conselho_classe_aluno cca 
                            left join conselho_classe_nota ccn
                                on ccn.conselho_classe_aluno_id  = cca.id and not ccn.excluido
                            inner join conselho_classe cc 
                                on cca.conselho_classe_id = cc.id
                            inner join fechamento_turma ft 
                                on cc.fechamento_turma_id  = ft.id 
                            inner join fechamento_turma_disciplina ftd
                                on ft.id = ftd.fechamento_turma_id
                            left join periodo_escolar pe 
                                on ft.periodo_escolar_id = pe.id 
                            where cca.aluno_codigo = @alunoCodigo
                            and ft.turma_id  = @turmaId");

            if (bimestre.HasValue && bimestre.Value > 0)
                query.AppendLine(" and pe.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");


            return await database.Conexao.QueryAsync<long>(query.ToString(), new { alunoCodigo, turmaId, bimestre });
        }

        public Task<ConselhoClasseAluno> ObterConselhoClasseAlunoPorId(long conselhoClasseAlunoId)
        {
            var query = @"select c.* 
                            from conselho_classe_aluno c 
                           where c.id = @conselhoClasseAlunoId";

            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseAluno>(query, new { conselhoClasseAlunoId });
        }
    }
}