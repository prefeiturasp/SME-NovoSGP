using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPapConsulta 
    {
        Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> ObterContagemDificuldadesConsolidadaGeral(int anoLetivo);
    }
}