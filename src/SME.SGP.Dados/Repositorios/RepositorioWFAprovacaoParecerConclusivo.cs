﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoParecerConclusivo : IRepositorioWFAprovacaoParecerConclusivo
    {
        private readonly ISgpContext database;

        public RepositorioWFAprovacaoParecerConclusivo(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(long id)
        {
            var query = @"delete from wf_aprovacao_parecer_conclusivo where id = @id";

            await database.Conexao.ExecuteScalarAsync(query, new { id });
        }

        public async Task<WFAprovacaoParecerConclusivo> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId)
        {
            var query = @"select * from wf_aprovacao_parecer_conclusivo where conselho_classe_aluno_id = @conselhoClasseAlunoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<WFAprovacaoParecerConclusivo>(query, new { conselhoClasseAlunoId });
        }

        public async Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId)
        {
            var query = @"select wa.*, ca.*, cpa.*, cpp.*
                          from wf_aprovacao_parecer_conclusivo wa
                         inner join conselho_classe_aluno ca on ca.id = wa.conselho_classe_aluno_id
                          left join conselho_classe_parecer cpa on cpa.id = ca.conselho_classe_parecer_id
                          left join conselho_classe_parecer cpp on cpp.id = wa.conselho_classe_parecer_id
                        where wa.wf_aprovacao_id = @workflowId";

            return (await database.Conexao.QueryAsync<WFAprovacaoParecerConclusivo, ConselhoClasseAluno, ConselhoClasseParecerConclusivo, ConselhoClasseParecerConclusivo, WFAprovacaoParecerConclusivo>(query
                , (wfAprovacao, conselhoClasseAluno, parecerAnterior, parecerNovo) =>
                {
                    conselhoClasseAluno.ConselhoClasseParecer = parecerAnterior;

                    wfAprovacao.ConselhoClasseParecer = parecerNovo;
                    wfAprovacao.ConselhoClasseAluno = conselhoClasseAluno;

                    return wfAprovacao;
                }, new { workflowId })).FirstOrDefault();
        }

        public async Task<WFAprovacaoParecerConclusivoDto> ObterAprovacaoParecerConclusivoPorWorkflowId(long workflowId)
        {
            var query = @"select wa.criado_em as CriadoEm, 
                                   wa.usuario_solicitante_id as UsuarioSolicitanteId,
	                               wa.conselho_classe_parecer_id as ConselhoClasseParecerId, 
	                               t.Id as TurmaId, 
	                               pe.Bimestre, 
	                               cpa.Nome as NomeParecerAnterior, 
	                               cpp.Nome as NomeParecerNovo, 
	                               ca.aluno_codigo as AlunoCodigo,
                                   ca.id as ConselhoClasseAlunoId,
                                   wa.wf_aprovacao_id as WorkFlowAprovacaoId,
                                   t.ano_letivo as AnoLetivo
                             from wf_aprovacao_parecer_conclusivo wa
                                 join conselho_classe_aluno ca on ca.id = wa.conselho_classe_aluno_id
                                 join conselho_classe cc on cc.id = ca.conselho_classe_id
                                 join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                                 join turma t on t.id = ft.turma_id
                                 join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                 left join conselho_classe_parecer cpa on cpa.id = ca.conselho_classe_parecer_id
                                 left join conselho_classe_parecer cpp on cpp.id = wa.conselho_classe_parecer_id
                            where wa.wf_aprovacao_id = @workflowId";

            return await database.Conexao.QueryFirstOrDefaultAsync<WFAprovacaoParecerConclusivoDto>(query, new { workflowId });
        }

        public async Task Salvar(WFAprovacaoParecerConclusivo entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }
    }
}
