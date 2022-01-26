using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamento : RepositorioBase<PendenciaFechamento>, IRepositorioPendenciaFechamento
    {
        public RepositorioPendenciaFechamento(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> ListarPaginada(Paginacao paginacao, string turmaCodigo, int bimestre, long componenteCurricularId)
        {
            var retorno = new PaginacaoResultadoDto<PendenciaFechamentoResumoDto>();

            var query = new StringBuilder(MontaQuery(paginacao, bimestre, componenteCurricularId, ordenar: true));
            query.AppendLine(MontaQuery(paginacao, bimestre, componenteCurricularId, true));

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new { turmaCodigo, bimestre, componenteCurricularId }))
            {
                retorno.Items = multi.Read<PendenciaFechamentoResumoDto>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        public async Task<IEnumerable<PendenciaFechamento>> ObterPorFechamentoIdDisciplinaId(long fechamentoId, long disciplinaId)
        {
            var query = @"select pf.fechamento_turma_disciplina_id as FechamentoTurmaDisciplinaId ,
                            pf.pendencia_id as PendenciaId
                            from pendencia_fechamento pf 
                            inner join fechamento_turma_disciplina ftd on pf.fechamento_turma_disciplina_id = ftd.id 
                            where disciplina_id = @disciplinaId and
                            fechamento_turma_id = @fechamentoId";

            return await database.Conexao.QueryAsync<PendenciaFechamento>(query, new { fechamentoId, disciplinaId });
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            var query = @"select p.id as PendenciaId, p.titulo as descricao, p.descricao as detalhamento, p.descricao_html as descricaohtml
                                , p.situacao, ftd.disciplina_id as DisciplinaId, pe.bimestre, pf.fechamento_turma_disciplina_id as FechamentoId
                                , p.criado_em as CriadoEm, p.criado_por as CriadoPor, p.criado_rf as CriadoRf, p.alterado_em as AlteradoEm, p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf
                          from pendencia_fechamento pf
                         inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                         inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                         inner join turma t on t.id = ft.turma_id
                         inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                         inner join pendencia p on p.id = pf.pendencia_id
                         where p.id = @pendenciaId";

            return await database.Conexao.QueryFirstAsync<PendenciaFechamentoCompletoDto>(query, new { pendenciaId });
        }

        public async Task<Turma> ObterTurmaPorPendenciaId(long pendenciaId)
        {
            var query = @"select t.* 
                          from pendencia_fechamento fp
                         inner join fechamento_turma_disciplina ftd on ftd.id = fp.fechamento_turma_disciplina_id
                         inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                         inner join turma t on t.id = ft.turma_id
                        where fp.pendencia_id = @pendenciaId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { pendenciaId });
        }

        public async Task RemoverAsync(PendenciaFechamento pendencia)
        {
            await database.Conexao.DeleteAsync(pendencia);
            Auditar(pendencia.Id, "E");
        }

        private void Auditar(long identificador, string acao)
        {
            database.Conexao.Insert<Auditoria>(new Auditoria()
            {
                Data = DateTime.Now,
                Entidade = "pendenciafechamento",
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Acao = acao
            });
        }

        public bool VerificaPendenciasAbertoPorFechamento(long fechamentoId)
        {
            var query = @"select count(p.id)
                      from pendencia_fechamento pf
                     inner join pendencia p on p.id = pf.pendencia_id
                     where not p.excluido
                       and pf.fechamento_turma_disciplina_id = @fechamentoId
                       and p.situacao = 1";

            return database.Conexao.QueryFirst<int>(query, new { fechamentoId }) > 0;
        }

        public async Task<bool> PossuiPendenciasAbertoPorTurmaDisciplina(string turmaId, int bimestre, long disciplinaId)
        {
            bool teste = false;
            var query = @"select 1 
                                 from fechamento_turma_disciplina ftd
                                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                                 inner join turma t on t.id = ft.turma_id
                                 inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                                 inner join periodo_fechamento_bimestre pfb on pfb.periodo_escolar_id = pe.id 
                                    and t.turma_id = @turmaId
                                    and pe.bimestre = @bimestre
                                    and current_date between pfb.inicio_fechamento and pfb.final_fechamento 
                                    and ftd.disciplina_id = @disciplinaId";
            try
            {
                teste = await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { turmaId, bimestre, disciplinaId });
            }
            catch (Exception ex)
            {

            }

            return teste;
        }

        private string MontaQuery(Paginacao paginacao, int bimestre, long componenteCurricularId, bool contador = false, bool ordenar = false)
        {
            var fields = contador ? "count(p.id)" : "p.id as PendenciaId, p.titulo as descricao, p.situacao, ftd.disciplina_id as DisciplinaId";
            var query = new StringBuilder(string.Format(@"select {0}
                                  from pendencia_fechamento pf
                                 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                                 inner join turma t on t.id = ft.turma_id
                                 inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                                 inner join pendencia p on p.id = pf.pendencia_id
                                  where not p.excluido
                                    and t.turma_id = @turmaCodigo ", fields));
            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");
            if (componenteCurricularId > 0)
                query.AppendLine(" and ftd.disciplina_id = @componenteCurricularId");

            if (ordenar)
                query.AppendLine(" order by p.situacao");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY;");
            
            return query.ToString();
        }

        public async Task<bool> ExistePendenciaFechamentoPorPendenciaId(long pendenciaId)
        {
            var query = "select 1 from pendencia_fechamento where pendencia_id = @pendenciaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { pendenciaId });
        }

        public async Task<IEnumerable<PendenciaParaFechamentoConsolidadoDto>> ObterPendenciasParaFechamentoConsolidado(long turmaId, int bimestre, long componenteCurricularId)
        {
            var query = @"select p.id as PendenciaId, 
                                 p.titulo as descricao, 
                                 P.tipo as tipoPendencia  
                            from pendencia_fechamento pf
                           inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                           inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                           inner join turma t on t.id = ft.turma_id
                           inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                           inner join pendencia p on p.id = pf.pendencia_id
                           where not p.excluido
                             and P.situacao = 1
                             and t.id = @turmaId
                             and pe.bimestre = @bimestre
                             and ftd.disciplina_id = @componenteCurricularId
                           order by p.criado_em";

            return await database.Conexao.QueryAsync<PendenciaParaFechamentoConsolidadoDto>(query, new { turmaId, bimestre, componenteCurricularId });
        }

        public async Task<DetalhamentoPendenciaFechamentoConsolidadoDto> ObterDetalhamentoPendenciaFechamentoConsolidado(long pendenciaId)
        {
            var query = @"select p.id as PendenciaId, 
                                 p.descricao as descricao, 
                                 p.descricao_html as descricaohtml,
                                 ftd.justificativa 
                            from pendencia_fechamento pf
                           inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                           inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                           inner join turma t on t.id = ft.turma_id
                           inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                           inner join pendencia p on p.id = pf.pendencia_id
                           where p.id = @pendenciaId      
                             and p.situacao = 1
                             and not ftd.excluido ";

            return await database.Conexao.QueryFirstOrDefaultAsync<DetalhamentoPendenciaFechamentoConsolidadoDto>(query, new { pendenciaId });
        }

        public async Task<DetalhamentoPendenciaAulaDto> ObterDetalhamentoPendenciaAula(long pendenciaId)
        {
            var query = @"select P.id as pendenciaId,
                                 p.tipo as tipoPendencia,
                                 p.descricao_html as descricaohtml                               
                            from pendencia_fechamento pf
                           inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                           inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                           inner join turma t on t.id = ft.turma_id
                           inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                           inner join pendencia p on p.id = pf.pendencia_id
                           where p.id = @pendenciaId                             
                             and p.situacao = 1
                             and not ftd.excluido ";

            return await database.Conexao.QueryFirstOrDefaultAsync<DetalhamentoPendenciaAulaDto>(query, new { pendenciaId });
        }
    }
}