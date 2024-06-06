using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarSecaoCommandHandler : IRequestHandler<SalvarSecaoCommand,ResultadoRelatorioPAPSecaoDto>
    {
        private readonly IMediator mediator;

        public SalvarSecaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoRelatorioPAPSecaoDto> Handle(SalvarSecaoCommand request, CancellationToken cancellationToken)
        {
            var relatorioSecao = await mediator.Send(new PersistirRelatorioSecaoCommand(request.Secao, request.RelatorioAlunoId), cancellationToken);

            foreach (var questoes in request.Secao.Respostas.GroupBy(q => q.QuestaoId))
            {
                await mediator.Send(new SalvarQuestaoCommand(relatorioSecao.Id, questoes.Key, questoes), cancellationToken);
            }

            return new ResultadoRelatorioPAPSecaoDto()
            {
                SecaoId = relatorioSecao.Id,
                Auditoria = new AuditoriaDto()
                {
                    Id = request.Secao.SecaoId,
                    CriadoEm = relatorioSecao.CriadoEm,
                    CriadoPor = relatorioSecao.CriadoPor,
                    CriadoRF = relatorioSecao.CriadoRF,
                    AlteradoEm = relatorioSecao.AlteradoEm,
                    AlteradoPor = relatorioSecao.AlteradoPor,
                    AlteradoRF = relatorioSecao.AlteradoRF
                }
            }; 
        }
    }
}