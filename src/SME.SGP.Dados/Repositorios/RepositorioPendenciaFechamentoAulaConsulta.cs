using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamentoAulaConsulta : IRepositorioPendenciaFechamentoAulaConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPendenciaFechamentoAulaConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<long>> ObterIdsAulaDaPendenciaDeFechamento(IEnumerable<long> idsPendenciaFechamento)
        {
            var query = @"select aula_id
                            from pendencia_fechamento_aula 
                            where pendencia_fechamento_id = any(@idsPendenciaFechamento)";

            return await database.Conexao.QueryAsync<long>(query, new { idsPendenciaFechamento = idsPendenciaFechamento.ToArray() });
        }

        public async Task<IEnumerable<PendenciaFechamento>> ObterPendenciaFechamentoDeAula(long idAula, TipoPendencia tipoPendencia)
        {
            var situacao = (int)SituacaoPendencia.Pendente;
            var query = @"select p.*, pf.*
                            from pendencia p
                            inner join pendencia_fechamento pf on p.id = pf.pendencia_id
                            inner join pendencia_fechamento_aula pfa on pfa.pendencia_fechamento_id = pf.id
                            where not p.excluido 
                              and p.situacao = @situacao
                              and p.tipo = @tipoPendencia
                              and pfa.aula_id = @idAula";

            return await database.Conexao.QueryAsync<PendenciaFechamento, Pendencia, PendenciaFechamento>(query, (pendenciaFechamento, pendencia) =>
            {
                pendenciaFechamento.Pendencia = pendencia;
                return pendenciaFechamento;
            }, new
            {
                situacao,
                tipoPendencia = (int)tipoPendencia,
                idAula
            });
        }
    }
}
