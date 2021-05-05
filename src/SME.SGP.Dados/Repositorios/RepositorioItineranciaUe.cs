using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaUe : RepositorioBase<ItineranciaUe>, IRepositorioItineranciaUe
    {
        public RepositorioItineranciaUe(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirItineranciaUe(long ueId, long itineranciaId)
        { 
            await database.Conexao.ExecuteScalarAsync(@"delete from itinerancia_ue iu where itinerancia_id = @itineranciaId and id = @ueId", new { ueId , itineranciaId });
        }
    }
}
