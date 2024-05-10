using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMotivoAusencia : IRepositorioMotivoAusencia
    {
        private readonly ISgpContext contexto;

        public RepositorioMotivoAusencia(ISgpContext contexto)
        {
            this.contexto = contexto;
        }
      
        public async Task<IEnumerable<MotivoAusencia>> ListarAsync()
        {
            return await contexto.Conexao.QueryAsync<MotivoAusencia>("select id, descricao from motivo_ausencia");
        }

        public async Task<MotivoAusencia> ObterPorIdAsync(long id)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<MotivoAusencia>("select id, descricao from motivo_ausencia where id = @id", new { id });
        }
    }
}
