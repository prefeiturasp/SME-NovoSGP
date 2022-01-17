using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoTurmaItinerario : IRepositorioTipoTurmaItinerario
    {
        private readonly ISgpContext contexto;

        public RepositorioTipoTurmaItinerario(ISgpContext contexto)
        {
            this.contexto = contexto;
        }
        
        public async Task<IEnumerable<TipoTurmaItinerario>> ObterPorSerie(int serie)
        {
            return await contexto.Conexao.QueryAsync<TipoTurmaItinerario>("select * from tipo_turma_itinerario where serie = @serie", new { serie });
        }

        public async Task<TipoTurmaItinerario> ObterPorId(int id)
        {
            return await contexto.QueryFirstOrDefaultAsync<TipoTurmaItinerario>("select * from tipo_turma_itinerario where id = @id", new { id });
        }
    }
}
