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

            var totalAulas = 0;
            var totalAusencias = 0;
            var totalCompensacoes = 0;

            foreach (var frequencia in frequenciasAluno)
            {
                totalAulas = frequencia.Sum(f => f.TotalAulas);
                totalAulas = frequencia.Sum(f => f.TotalAusencias);
                totalAulas = frequencia.Sum(f => f.TotalCompensacoes);
                
                var frequenciaBimestreAluno = new FrequenciaBimestreAlunoDto()
                {
                    Bimestre = frequencia.Key.Bimestre,
                    CodigoAluno = frequencia.Key.CodigoAluno,
                    Frequencia = PercentualFrequencia(totalAulas, totalAusencias, totalCompensacoes).percentualFrequencia,
                    QuantidadeAusencias = totalAusencias,
                    QuantidadeCompensacoes = totalCompensacoes,
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
            totalAulas = frequenciaAlunoBimestre.Sum(f => f.TotalAulas);
            totalAusencias = frequenciaAlunoBimestre.Sum(f => f.QuantidadeAusencias);
            totalCompensacoes = frequenciaAlunoBimestre.Sum(f => f.QuantidadeCompensacoes);
            informacoesEscolaresAluno.FrequenciaGlobal = PercentualFrequencia(totalAulas, totalAusencias, totalCompensacoes).percentualFrequenciaFormatado;

            return informacoesEscolaresAluno;
        }

        private (double percentualFrequencia,string percentualFrequenciaFormatado) PercentualFrequencia(int totalAulas,int totalAusencias,int totalCompensacoes)
        {
           var frequenciaAlunoCalculo = new FrequenciaAluno()
            {
                TotalAulas = totalAulas,
                TotalAusencias = totalAusencias,
                TotalCompensacoes = totalCompensacoes
            };
            return (frequenciaAlunoCalculo.PercentualFrequencia,frequenciaAlunoCalculo.PercentualFrequenciaFormatado);   
        }
    }
}