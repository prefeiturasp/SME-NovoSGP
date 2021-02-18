using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaNotificacaoConclusaoEncaminhamentoAEEUseCase
    {
        Task Executar(long encaminhamentoAEEId, string usuarioRF, string usuarioNome);
    }
}
