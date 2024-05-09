using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPAPCommandHandler : IRequestHandler<SalvarRelatorioPAPCommand,ResultadoRelatorioPAPDto>
    {
        private readonly IMediator mediator;

        public SalvarRelatorioPAPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoRelatorioPAPDto> Handle(SalvarRelatorioPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorioPapDto = request.RelatorioPAPDto;
            var resultado = new ResultadoRelatorioPAPDto();
            var relatorioTurma = await mediator.Send(new SalvarRelatorioPeriodicoTurmaPAPCommand(relatorioPapDto.TurmaId, relatorioPapDto.periodoRelatorioPAPId), cancellationToken);
            var relatorioAluno = await mediator.Send(new PersistirRelatorioAlunoCommand(relatorioPapDto, relatorioTurma.Id), cancellationToken);

            resultado.PAPTurmaId = relatorioTurma.Id;
            resultado.PAPAlunoId = relatorioAluno.Id;

            foreach (var secao in relatorioPapDto.Secoes)
            {
                var seccao = await mediator.Send(new SalvarSecaoCommand(secao, relatorioAluno.Id), cancellationToken);
                resultado.Secoes.Add(seccao);
            }

            return resultado;
        }
    }
}