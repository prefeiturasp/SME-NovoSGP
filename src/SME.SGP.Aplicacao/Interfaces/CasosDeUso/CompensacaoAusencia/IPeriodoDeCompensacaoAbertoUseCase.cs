using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPeriodoDeCompensacaoAbertoUseCase
    {
        Task<bool> VerificarPeriodoAberto(string turmaCodigo, int bimestre);
    }
}