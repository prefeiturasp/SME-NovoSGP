using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoQueryHandler : IRequestHandler<ObterNecessidadesEspeciaisAlunoQuery, InformacoesEscolaresAlunoDto>
    {
        private readonly IMediator mediator;

        public ObterNecessidadesEspeciaisAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoQuery request, CancellationToken cancellationToken)
        {
            var informacoesEscolaresAluno = new InformacoesEscolaresAlunoDto();
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo));
            var tipoCalendarioId = turma.ModalidadeCodigo == Modalidade.EJA ? await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma)) : 0;

            var necessidadesEspeciaisAluno = await mediator.Send(new ObterNecessidadesEspeciaisAlunoEolQuery(request.CodigoAluno));

            if (necessidadesEspeciaisAluno.NaoEhNulo())
                informacoesEscolaresAluno = necessidadesEspeciaisAluno;

            var frequenciasAluno = await mediator.Send(new ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQuery(request.CodigoAluno, turma.AnoLetivo, tipoCalendarioId));

            var frequenciaBimestreAlunoDto = new List<FrequenciaBimestreAlunoDto>();

            foreach (var frequencia in frequenciasAluno)
            {
                var frequenciaBimestreAluno = new FrequenciaBimestreAlunoDto()
                {
                    Bimestre = frequencia.Bimestre,
                    CodigoAluno = frequencia.CodigoAluno,
                    Frequencia = frequencia.PercentualFrequencia,
                    QuantidadeAusencias = frequencia.TotalAusencias,
                    QuantidadeCompensacoes = frequencia.TotalCompensacoes
                };

                frequenciaBimestreAlunoDto.Add(frequenciaBimestreAluno);
            }

            informacoesEscolaresAluno.FrequenciaAlunoPorBimestres = frequenciaBimestreAlunoDto;

            if (frequenciasAluno.EhNulo() || !frequenciasAluno.Any())
            {
                informacoesEscolaresAluno.FrequenciaGlobal = "";
                return informacoesEscolaresAluno;
            }                

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciasAluno.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciasAluno.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciasAluno.Sum(f => f.TotalCompensacoes),
            };

            informacoesEscolaresAluno.FrequenciaGlobal = frequenciaAluno.PercentualFrequenciaFormatado;

            return informacoesEscolaresAluno;
        }
    }
}