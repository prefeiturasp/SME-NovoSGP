using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterDiarioBordoUseCase : IUseCase<long, DiarioBordoDto>
    {
    }
}
