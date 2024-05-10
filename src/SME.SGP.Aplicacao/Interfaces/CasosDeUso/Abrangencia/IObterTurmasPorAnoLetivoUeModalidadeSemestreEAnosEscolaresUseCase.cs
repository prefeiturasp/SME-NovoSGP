﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase
    {
        Task<IEnumerable<DropdownTurmaRetornoDto>> Executar(int anoLetivo, string ueCodigo, int[] modalidades, int semestre, string[] anos, bool historico);
    }
}