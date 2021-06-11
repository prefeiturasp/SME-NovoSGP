using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase
    {
        Task<bool> Executar(long turmaId, int semestre, string alunoCodigo);
    }
}