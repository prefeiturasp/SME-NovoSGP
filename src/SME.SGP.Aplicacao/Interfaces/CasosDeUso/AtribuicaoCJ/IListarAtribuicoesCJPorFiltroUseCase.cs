﻿using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IListarAtribuicoesCJPorFiltroUseCase : IUseCase<AtribuicaoCJListaFiltroDto, IEnumerable<AtribuicaoCJListaRetornoDto>>
    {
    }
}
