﻿using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWfAprovacaoNotaFechamento: IRepositorioWfAprovacaoNotaFechamento
    {
        protected readonly ISgpContext database;

        public RepositorioWfAprovacaoNotaFechamento(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(WfAprovacaoNotaFechamento wfAprovacaoNota)
        {
            await database.Conexao.DeleteAsync(wfAprovacaoNota);
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterPorNotaId(long fechamentoNotaId)
        {
            var query = @"select * from wf_aprovacao_nota_fechamento where fechamento_nota_id = @fechamentoNotaId ";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamento>(query, new { fechamentoNotaId });
        }

        public async Task SalvarAsync(WfAprovacaoNotaFechamento entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterWfAprovacaoNotaFechamentoSemWfAprovacaoId()
        {
            var query = @"select ft.turma_id as TurmaId, fa.fechamento_turma_disciplina_id as FechamentoTurmaDisciplinaId, pe.bimestre as Bimestre, 
                                fa.aluno_codigo as CodigoAluno, fn.nota as NotaAnterior, coalesce(cc.descricao_infantil, cc.descricao_sgp, cc.descricao) as ComponenteCurricularDescricao, 
                                cc.eh_regencia as ComponenteCurricularEhRegencia,fn.conceito_id as ConceitoAnterior, wanf.* from wf_aprovacao_nota_fechamento wanf 
                            inner join fechamento_nota fn on fn.id = wanf.fechamento_nota_id 
                            inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
                            inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                            inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                            inner join componente_curricular cc on cc.id = fn.disciplina_id 
                            inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                            where wf_aprovacao_id is null";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamentoTurmaDto, WfAprovacaoNotaFechamento, WfAprovacaoNotaFechamentoTurmaDto>(query, (wfAprovacaoDto, wfAprovacaoNotaFechamento) =>
            {
                wfAprovacaoDto.WfAprovacao = wfAprovacaoNotaFechamento;

                return wfAprovacaoDto;
            }, splitOn: "TurmaId, FechamentoTurmaDisciplinaId,Bimestre,CodigoAluno, NotaAnterior, ComponenteCurricularDescricao, ComponenteCurricularEhRegencia, ConceitoAnterior, id"
            );
        }

        public Task<bool> AlterarWfAprovacaoNotaFechamentoComWfAprovacaoId(long workflowAprovacaoId, long[] workflowAprovacaoNotaFechamentoIds)
        {
            var query = @"update wf_aprovacao_nota_fechamento 
                                    set wf_aprovacao_id = @workflowAprovacaoId 
                                    where id = ANY(@workflowAprovacaoNotaFechamentoIds)";

            database.Conexao.Execute(query, new { workflowAprovacaoId, workflowAprovacaoNotaFechamentoIds });
            return Task.FromResult(true);
        }
    }
}
