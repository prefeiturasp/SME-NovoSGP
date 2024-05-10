﻿using MediatR;
using SME.SGP.Aplicacao.Queries.Relatorios.ObterFiltroRelatoriosDresPorAbrangencia;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosDresPorAbrangenciaUseCase : IObterFiltroRelatoriosDresPorAbrangenciaUseCase
    {
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosDresPorAbrangenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Executar()
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            return await mediator.Send(new ObterFiltroRelatoriosDresPorAbrangenciaQuery(usuarioLogado));
        }
    }
}
