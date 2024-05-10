﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioPendenciasUseCase
    {
        Task<bool> Executar(FiltroRelatorioPendenciasDto filtroRelatorioPendenciasFechamentoDto);
        List<FiltroTipoPendenciaDto> ListarTodosTipos(bool opcaoTodos);
    }
}