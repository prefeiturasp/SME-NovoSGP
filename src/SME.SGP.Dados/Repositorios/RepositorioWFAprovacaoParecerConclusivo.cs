using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoParecerConclusivo : RepositorioBase<WFAprovacaoParecerConclusivo>, IRepositorioWFAprovacaoParecerConclusivo
    {

        public RepositorioWFAprovacaoParecerConclusivo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}

        public async Task Excluir(long id)
        {
            var query = @"delete from wf_aprovacao_parecer_conclusivo where id = @id";

            await database.Conexao.ExecuteScalarAsync(query, new { id });
        }

        public async Task ExcluirLogico(long id)
        {
            var query = $@"update wf_aprovacao_parecer_conclusivo
                            set excluido = true 
                           where id=@id";

            await database.Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<WFAprovacaoParecerConclusivo>> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId)
        {
            var query = @"select * from wf_aprovacao_parecer_conclusivo where not excluido and conselho_classe_aluno_id = @conselhoClasseAlunoId";

            return await database.Conexao.QueryAsync<WFAprovacaoParecerConclusivo>(query, new { conselhoClasseAlunoId });
        }

        public async Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId)
        {
            var query = @"select wa.*, ca.*, cpa.*, cpp.*
                          from wf_aprovacao_parecer_conclusivo wa
                         inner join conselho_classe_aluno ca on ca.id = wa.conselho_classe_aluno_id
                          left join conselho_classe_parecer cpa on cpa.id = ca.conselho_classe_parecer_id
                          left join conselho_classe_parecer cpp on cpp.id = wa.conselho_classe_parecer_id
                        where not wa.excluido and wa.wf_aprovacao_id = @workflowId";

            return (await database.Conexao.QueryAsync<WFAprovacaoParecerConclusivo, ConselhoClasseAluno, ConselhoClasseParecerConclusivo, ConselhoClasseParecerConclusivo, WFAprovacaoParecerConclusivo>(query
                , (wfAprovacao, conselhoClasseAluno, parecerAnterior, parecerNovo) =>
                {
                    conselhoClasseAluno.ConselhoClasseParecer = parecerAnterior;

                    wfAprovacao.ConselhoClasseParecer = parecerNovo;
                    wfAprovacao.ConselhoClasseAluno = conselhoClasseAluno;

                    return wfAprovacao;
                }, new { workflowId })).FirstOrDefault();
        }

        public async Task<IEnumerable<WFAprovacaoParecerConclusivoDto>> ObterAprovacaoPareceresConclusivosPorWorkflowId(long workflowId)
        {
            var query = ObterQueryPareceresWorkflow();
            query += " where wa.wf_aprovacao_id = @workflowId";

            return await database.Conexao.QueryAsync<WFAprovacaoParecerConclusivoDto>(query, new { workflowId });
        }

        public async Task<IEnumerable<WFAprovacaoParecerConclusivoDto>> ObterPareceresAguardandoAprovacaoSemWorkflow()
        {
            var query = ObterQueryPareceresWorkflow();
            query += " where not wa.excluido and wa.wf_aprovacao_id is null";
            return await database.Conexao
                .QueryAsync<WFAprovacaoParecerConclusivoDto>(query);
        }

        private string ObterQueryPareceresWorkflow()
        {
            return @"select wa.id, wa.criado_em as CriadoEm, 
                    wa.usuario_solicitante_id as UsuarioSolicitanteId,
	                wa.conselho_classe_parecer_id as ConselhoClasseParecerId, 
	                t.Id as TurmaId, 
	                pe.Bimestre, 
	                cpa.Nome as NomeParecerAnterior, 
	                cpp.Nome as NomeParecerNovo, 
	                ca.aluno_codigo as AlunoCodigo,
                    ca.id as ConselhoClasseAlunoId,
                    wa.conselho_classe_parecer_id_anterior as ConselhoClasseParecerAnteriorId,
                    wa.wf_aprovacao_id as WorkFlowAprovacaoId,
                    t.ano_letivo as AnoLetivo
                from wf_aprovacao_parecer_conclusivo wa
                    join conselho_classe_aluno ca on ca.id = wa.conselho_classe_aluno_id
                    join conselho_classe cc on cc.id = ca.conselho_classe_id
                    join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                    join turma t on t.id = ft.turma_id
                    left join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                    left join conselho_classe_parecer cpa on cpa.id = wa.conselho_classe_parecer_id_anterior
                    left join conselho_classe_parecer cpp on cpp.id = wa.conselho_classe_parecer_id";
        }
    }
}
