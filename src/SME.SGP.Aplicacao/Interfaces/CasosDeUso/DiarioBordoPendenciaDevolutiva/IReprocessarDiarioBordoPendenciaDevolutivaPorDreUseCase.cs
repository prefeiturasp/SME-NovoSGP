﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}
