using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroPoa : IRepositorioBase<RegistroPoa>
    {
        Task<PaginacaoResultadoDto<RegistroPoa>> ListarPaginado(string codigoRf, string dreId, int bimestre, string ueId, string titulo, int anoLetivo, Paginacao paginacao);
    }
}