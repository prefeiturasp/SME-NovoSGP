using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioItineranciaEvento : RepositorioBase<ItineranciaEvento>, IRepositorioItineranciaEvento
    {
        public RepositorioItineranciaEvento(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<long>> ObterEventosIdsPorItinerancia(long itineranciaId)
        {
            var query = "select evento_id from itinerancia_evento where not excluido and itinerancia_id = @itineranciaId";

            return await database.Conexao.QueryAsync<long>(query, new { itineranciaId });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorItineranciaId(long itineranciaId)
        {
            var query = @"select e.* from evento e
                          inner join itinerancia_evento ie on ie.evento_id = e.id
                          where not ie.excluido
                            and not e.excluido
                            and ie.itinerancia_id = @itineranciaId";

            return await database.Conexao.QueryAsync<Evento>(query, new { itineranciaId });
        }
    }
}
