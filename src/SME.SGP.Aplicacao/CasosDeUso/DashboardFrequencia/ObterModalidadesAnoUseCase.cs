﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesAnoUseCase : IObterModalidadesAnoUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadesAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<RetornoModalidadesPorAnoDto>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre)
        {
            var modalidadesPorAnoRetornoDto = new List<RetornoModalidadesPorAnoDto>();
            var modalidades = await mediator.Send(new ObterModalidadesPorAnosQuery(anoLetivo, dreId, ueId, modalidade, semestre));
            foreach (var item in modalidades.GroupBy(s=> new {s.Modalidade, s.Ano}))
            {
                modalidadesPorAnoRetornoDto.Add(new RetornoModalidadesPorAnoDto()
                {
                    ModalidadeAno = $"{item.Key.Modalidade.ShortName()}-{item.Key.Ano}",
                    Ano = item.Key.Ano
                });
            }
            return modalidadesPorAnoRetornoDto;
        }
    }
}
