﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterNotasParaAvaliacoesUseCase
    {
        Task<NotasConceitosRetornoDto> Executar(ListaNotasConceitosDto filtro);
    }
}
