using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterSituacaoEncaminhamentoNAAPAUseCase
    {
        Task<SituacaoDto> Executar(long id);
    }
}
