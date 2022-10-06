using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class GerarParecerConclusivoUseCase: AbstractUseCase, IGerarParecerConclusivoUseCase
    {
        private readonly IMediator mediator;
        
        public GerarParecerConclusivoUseCase(IMediator mediator) : base(mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> Executar(ConselhoClasseFechamentoAlunoDto conselhoClasseFechamentoAlunoDto)
        { 
            return await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(
                conselhoClasseFechamentoAlunoDto.ConselhoClasseId, 
                conselhoClasseFechamentoAlunoDto.FechamentoTurmaId, 
                conselhoClasseFechamentoAlunoDto.AlunoCodigo));
        }
    }
}