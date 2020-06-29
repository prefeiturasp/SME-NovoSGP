using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacaoFrequencia
    {
        void ExecutaNotificacaoRegistroFrequencia();
        void VerificaRegraAlteracaoFrequencia(long registroFrequenciaId, DateTime criadoEm, DateTime alteradoEm, long usuarioAlteracaoId);
        Task NotificarCompensacaoAusencia(long compensacaoId);
        Task VerificaNotificacaoBimestral();
        Task NotificarAlunosFaltosos();
        Task NotificarAlunosFaltososBimestre();
    }
}
