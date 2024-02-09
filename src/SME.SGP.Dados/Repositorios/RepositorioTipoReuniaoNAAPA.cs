using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoReuniaoNAAPA : IRepositorioTipoReuniaoNAAPA
    {
        private readonly ISgpContext contexto;
        public RepositorioTipoReuniaoNAAPA(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<IEnumerable<TipoReuniaoDto>> ObterTiposDeReuniao()
        {
            var query = "SELECT id, titulo FROM tipo_reuniao_naapa";

            return await contexto.Conexao.QueryAsync<TipoReuniaoDto>(query);
        }
    }
}
