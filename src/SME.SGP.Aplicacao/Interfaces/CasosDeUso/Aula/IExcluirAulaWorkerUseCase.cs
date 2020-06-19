using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Aula
{
    public interface IExcluirAulaWorkerUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
