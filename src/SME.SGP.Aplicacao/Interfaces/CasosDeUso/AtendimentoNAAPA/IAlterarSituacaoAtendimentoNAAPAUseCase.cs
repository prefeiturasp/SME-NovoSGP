using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IAlterarSituacaoAtendimentoNAAPAUseCase
    {
        Task<bool> Executar(long encaminhamentoId);
    }
}
