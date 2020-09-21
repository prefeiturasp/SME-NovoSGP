using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase : IUseCase<FiltroTurmaComponenteDto, DateTime?>
    {
    }
}
