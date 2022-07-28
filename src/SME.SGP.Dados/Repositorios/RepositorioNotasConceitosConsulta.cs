﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotasConceitosConsulta : RepositorioBase<NotaConceito>, IRepositorioNotasConceitosConsulta
    {
        public RepositorioNotasConceitosConsulta(ISgpContextConsultas sgpContext, IServicoAuditoria servicoAuditoria) : base(sgpContext, servicoAuditoria)
        {
        }

        public IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas, IEnumerable<string> alunosIds, string disciplinaId)
        {
            var atividadesAvaliativasString = string.Join(",", atividadesAvaliativas.Distinct());
            var alunosIdsString = $"'{string.Join("','", alunosIds.Distinct())}'";

            var sql = $@"select id, 
                                atividade_avaliativa, 
                                aluno_id, 
                                nota, 
                                conceito, 
                                tipo_nota, 
                                criado_em,
                                criado_por, 
                                criado_rf, 
                                alterado_em, 
                                alterado_por, 
                                alterado_rf
                         from notas_conceito 
                         where atividade_avaliativa = any(array[{atividadesAvaliativasString}]) 
                            and aluno_id = any(array[{alunosIdsString}])
                            and disciplina_id = @disciplinaId";

            return database.Query<NotaConceito>(sql, new { disciplinaId });
        }
        public async Task<IEnumerable<NotaConceito>> ObterNotasPorAlunosAtividadesAvaliativasAsync(long[] atividadesAvaliativasId, string[] alunosIds, string componenteCurricularId)
        {

            var sql = $@"select id, 
                                atividade_avaliativa, 
                                aluno_id, 
                                nota, 
                                conceito, 
                                tipo_nota, 
                                criado_em,
                                criado_por, 
                                criado_rf, 
                                alterado_em, 
                                alterado_por, 
                                alterado_rf,
                                status_gsa
                         from notas_conceito 
                         where atividade_avaliativa = any(@atividadesAvaliativasId) 
                            and aluno_id = any(@alunosIds)
                            and disciplina_id = @componenteCurricularId";

            return await database.QueryAsync<NotaConceito>(sql, new { atividadesAvaliativasId, alunosIds, componenteCurricularId });
        }

        public async Task<NotaConceito> ObterNotasPorAtividadeIdCodigoAluno(long atividadeId,string codigoAluno)
        {
            var sql = $@"select nc.id, 
                                nc.atividade_avaliativa, 
                                nc.aluno_id, 
                                nc.nota, 
                                nc.conceito, 
                                nc.tipo_nota, 
                                nc.criado_em,
                                nc.criado_por, 
                                nc.criado_rf, 
                                nc.alterado_em, 
                                nc.alterado_por, 
                                nc.alterado_rf
                         from notas_conceito nc
                         inner join atividade_avaliativa aa on nc.atividade_avaliativa = aa.id 
                         where nc.atividade_avaliativa = @atividadeId and nc.aluno_id = @codigoAluno ";

            return await database.QuerySingleOrDefaultAsync<NotaConceito>(sql, new { atividadeId, codigoAluno });
        }

        public async Task<NotaConceito> ObterNotasPorId(long id)
        {
            var sql = $@"select * from notas_conceito nc where nc.id = @id";

            return await database.QuerySingleOrDefaultAsync<NotaConceito>(sql, new { id });
        }

        public Task<double> ObterNotaEmAprovacao(string codigoAluno, long disciplinaId, long turmaFechamentoId)
        {
            var sql = $@"select coalesce(coalesce(w.nota,w.conceito_id),-1)
                            from fechamento_turma ft 
                            inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                            inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                            inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                            inner join wf_aprovacao_nota_fechamento w on w.fechamento_nota_id = fn.id
                            where ft.id = @turmaFechamentoId and fn.disciplina_id = @disciplinaId and fa.aluno_codigo = @codigoAluno
                        order by w.id desc";

            return database.QueryFirstOrDefaultAsync<double>(sql, new { turmaFechamentoId, disciplinaId, codigoAluno });
        }

    }
}