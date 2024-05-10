﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTurmasParaCopiaUseCase
    {
        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Executar(int turmaId, long componenteCurricularId, bool ensinoEspecial, bool consideraHistorico);
    }
}