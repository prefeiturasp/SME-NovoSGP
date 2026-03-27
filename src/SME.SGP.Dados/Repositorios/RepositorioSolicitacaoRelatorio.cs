using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSolicitacaoRelatorio : RepositorioBase<SolicitacaoRelatorio>, IRepositorioSolicitacaoRelatorio
    {
        public RepositorioSolicitacaoRelatorio(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }


        public async Task<IEnumerable<SolicitacaoRelatorio>> ObterSolicitacaoRelatorioAsync(TipoRelatorio tipoRelatorio, TipoFormatoRelatorio extensaoRelatorio, string usuarioQueSolicitou, int? intervaloSolicitacao = 1)
        {
            var sql = @"
                        SELECT *
                        FROM solicitacao_relatorio
                        WHERE excluido = false
                            AND relatorio = @TipoRelatorio 
                            AND extensao_relatorio = @ExtensaoRelatorio 
                            AND usuario_que_solicitou = @UsuarioQueSolicitou 
                            AND status_solicitacao != @StatusEntregue 
                            AND solicitado_em >= NOW() - (@Horas * INTERVAL '1 hour');";

            return await database.Conexao.QueryAsync<SolicitacaoRelatorio>(sql, new
            {
                TipoRelatorio = (int)tipoRelatorio,
                ExtensaoRelatorio = (int)extensaoRelatorio,
                UsuarioQueSolicitou = usuarioQueSolicitou,
                StatusEntregue = (int)StatusSolicitacao.Entregue,
                Horas = intervaloSolicitacao
            });
        }

        public async Task<bool> RelatorioJaSolicitadoAsync(string filtros, TipoRelatorio tipoRelatorio, string usuarioQueSolicitou)
        {
            var objeto = JsonSerializer.Deserialize<object>(filtros);
            string filtrosFormatado = JsonSerializer.Serialize(objeto);

            var sql = @"
                        SELECT COUNT(1)
                        FROM solicitacao_relatorio
                        WHERE
                            excluido = false AND
                            filtros_usados = @FiltrosUsados::text AND
                            relatorio = @TipoRelatorio AND
                            usuario_que_solicitou = @UsuarioQueSolicitou AND
                            status_solicitacao != @StatusEntregue AND
                            solicitado_em >= NOW() - INTERVAL '1 hour';";

            var count = await database.Conexao.QueryFirstOrDefaultAsync<int>(sql, new
            {
                FiltrosUsados = filtrosFormatado,
                TipoRelatorio = (int)tipoRelatorio,
                UsuarioQueSolicitou = usuarioQueSolicitou,
                StatusEntregue = (int)StatusSolicitacao.Entregue
            });

            return count > 0;
        }
    }
}
