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
        protected readonly ISgpContext database;

        public RepositorioFechamentoReabertura(ISgpContext database) : base(database)
        {
            this.database = database;
        }

        public void ExcluirBimestres(long id)
        {
            database.Conexao.Execute("DELETE FROM FECHAMENTO_REABERTURA_BIMESTRE FRB WHERE FRB.FECHAMENTO_REABERTURA_ID = @id", new { id });
        }

        public async Task<IEnumerable<FechamentoReabertura>> Listar(long tipoCalendarioId, long? dreId, long? ueId)
        {
            var query = new StringBuilder();
            MontaQueryListarCabecalho(query);
            MontaQueryListarFrom(query);
            MontaQueryListarWhere(query, tipoCalendarioId, dreId, ueId);

            var lookup = new Dictionary<long, FechamentoReabertura>();

            await database.Conexao.QueryAsync<FechamentoReabertura, FechamentoReaberturaBimestre, FechamentoReabertura>(query.ToString(), (fechamento, bimestre) =>
            {
                FechamentoReabertura fechamentoReabertura;
                if (!lookup.TryGetValue(fechamento.Id, out fechamentoReabertura))
                {
                    fechamentoReabertura = fechamento;
                    lookup.Add(fechamento.Id, fechamentoReabertura);
                }

                fechamentoReabertura.Adicionar(bimestre);
                return fechamentoReabertura;
            }, new
            {
                tipoCalendarioId,
                dreId,
                ueId
            });

            return lookup.Values;
        }

        public async Task<PaginacaoResultadoDto<FechamentoReabertura>> ListarPaginado(long tipoCalendarioId, long? dreId, long? ueId, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();

            if (paginacao == null || paginacao.QuantidadeRegistros == 0)
                paginacao = new Paginacao(1, 10);

            MontaQueryListarCabecalho(query);
            MontaQueryListarFrom(query);
            MontaQueryListarWhere(query, tipoCalendarioId, dreId, ueId);

            var retornoPaginado = new PaginacaoResultadoDto<FechamentoReabertura>();

            var lookup = new Dictionary<long, FechamentoReabertura>();

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);

            await database.Conexao.QueryAsync<FechamentoReabertura, FechamentoReaberturaBimestre, FechamentoReabertura>(query.ToString(), (fechamento, bimestre) =>
           {
               FechamentoReabertura fechamentoReabertura;
               if (!lookup.TryGetValue(fechamento.Id, out fechamentoReabertura))
               {
                   fechamentoReabertura = fechamento;
                   lookup.Add(fechamento.Id, fechamentoReabertura);
               }

               fechamentoReabertura.Adicionar(bimestre);
               return fechamentoReabertura;
           }, new
           {
               tipoCalendarioId,
               dreId,
               ueId
           });

            retornoPaginado.Items = lookup.Values;

            query = new StringBuilder();
            MontaQueryListarCount(query, tipoCalendarioId, dreId, ueId);

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new
            {
                tipoCalendarioId,
                dreId,
                ueId
            }));

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        public FechamentoReabertura ObterCompleto(long idFechamentoReabertura, long workflowId)
        {
            var query = new StringBuilder(@"select fr.*, frb.*, ue.*, dre.*, tc.*
                            from fechamento_reabertura fr
                            join fechamento_reabertura_bimestre frb
                            on frb.fechamento_reabertura_id = fr.id
                            inner join tipo_calendario tc
                            on fr.tipo_calendario_id = tc.id
                            left join ue
                            on fr.ue_id = ue.id
                            left join dre
                            on fr.dre_id = dre.id
                            where fr.excluido = false");

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

        public FechamentoReabertura ObterPorIdCompleto(long id)
        {
            throw new NotImplementedException();
        }

        public async Task SalvarBimestre(FechamentoReaberturaBimestre fechamentoReabertura)
        {
            await database.Conexao.InsertAsync(fechamentoReabertura);
        }

        private void MontaQueryListarCabecalho(StringBuilder query)
        {
            query.AppendLine("select fr.*, frb.*");
        }

        private void MontaQueryListarCount(StringBuilder query, long tipoCalendarioId, long? dreId, long? ueId)
        {
            query.AppendLine("select count(fr.*)");
            query.AppendLine("from fechamento_reabertura fr");
            MontaQueryListarWhere(query, tipoCalendarioId, dreId, ueId);
        }

        private void MontaQueryListarFrom(StringBuilder query)
        {
            query.AppendLine("from fechamento_reabertura fr");
            query.AppendLine("inner");
            query.AppendLine("join fechamento_reabertura_bimestre frb");
            query.AppendLine("on frb.fechamento_reabertura_id = fr.id");
        }

        private void MontaQueryListarWhere(StringBuilder query, long tipoCalendarioId, long? dreId, long? ueId)
        {
            query.AppendLine("where excluido = false and status <> 3");

            if (tipoCalendarioId > 0)
                query.AppendLine("and fr.tipo_calendario_id = @tipoCalendarioId");

            if (dreId.HasValue && dreId.Value > 0)
                query.AppendLine("and fr.dre_id = @dreId");

            if (ueId.HasValue && ueId.Value > 0)
                query.AppendLine("and fr.ue_id = @ueId");
        }
    }
}