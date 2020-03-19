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
    public class RepositorioFechamentoReabertura : RepositorioBase<FechamentoReabertura>, IRepositorioFechamentoReabertura
    {
        public RepositorioFechamentoReabertura(ISgpContext database) : base(database)
        {
        }

        public void ExcluirBimestres(long id)
        {
            database.Conexao.Execute("DELETE FROM FECHAMENTO_REABERTURA_BIMESTRE FRB WHERE FRB.FECHAMENTO_REABERTURA_ID = @id", new { id });
        }

        public async Task ExcluirVinculoDeNotificacoesAsync(long fechamentoReaberturaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM FECHAMENTO_REABERTURA_NOTIFICACAO WHERE FECHAMENTO_REABERTURA_ID = @fechamentoReaberturaId", new { fechamentoReaberturaId });
        }

        public async Task<IEnumerable<FechamentoReabertura>> Listar(long tipoCalendarioId, long? dreId, long? ueId, long[] ids = null)
        {
            var query = new StringBuilder();
            MontaQueryCabecalhoCompleto(query);
            MontaQueryFromCompleto(query);
            MontaQueryListarWhere(query, tipoCalendarioId, dreId, ueId, ids: ids);

            var lookup = new Dictionary<long, FechamentoReabertura>();

            await database.Conexao.QueryAsync<FechamentoReabertura, FechamentoReaberturaBimestre, Ue, Dre, TipoCalendario, FechamentoReabertura>(query.ToString(), (fechamento, bimestre, ue, dre, tipoCalendario) =>
            {
                FechamentoReabertura fechamentoReabertura;
                if (!lookup.TryGetValue(fechamento.Id, out fechamentoReabertura))
                {
                    fechamentoReabertura = fechamento;
                    lookup.Add(fechamento.Id, fechamentoReabertura);
                }
                fechamentoReabertura.AtualizarDre(dre);
                fechamentoReabertura.AtualizarUe(ue);
                fechamentoReabertura.AtualizarTipoCalendario(tipoCalendario);
                fechamentoReabertura.Adicionar(bimestre);
                return fechamentoReabertura;
            }, new
            {
                tipoCalendarioId,
                dreId,
                ueId,
                ids
            });

            return lookup.Values;
        }

        public async Task<PaginacaoResultadoDto<FechamentoReabertura>> ListarPaginado(long tipoCalendarioId, string dreCodigo, string ueCodigo, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();

            if (paginacao == null || paginacao.QuantidadeRegistros == 0)
                paginacao = new Paginacao(1, 10);

            MontaQueryCabecalhoCompleto(query);
            MontaQueryFromCompleto(query);
            MontaQueryListarWhere(query, tipoCalendarioId, 0, 0, dreCodigo, ueCodigo);

            var retornoPaginado = new PaginacaoResultadoDto<FechamentoReabertura>();

            var lookup = new Dictionary<long, FechamentoReabertura>();

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);

            await database.Conexao.QueryAsync<FechamentoReabertura, FechamentoReaberturaBimestre, Ue, Dre, TipoCalendario, FechamentoReabertura>(query.ToString(), (fechamento, bimestre, ue, dre, tipoCalendario) =>
            {
                FechamentoReabertura fechamentoReabertura;
                if (!lookup.TryGetValue(fechamento.Id, out fechamentoReabertura))
                {
                    fechamentoReabertura = fechamento;
                    lookup.Add(fechamento.Id, fechamentoReabertura);
                }
                fechamentoReabertura.AtualizarDre(dre);
                fechamentoReabertura.AtualizarUe(ue);
                fechamentoReabertura.AtualizarTipoCalendario(tipoCalendario);
                fechamentoReabertura.Adicionar(bimestre);
                return fechamentoReabertura;
            }, new
            {
                tipoCalendarioId,
                dreCodigo,
                ueCodigo
            });

            retornoPaginado.Items = lookup.Values;

            query = new StringBuilder();
            MontaQueryListarCount(query, tipoCalendarioId, dreCodigo, ueCodigo);

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new
            {
                tipoCalendarioId,
                dreCodigo,
                ueCodigo
            }));

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        public FechamentoReabertura ObterCompleto(long idFechamentoReabertura = 0, long workflowId = 0)
        {
            var query = new StringBuilder();
            MontaQueryCabecalhoCompleto(query);
            MontaQueryFromCompleto(query);

            query.AppendLine("where  1=1");

            if (idFechamentoReabertura != 0)
                query.AppendLine("and fr.id = @idFechamentoReabertura");

            if (workflowId > 0)
                query.AppendLine("and fr.wf_aprovacao_id = @workflowId");

            var lookup = new Dictionary<long, FechamentoReabertura>();

            database.Conexao.Query<FechamentoReabertura, FechamentoReaberturaBimestre, Ue, Dre, TipoCalendario, FechamentoReabertura>(query.ToString(), (fechamento, bimestre, ue, dre, tipoCalendario) =>
           {
               FechamentoReabertura fechamentoReabertura;
               if (!lookup.TryGetValue(fechamento.Id, out fechamentoReabertura))
               {
                   fechamentoReabertura = fechamento;
                   lookup.Add(fechamento.Id, fechamentoReabertura);
               }
               fechamentoReabertura.AtualizarDre(dre);
               fechamentoReabertura.AtualizarUe(ue);
               fechamentoReabertura.AtualizarTipoCalendario(tipoCalendario);
               fechamentoReabertura.Adicionar(bimestre);
               return fechamentoReabertura;
           }, new
           {
               idFechamentoReabertura,
               workflowId
           });

            return lookup.Values.FirstOrDefault();
        }

        public async Task<IEnumerable<FechamentoReaberturaNotificacao>> ObterNotificacoes(long id)
        {
            return await database.Conexao.QueryAsync<FechamentoReaberturaNotificacao>("SELECT * FROM FECHAMENTO_REABERTURA_NOTIFICACAO FRN WHERE FRN.FECHAMENTO_REABERTURA_ID = @Id", new { id });
        }

        public async Task<IEnumerable<FechamentoReabertura>> ObterReaberturaFechamentoBimestre(int bimestre, DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            var query = @"select fr.* 
                          from fechamento_reabertura_bimestre frb
                         inner join fechamento_reabertura fr on fr.id = frb.fechamento_reabertura_id
                         inner join dre on dre.id = fr.dre_id
                         inner join ue on ue.id = fr.ue_id
                         where not fr.excluido
                           and frb.bimestre = @bimestre
                           and TO_DATE(fr.inicio, 'yyyy/mm/dd') = TO_DATE(@dataInicio, 'yyyy/mm/dd')
                           and TO_DATE(fr.fim, 'yyyy/mm/dd') = TO_DATE(@dataFim, 'yyyy/mm/dd')
                           and fr.tipo_calendario_id = @tipoCalendarioId
                           and dre.dre_id = @dreCodigo
                           and ue.ue_id = @ueCodigo";

            return await database.Conexao.QueryAsync<FechamentoReabertura>(query, new
            {
                bimestre,
                dataInicio,
                dataFim,
                tipoCalendarioId,
                dreCodigo,
                ueCodigo
            });
        }

        public async Task SalvarBimestreAsync(FechamentoReaberturaBimestre fechamentoReabertura)
        {
            await database.Conexao.InsertAsync(fechamentoReabertura);
        }

        public async Task SalvarNotificacaoAsync(FechamentoReaberturaNotificacao fechamentoReaberturaNotificacao)
        {
            await database.Conexao.InsertAsync(fechamentoReaberturaNotificacao);
        }

        private void MontaQueryCabecalhoCompleto(StringBuilder query)
        {
            query.AppendLine("select fr.*, frb.*, ue.*, dre.*, tc.*");
        }

        private void MontaQueryFromCompleto(StringBuilder query)
        {
            query.AppendLine("from fechamento_reabertura fr");
            query.AppendLine("join fechamento_reabertura_bimestre frb");
            query.AppendLine("on frb.fechamento_reabertura_id = fr.id");
            query.AppendLine("inner join tipo_calendario tc");
            query.AppendLine("on fr.tipo_calendario_id = tc.id");
            query.AppendLine("left join ue");
            query.AppendLine("on fr.ue_id = ue.id");
            query.AppendLine("left join dre");
            query.AppendLine("on fr.dre_id = dre.id");
        }

        private void MontaQueryListarCount(StringBuilder query, long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            query.AppendLine("select count(fr.*)");
            query.AppendLine("from fechamento_reabertura fr");
            query.AppendLine("inner join tipo_calendario tc");
            query.AppendLine("on fr.tipo_calendario_id = tc.id");
            query.AppendLine("left join ue");
            query.AppendLine("on fr.ue_id = ue.id");
            query.AppendLine("left join dre");
            query.AppendLine("on fr.dre_id = dre.id");
            MontaQueryListarWhere(query, tipoCalendarioId, 0, 0, dreCodigo, ueCodigo);
        }

        private void MontaQueryListarWhere(StringBuilder query, long tipoCalendarioId, long? dreId, long? ueId, string dreCodigo = "", string ueCodigo = "", long[] ids = null)
        {
            query.AppendLine("where fr.excluido = false and fr.status <> 3");

            if (tipoCalendarioId > 0)
                query.AppendLine("and fr.tipo_calendario_id = @tipoCalendarioId");

            if (dreId.HasValue && dreId.Value > 0)
                query.AppendLine("and fr.dre_id = @dreId");

            if (ueId.HasValue && ueId.Value > 0)
                query.AppendLine("and fr.ue_id = @ueId");

            if (!string.IsNullOrEmpty(dreCodigo))
                query.AppendLine("and dre.dre_id = @dreCodigo");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and ue.ue_id = @ueCodigo");

            if (ids != null && ids.Any())
                query.AppendLine("and fr.id = ANY(@ids)");
        }
    }
}