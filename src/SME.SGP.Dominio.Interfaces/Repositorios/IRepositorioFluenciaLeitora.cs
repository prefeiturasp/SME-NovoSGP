using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioFluenciaLeitora : IRepositorioBase<FluenciaLeitora>
    {
        Task<FluenciaLeitora> ObterRegistroFluenciaLeitoraAsync(int anoLetivo, string codigoEOLTurma, string codigoEOLAluno, int tipoAvaliacao);
    }
}
