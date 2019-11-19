using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoEsporadica : RepositorioBase<AtribuicaoEsporadica>, IRepositorioAtribuicaoEsporadica
    {
        public RepositorioAtribuicaoEsporadica(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> ListarAtribuicoes(int anoLetivo, string dreId, string ueId, string codigoRF = "")
        {
        }
    }
}