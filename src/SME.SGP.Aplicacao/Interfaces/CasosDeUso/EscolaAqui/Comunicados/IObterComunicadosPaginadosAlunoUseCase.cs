using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface IObterComunicadosPaginadosAlunoUseCase
    {
        Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> Executar(FiltroTurmaAlunoSemestreDto dto);
    }
}
