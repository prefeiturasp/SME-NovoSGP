using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
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

            var necessidadesEspeciaisAluno = await mediator.Send(new ObterNecessidadesEspeciaisAlunoEolQuery(request.CodigoAluno));

            if (necessidadesEspeciaisAluno != null)
                informacoesEscolaresAluno = necessidadesEspeciaisAluno;

            var frequenciasAluno = (await mediator.Send(new ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQuery(request.CodigoAluno, turma.AnoLetivo, tipoCalendarioId))).GroupBy(x => (x.Bimestre, x.CodigoAluno));

            var frequenciaBimestreAlunoDto = new List<FrequenciaBimestreAlunoDto>();

            foreach (var frequencia in frequenciasAluno)
            {
                var frequenciaBimestreAluno = new FrequenciaBimestreAlunoDto()
                {
                    Bimestre = frequencia.Key.Bimestre,
                    CodigoAluno = frequencia.Key.CodigoAluno,
                    Frequencia = frequencia.Sum(f => f.PercentualFrequencia),
                    QuantidadeAusencias = frequencia.Sum(f => f.TotalAusencias),
                    QuantidadeCompensacoes = frequencia.Sum(f => f.TotalCompensacoes),
                    TotalAulas = frequencia.Sum(f => f.TotalAulas)
                };

                frequenciaBimestreAlunoDto.Add(frequenciaBimestreAluno);
            }

            informacoesEscolaresAluno.FrequenciaAlunoPorBimestres = frequenciaBimestreAlunoDto;

            if (frequenciasAluno == null || !frequenciasAluno.Any())
            {
                informacoesEscolaresAluno.FrequenciaGlobal = "";
                return informacoesEscolaresAluno;
            }
            var frequenciaAlunoBimestre = informacoesEscolaresAluno.FrequenciaAlunoPorBimestres;
            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoBimestre.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoBimestre.Sum(f => f.QuantidadeAusencias),
                TotalCompensacoes = frequenciaAlunoBimestre.Sum(f => f.QuantidadeCompensacoes),
            };

            informacoesEscolaresAluno.FrequenciaGlobal = frequenciaAluno.PercentualFrequenciaFormatado;

            return informacoesEscolaresAluno;
        }
    }
}