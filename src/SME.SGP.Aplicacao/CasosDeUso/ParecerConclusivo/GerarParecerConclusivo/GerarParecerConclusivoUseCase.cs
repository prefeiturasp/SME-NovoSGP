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
        public GerarParecerConclusivoUseCase(IMediator mediator) : base(mediator)
        {
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