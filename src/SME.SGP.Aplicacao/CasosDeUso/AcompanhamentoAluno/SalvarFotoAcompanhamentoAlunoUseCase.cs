﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAcompanhamentoAlunoUseCase : AbstractUseCase, ISalvarFotoAcompanhamentoAlunoUseCase
    {
        public SalvarFotoAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AcompanhamentoAlunoDto param)
            => await mediator.Send(new SalvarFotoAcompanhamentoAlunoCommand(param));
    }
}
