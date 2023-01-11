﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterSecoesEncaminhamentosSecaoNAAPAUseCase
    {
        Task<IEnumerable<SecaoQuestionarioDto>> Executar(FiltroSecoesDeEncaminhamento filtro);
    }
}
