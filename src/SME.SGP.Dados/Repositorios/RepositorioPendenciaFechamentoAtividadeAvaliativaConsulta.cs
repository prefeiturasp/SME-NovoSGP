using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamentoAtividadeAvaliativaConsulta : IRepositorioPendenciaFechamentoAtividadeAvaliativaConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPendenciaFechamentoAtividadeAvaliativaConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<long>> ObterIdsAtividadeAvaliativaDaPendenciaDeFechamento(IEnumerable<long> idsPendenciaFechamento)
        {
            var query = @"select atividade_avaliativa_id
                            from pendencia_fechamento_atividade_avaliativa 
                            where pendencia_fechamento_id = any(@idsPendenciaFechamento)";

            return await database.Conexao.QueryAsync<long>(query, new { idsPendenciaFechamento = idsPendenciaFechamento.ToArray() });
        }
    }
}
