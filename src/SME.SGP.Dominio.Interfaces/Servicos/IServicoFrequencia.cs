using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFrequencia
    {
        Task ExcluirFrequenciaAula(long aulaId);

        Task<IEnumerable<RegistroAusenciaAluno>> ObterListaAusenciasPorAula(long aulaId);

        Task<RegistroFrequencia> ObterRegistroFrequenciaPorAulaId(long aulaId);

        Task Registrar(long aulaId, IEnumerable<RegistroAusenciaAluno> registroAusenciaAlunos);

        void AtualizarQuantidadeFrequencia(long aulaId, int quantidadeOriginal, int quantidadeAtual);
    }
}