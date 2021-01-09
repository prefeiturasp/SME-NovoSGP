using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarEncaminhamentoAEEUseCase
    {
        Task<ResultadoEncaminhamentoAeeDto> Executar(EncaminhamentoAeeDto encaminhamentoDto);
    }
}
