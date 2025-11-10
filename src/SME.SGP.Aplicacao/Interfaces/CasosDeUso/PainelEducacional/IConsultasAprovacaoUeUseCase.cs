using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAprovacaoUeUseCase
    {
        Task<PainelEducacionalAprovacaoUeResultadoDto> ObterAprovacao(int anoLetivo, string codigoUe, int numeroPagina, int numeroRegistros);
    }
}
