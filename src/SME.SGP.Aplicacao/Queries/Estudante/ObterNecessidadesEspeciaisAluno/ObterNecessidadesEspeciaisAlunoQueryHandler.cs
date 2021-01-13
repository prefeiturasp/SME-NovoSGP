using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQueryHandler : IRequestHandler<ObterNecessidadesEspeciaisAlunoQuery, InformacoesEscolaresAlunoDto>
    {
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ObterNecessidadesEspeciaisAlunoQueryHandler(IServicoEol servicoEOL, IMediator mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoQuery request, CancellationToken cancellationToken)
        {
            var informacoesEscolaresAlunoDto = await servicoEOL.ObterNecessidadesEspeciaisAluno(request.CodigoAluno);

            informacoesEscolaresAlunoDto.FrequenciaGlobal = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(request.CodigoAluno, request.TurmaId));

            return informacoesEscolaresAlunoDto;
        }
    }
}