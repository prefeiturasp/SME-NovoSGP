using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAbandonoPainelEducacionalUeUseCase
    {
        Task<PainelEducacionalAbandonoUeDto> Executar(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros);
    }
}