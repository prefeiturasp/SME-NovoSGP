﻿using Dapper;
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
    public class RepositorioFechamentoAlunoConsulta : RepositorioBase<FechamentoAluno>, IRepositorioFechamentoAlunoConsulta
    {
        public RepositorioFechamentoAlunoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> ObterAlunosComFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
        
            var sqlQuery = new StringBuilder(@"select
                                t.id as TurmaId,
                                fa.aluno_codigo as AlunoCodigo,
	                            count(fa.fechamento_turma_disciplina_id) as QuantidadeDisciplinaFechadas
                                from fechamento_aluno fa
                            inner join fechamento_turma_disciplina ftd on fa.fechamento_turma_disciplina_id = ftd.id 
                            inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id 
                            inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                            inner join turma t on ft.turma_id = t.id
                            inner join ue on t.ue_id = ue.id 
                            where t.ano_letivo = @ano ");


            if (ueId > 0)
            {
                sqlQuery.Append(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                sqlQuery.Append(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                sqlQuery.Append(" and t.modalidade_codigo = @modalidade ");
            }

            if (semestre > 0)
            {
                sqlQuery.Append(" and t.semestre = @semestre ");
            }

            if (bimestre >= 0)
            {
                sqlQuery.Append(" and pe.bimestre = @bimestre ");
            }

            sqlQuery.Append(@" group by t.id, fa.aluno_codigo order by t.ano;");


            return await database.Conexao.QueryAsync<TurmaAlunoBimestreFechamentoDto>(sqlQuery.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        public async Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select *
                        from fechamento_aluno
                        where fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                          and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoAluno>(query, new { fechamentoTurmaDisciplinaId, alunoCodigo });
        }

        public async Task<FechamentoAluno> ObterFechamentoAlunoENotas(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select a.*, n.*
                            from fechamento_aluno a
                           inner join fechamento_nota n on n.fechamento_aluno_id = a.id
                           where a.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                             and a.aluno_codigo = @alunoCodigo";

            FechamentoAluno fechamentoAlunoRetorno = null;
            await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    if (fechamentoAlunoRetorno == null)
                        fechamentoAlunoRetorno = fechamentoAluno;

                    fechamentoAlunoRetorno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId, alunoCodigo });

            return fechamentoAlunoRetorno;
        }

        public async Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select fa.*, n.*
                            from fechamento_aluno fa
                           inner join fechamento_nota n on n.fechamento_aluno_id = fa.id
                           where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            List<FechamentoAluno> fechamentosAlunos = new List<FechamentoAluno>();

            await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    var fechamentoAlunoLista = fechamentosAlunos.FirstOrDefault(a => a.Id == fechamentoAluno.Id);
                    if (fechamentoAlunoLista == null)
                    {
                        fechamentoAlunoLista = fechamentoAluno;
                        fechamentosAlunos.Add(fechamentoAluno);
                    }
                    fechamentoAlunoLista.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId });

            return fechamentosAlunos;
        }
    }
}