﻿using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterFuncionariosABAEUseCase : IUseCase<FiltroFuncionarioDto, IEnumerable<NomeCpfABAEDto>>
    {
    }
}
