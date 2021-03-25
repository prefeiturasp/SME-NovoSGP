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
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo));
            var tipoCalendarioId = turma.ModalidadeCodigo == Modalidade.EJA ? await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma)) : 0;

            var necessidadesEspeciaisAluno = await servicoEOL.ObterNecessidadesEspeciaisAluno(request.CodigoAluno);

            if (necessidadesEspeciaisAluno != null)
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

            if (frequenciasAluno == null || !frequenciasAluno.Any())
            {
                informacoesEscolaresAluno.FrequenciaGlobal = 100;
                return informacoesEscolaresAluno;
            }                

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciasAluno.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciasAluno.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciasAluno.Sum(f => f.TotalCompensacoes),
            };

            informacoesEscolaresAluno.FrequenciaGlobal = frequenciaAluno.PercentualFrequencia;

            return informacoesEscolaresAluno;
        }
    }
}