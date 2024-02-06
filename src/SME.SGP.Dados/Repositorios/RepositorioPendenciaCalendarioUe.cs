using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaCalendarioUe : RepositorioBase<PendenciaCalendarioUe>, IRepositorioPendenciaCalendarioUe
    {
        public RepositorioPendenciaCalendarioUe(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<PendenciaCalendarioUe>> ObterPendenciasPorCalendarioUe(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia)
        {
            var query = @"select pc.* 
                        from pendencia_calendario_ue pc
                       inner join pendencia p on p.id = pc.pendencia_id
                        where not p.excluido 
                          and pc.tipo_calendario_id = @tipoCalendarioId 
                          and pc.ue_id = @ueId
                          and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryAsync<PendenciaCalendarioUe>(query, new { tipoCalendarioId, ueId, tipoPendencia });
        }

        public async Task<IEnumerable<long>> ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomatico(long idUe, int anoLetivo)
        {
            var tipoPendencia = new List<int>() { (int)TipoPendencia.CalendarioLetivoInsuficiente, (int)TipoPendencia.AulaNaoLetivo, (int)TipoPendencia.CadastroEventoPendente };
            var situacao = new List<int>() { (int)SituacaoPendencia.Pendente, (int)SituacaoPendencia.Resolvida };

            var query = @"SELECT 
                                distinct p.id AS PendenciaId
                            FROM pendencia p
                                JOIN pendencia_aula pa ON p.id = pa.pendencia_id
                                inner JOIN aula a ON a.id = pa.aula_id
                                inner JOIN turma t ON t.turma_id = a.turma_id
                            WHERE NOT p.excluido
                              AND p.tipo = any(@tipoPendencia)
                              AND p.situacao = any(@situacao)
                              AND t.ano_letivo = @anoLetivo
                              AND t.ue_id = @idUe ";
                                        
            return await database.Conexao.QueryAsync<long>(query, new
            {
                tipoPendencia = tipoPendencia.ToArray(),
                situacao = situacao.ToArray(),
                anoLetivo,
                idUe
            });
        }
    }
}
