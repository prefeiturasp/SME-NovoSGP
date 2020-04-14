using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacaoFrequencia
    {
        void ExecutaNotificacaoRegistroFrequencia();
        void VerificaRegraAlteracaoFrequencia(long registroFrequenciaId, DateTime criadoEm, DateTime alteradoEm, long usuarioAlteracaoId);
        void NotificarCompensacaoAusencia(long compensacaoId);
        void VerificaNotificacaoBimestral();
        void NotificarAlunosFaltosos();
        void NotificarAlunosFaltososBimestre();
    }
}
