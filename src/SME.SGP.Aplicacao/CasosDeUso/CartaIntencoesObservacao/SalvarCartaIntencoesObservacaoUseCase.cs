using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCartaIntencoesObservacaoUseCase : ISalvarCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;

        public SalvarCartaIntencoesObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Executar(long turmaId, long componenteCurricularId, SalvarCartaIntencoesObservacaoDto dto)
        {
            var usuarioLogadoId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new SalvarCartaIntencoesObservacaoCommand(turmaId, componenteCurricularId, usuarioLogadoId, dto.Observacao));
            throw new NotImplementedException();
        }
    }
}
