﻿using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacaoFrequencia
    {
        Task ExecutaNotificacaoRegistroFrequencia();
        Task VerificaRegraAlteracaoFrequencia(long registroFrequenciaId, DateTime criadoEm, DateTime alteradoEm);
        Task NotificarAlunosFaltosos(long ueId);
        Task NotificarAlunosFaltososBimestre();
    }
}
