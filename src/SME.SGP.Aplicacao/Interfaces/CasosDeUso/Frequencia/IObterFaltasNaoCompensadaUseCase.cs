﻿using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFaltasNaoCompensadaUseCase : IUseCase<FiltroFaltasNaoCompensadasDto,IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
    
    }
}