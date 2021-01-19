using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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
            var informacoesEscolaresAluno = new InformacoesEscolaresAlunoDto();

            var necessidadesEspeciaisAluno = await servicoEOL.ObterNecessidadesEspeciaisAluno(request.CodigoAluno);

            if (necessidadesEspeciaisAluno != null)
                informacoesEscolaresAluno = necessidadesEspeciaisAluno;

            informacoesEscolaresAluno.FrequenciaAlunoPorBimestres = await mediator.Send(new ObterFrequenciaBimestresQuery(request.CodigoAluno, 0, request.TurmaCodigo, TipoFrequenciaAluno.Geral));

            informacoesEscolaresAluno.FrequenciaGlobal = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(request.CodigoAluno, request.TurmaCodigo));

            return informacoesEscolaresAluno;
        }
    }
}