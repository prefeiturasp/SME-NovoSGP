using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
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
    }
}
