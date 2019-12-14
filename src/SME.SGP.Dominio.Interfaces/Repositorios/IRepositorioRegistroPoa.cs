using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroPoa : IRepositorioBase<RegistroPoa>
    {
        Task<PaginacaoResultadoDto<RegistroPoa>> ListarPaginado(string codigoRf, string dreId, int mes, string ueId, string titulo, Paginacao paginacao);
    }
}