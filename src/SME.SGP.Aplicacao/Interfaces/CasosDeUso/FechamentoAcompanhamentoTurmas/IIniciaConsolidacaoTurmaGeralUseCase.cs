using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IIniciaConsolidacaoTurmaGeralUseCase
    {
        Task Executar(string turmaCodigo, int? bimestre);
    }
}
