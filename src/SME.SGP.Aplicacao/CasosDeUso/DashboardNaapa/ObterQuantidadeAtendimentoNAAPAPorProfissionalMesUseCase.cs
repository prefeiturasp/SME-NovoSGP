﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase : IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<GraficoEncaminhamentoNAAPADto> Executar(FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto param)
        {
            return mediator.Send(new ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery(param.AnoLetivo, param.DreId, param.UeId, param.Mes));
        }
    }
}
