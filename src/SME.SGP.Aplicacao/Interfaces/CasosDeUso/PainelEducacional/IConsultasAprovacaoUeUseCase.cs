using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAprovacaoUeUseCase
    {
        Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> ObterAprovacao(int anoLetivo, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros);
    }
}
