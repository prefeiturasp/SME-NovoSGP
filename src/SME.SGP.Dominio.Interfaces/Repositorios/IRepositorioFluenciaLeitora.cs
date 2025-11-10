using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioFluenciaLeitora : IRepositorioBase<FluenciaLeitora>
    {
        Task<FluenciaLeitora> ObterRegistroFluenciaLeitoraAsync(int anoLetivo, string codigoEOLTurma, string codigoEOLAluno, int tipoAvaliacao);
        Task<IEnumerable<PainelEducacionalRegistroFluenciaLeitoraDto>> ObterRegistroFluenciaLeitoraGeralAsync();
    }
}
