using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarEncaminhamentoNAAPAUseCase
    {
        Task<ResultadoEncaminhamentoNAAPADto> Executar(EncaminhamentoNAAPADto encaminhamentoNAAPADto);
    }
}
