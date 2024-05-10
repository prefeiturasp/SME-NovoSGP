﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase : AbstractUseCase, IObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase
    {
        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DropdownTurmaRetornoDto>> Executar(int anoLetivo, string ueCodigo, int[] modalidades, int semestre, string[] anos, bool historico)
            => await mediator.Send(new ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery(anoLetivo,
                                                                                                      ueCodigo,
                                                                                                      modalidades,
                                                                                                      semestre,
                                                                                                      anos, historico));
    }
}
