using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase
    {
        Task<bool> Executar(long turmaId, int semestre, string alunoCodigo);
    }
}