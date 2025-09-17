using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPapConsulta 
    {
        Task<ContagemDificuldadePorTipoDto> ObterContagemDificuldadesPorTipo(TipoPap tipoPap);
    }
}