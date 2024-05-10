using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IObterDiasLetivosPorCalendarioUseCase : IUseCase<FiltroDiasLetivosDTO, DiasLetivosDto>
    {
    }
}
