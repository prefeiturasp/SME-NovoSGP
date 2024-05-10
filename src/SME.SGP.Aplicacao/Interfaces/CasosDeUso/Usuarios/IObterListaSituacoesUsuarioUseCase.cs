﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterListaSituacoesUsuarioUseCase
    {
        Task<IEnumerable<KeyValuePair<int, string>>> Executar();
    }
}
