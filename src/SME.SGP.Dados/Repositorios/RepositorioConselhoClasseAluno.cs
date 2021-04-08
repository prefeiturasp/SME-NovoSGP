using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo)
        {
            var query = $@"select distinct * from (
                                select cca.id as ConselhoAlunoId, 
                                       fn.disciplina_id as ComponenteCurricularCodigo, 
                                       coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, 
                                       coalesce(ccn.nota, fn.nota) as Nota
                                  from fechamento_turma ft
                                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                 inner join turma t on t.id = ft.turma_id 
                                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                                 left join componente_curricular ccr on ccr.id = fn.disciplina_id 
                                  left join conselho_classe cc on cc.fechamento_turma_id = ft.id
                                  left join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
		                                                        and cca.aluno_codigo = fa.aluno_codigo 
                                  left join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id 
		                                                        and ccn.componente_curricular_codigo = fn.disciplina_id 
                                 where t.turma_id = ANY(@turmasCodigos)
                                   and fa.aluno_codigo = @alunoCodigo
                                   and bimestre is null
                                union all 
                                select cca.id as ConselhoAlunoId, 
                                       ccn.componente_curricular_codigo as ComponenteCurricularCodigo, 
                                       coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId,
                                       coalesce(ccn.nota, fn.nota) as Nota
                                  from fechamento_turma ft
                                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                 inner join turma t on t.id = ft.turma_id 
                                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id                  
                                 left join componente_curricular ccr on ccr.id = ccn.componente_curricular_codigo 
                                  left join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                                  left join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
		                                                        and cca.aluno_codigo = fa.aluno_codigo 
                                  left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
		                                                        and ccn.componente_curricular_codigo = fn.disciplina_id 
                                 where t.turma_id = ANY(@turmasCodigos)
                                   and cca.aluno_codigo = @alunoCodigo
                                   and bimestre is null
                                ) x ";

            return await database.Conexao.QueryAsync<NotaConceitoFechamentoConselhoFinalDto>(query, new { turmasCodigos, alunoCodigo });
        }
    }
}