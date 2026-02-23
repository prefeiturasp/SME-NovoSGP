using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSolicitacaoRelatorio : IRepositorioSolicitacaoRelatorio
    {
        protected readonly ISgpContext database;

        public RepositorioSolicitacaoRelatorio(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<SolicitacaoRelatorio>> BuscarPorFiltrosExatosAsync(FiltroRelatorioBase filtros, TipoRelatorio? tipoRelatorio = null, StatusSolicitacao? statusSolicitacao = null)
        {
            var filtrosJson = JsonSerializer.Serialize(filtros);

            var sql = @"
                    SELECT *
                    FROM solicitacao_relatorio
                    WHERE
                        excluido = false AND
                        (@FiltrosUsados IS NULL OR filtros_usados = @FiltrosUsados::text) AND
                        (@TipoRelatorio IS NULL OR tipo_relatorio = @TipoRelatorio) AND
                        (@StatusSolicitacao IS NULL OR status_solicitacao = @StatusSolicitacao);";

            return await database.Conexao.QueryAsync<SolicitacaoRelatorio>(sql, new
            {
                FiltrosUsados = filtrosJson,
                TipoRelatorio = tipoRelatorio.HasValue ? (int?)tipoRelatorio.Value : null,
                StatusSolicitacao = statusSolicitacao.HasValue ? (int?)statusSolicitacao.Value : null
            });
        }

        public async Task<long> InserirAsync(SolicitacaoRelatorio solicitacao)
        {
            var sql = @"
            INSERT INTO solicitacao_relatorio (
                filtros_usados,
                tipo_relatorio,
                usuario_que_solicitou,
                status_solicitacao,
                excluido,
                criado_em,
                criado_por,
                criado_rf,
                solicitado_em
            ) VALUES (
                @FiltrosUsados,
                @TipoRelatorio,
                @UsuarioQueSolicitou,
                @StatusSolicitacao,
                @Excluido,
                @CriadoEm,
                @CriadoPor,
                @CriadoRf,
                @SolicitadoEm
            ) RETURNING id;";


            return await database.Conexao.ExecuteAsync(sql, solicitacao);
        }

        public async Task<bool> RelatorioJaSolicitadoAsync(FiltroRelatorioBase filtros, TipoRelatorio tipoRelatorio, string usuarioQueSolicitou)
        {
            var filtrosJson = JsonSerializer.Serialize(filtros);
            var sql = @"
                        SELECT COUNT(1)
                        FROM solicitacao_relatorio
                        WHERE
                            excluido = false AND
                            filtros_usados = @FiltrosUsados::text AND
                            tipo_relatorio = @TipoRelatorio AND
                            usuario_que_solicitou = @UsuarioQueSolicitou AND
                            status_solicitacao != @StatusFalha AND
                            solicitado_em >= NOW() - INTERVAL '1 hour';";

            var count = await database.Conexao.QueryFirstOrDefaultAsync<int>(sql, new
            {
                FiltrosUsados = filtrosJson,
                TipoRelatorio = (int)tipoRelatorio,
                UsuarioQueSolicitou = usuarioQueSolicitou,
                StatusFalha = (int)StatusSolicitacao.Falha
            });

            return count > 0;
        }
    }
}
