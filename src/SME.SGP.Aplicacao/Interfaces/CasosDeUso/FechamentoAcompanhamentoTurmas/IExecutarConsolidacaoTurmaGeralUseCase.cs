using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutarConsolidacaoTurmaGeralUseCase
    {
        Task Executar(string turmaCodigo, int? bimestre);
    }
}