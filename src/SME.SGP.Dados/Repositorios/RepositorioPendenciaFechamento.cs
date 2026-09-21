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
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamento : RepositorioBase<PendenciaFechamento>, IRepositorioPendenciaFechamento
    {
        public RepositorioPendenciaFechamento(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> ListarPaginada(Paginacao paginacao, string turmaCodigo, int bimestre, long componenteCurricularId)
        {
            var retorno = new PaginacaoResultadoDto<PendenciaFechamentoResumoDto>();

            var query = MontaQueryPaginadaComContagem(paginacao, bimestre, componenteCurricularId);

            var parametros = new
            {
                turmaCodigo,
                bimestre,
                componenteCurricularId,
                quantidadeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                quantidadeRegistros = paginacao.QuantidadeRegistros
            };

            var itensComTotal = (await database.Conexao.QueryAsync<PendenciaFechamentoResumoComTotalDto>(query, parametros)).ToList();

            retorno.TotalRegistros = itensComTotal.FirstOrDefault()?.TotalRegistros ?? 0;
            retorno.Items = itensComTotal.Select(i => new PendenciaFechamentoResumoDto
            {
                PendenciaId = i.PendenciaId,
                DisciplinaId = i.DisciplinaId,
                Descricao = i.Descricao,
                Situacao = i.Situacao
            }).ToList();

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
                : (retorno.TotalRegistros > 0 ? 1 : 0);

            return retorno;
        }

        public async Task<IEnumerable<PendenciaFechamento>> ObterPorFechamentoIdDisciplinaId(long fechamentoId, long disciplinaId)
        {
            const string query = @"select pf.fechamento_turma_disciplina_id as FechamentoTurmaDisciplinaId ,
                             pf.pendencia_id as PendenciaId
                             from pendencia_fechamento pf 
                             inner join fechamento_turma_disciplina ftd on pf.fechamento_turma_disciplina_id = ftd.id 
                             where disciplina_id = @disciplinaId and
                             fechamento_turma_id = @fechamentoId";

            return await database.Conexao.QueryAsync<PendenciaFechamento>(query, new { fechamentoId, disciplinaId });
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            const string query = @"select p.id as PendenciaId, p.titulo as descricao, p.descricao as detalhamento, p.descricao_html as descricaohtml
                                , p.situacao, ftd.disciplina_id as DisciplinaId, pe.bimestre, pf.fechamento_turma_disciplina_id as FechamentoId
                                , p.criado_em as CriadoEm, p.criado_por as CriadoPor, p.criado_rf as CriadoRf, p.alterado_em as AlteradoEm, p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf,
                                  ft.turma_id as turmaId, 
                                  t.turma_id as CodigoTurma
                          from pendencia_fechamento pf
                         inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                         inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                         inner join turma t on t.id = ft.turma_id
                         inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                         inner join pendencia p on p.id = pf.pendencia_id
                         where p.id = @pendenciaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PendenciaFechamentoCompletoDto>(query, new { pendenciaId });
        }

        public async Task<Turma> ObterTurmaPorPendenciaId(long pendenciaId)
        {
            const string query = @"select t.* 
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
            const string query = @"select count(p.id)
                      from pendencia_fechamento pf
                     inner join pendencia p on p.id = pf.pendencia_id
                     where not p.excluido
                       and pf.fechamento_turma_disciplina_id = @fechamentoId
                       and p.situacao = 1";

            return database.Conexao.QueryFirst<int>(query, new { fechamentoId }) > 0;
        }

        public async Task<bool> PossuiFechamentoPorTurmaComponenteBimestre(long turmaId, int bimestre, long componenteCurricularId)
        {
            var condicaoBimestre = bimestre > 0 ? "pe.bimestre = @bimestre" : "pe.id is null";

            var query = $@"select 1 
                                 from fechamento_turma_disciplina ftd
                                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                                where not ftd.excluido
                                    and not ft.excluido
                                    and ft.turma_id = @turmaId
                                    and {condicaoBimestre}
                                    and ftd.disciplina_id = @componenteCurricularId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { turmaId, bimestre, componenteCurricularId });
        }

        private static string MontaQueryPaginadaComContagem(Paginacao paginacao, int bimestre, long componenteCurricularId)
        {
            var query = new StringBuilder(@"select p.id as PendenciaId, p.titulo as descricao, p.situacao, ftd.disciplina_id as DisciplinaId,
                       count(*) over() as TotalRegistros
                  from pendencia_fechamento pf
                 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
                 inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                 inner join turma t on t.id = ft.turma_id
                 inner join pendencia p on p.id = pf.pendencia_id ");

            if (bimestre > 0)
                query.AppendLine(" inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id");

            query.AppendLine(" where not p.excluido and not ftd.excluido and not ft.excluido");
            query.AppendLine(" and t.turma_id = @turmaCodigo");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");

            if (componenteCurricularId > 0)
                query.AppendLine(" and ftd.disciplina_id = @componenteCurricularId");

            query.AppendLine(" order by p.situacao, p.id");

            if (paginacao.QuantidadeRegistros > 0)
                query.AppendLine(" OFFSET @quantidadeRegistrosIgnorados ROWS FETCH NEXT @quantidadeRegistros ROWS ONLY;");
            else
                query.AppendLine(";");

            return query.ToString();
        }
        public async Task<bool> ExistePendenciaFechamentoPorPendenciaId(long pendenciaId)
        {
            const string query = "select 1 from pendencia_fechamento where pendencia_id = @pendenciaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { pendenciaId });
        }

        public async Task<IEnumerable<PendenciaParaFechamentoConsolidadoDto>> ObterPendenciasParaFechamentoConsolidado(long turmaId, int bimestre, long componenteCurricularId)
        {
            const string query = @"select p.id as PendenciaId, 
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
            const string query = @"select p.id as PendenciaId, 
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
            const string query = @"select P.id as pendenciaId,
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

        public async Task<IEnumerable<long>> ObterIdPendenciaFechamentoAprovadaResolvida(long fechamentoId, TipoPendencia tipoPendencia)
        {
            const int situacao = (int)SituacaoPendencia.Pendente;
            const string query = @"select pf.id          
                           from pendencia p
                           inner join pendencia_fechamento pf on pf.pendencia_id = p.id
                           where p.situacao <> @situacao 
                            and p.tipo = @tipoPendencia
                            and pf.fechamento_turma_disciplina_id = @fechamentoId";

            return await database.Conexao.QueryAsync<long>(query, new { situacao, fechamentoId, tipoPendencia = (int)tipoPendencia });
        }
    }
}
