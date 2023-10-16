using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQueryHandler : IRequestHandler<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery, List<ConselhoClasseAlunoDto>>
    {
        private readonly IMediator mediator;
        public ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<ConselhoClasseAlunoDto>> Handle(ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery request, CancellationToken cancellationToken)
        {
            var situacaoConselhoClasse = request.SituacaoConselhoClasse;
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, request.Bimestre));

            if (periodoEscolar == null && request.Bimestre != 0)
                throw new NegocioException("Periodo escolar não encontrado");

            if (request.Bimestre == 0)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, 1));

            var alunosTurmaEol = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turma.CodigoTurma, true));
            var consolidadoConselhosClasses = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaBimestreQuery(turma.Id, request.Bimestre, request.SituacaoConselhoClasse));
            var codigosAlunos = consolidadoConselhosClasses.Select(c => c.AlunoCodigo).ToArray();
            var alunosEol = alunosTurmaEol.Where(a => codigosAlunos.Contains(a.CodigoAluno));
            if (periodoEscolar != null)
            {
                var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(periodoEscolar.TipoCalendarioId, turma.EhTurmaInfantil, periodoEscolar.Bimestre));

                if (periodoFechamento != null)
                    alunosEol = alunosEol.Where(a => !a.Inativo || a.Inativo && a.DataSituacao >= periodoFechamento.InicioDoFechamento);
            }
            var alunos = alunosEol.DistinctBy(a => a.CodigoAluno);

            var lista = new List<ConselhoClasseAlunoDto>();

            var pareceresConclusivosDoPeriodo = await mediator.Send(new ObterPareceresConclusivosQuery(periodoEscolar.PeriodoFim));
            var pareceresConclusivosDoPeriodoAnoAnterior = await mediator.Send(new ObterPareceresConclusivosQuery(new System.DateTime((periodoEscolar.PeriodoFim.Year - 1), periodoEscolar.PeriodoFim.Month, periodoEscolar.PeriodoFim.Day)));

            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunos.Select(x => x.CodigoAluno).ToArray(), periodoEscolar.PeriodoFim.Year);

            foreach (var aluno in alunosEol)
            {
                var consolidadoConselhoClasse = consolidadoConselhosClasses.FirstOrDefault(a => a.AlunoCodigo == aluno.CodigoAluno.ToString());
              
                if (consolidadoConselhoClasse == null)
                    continue;

                var frequenciaGlobal = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(aluno.CodigoAluno.ToString(), turma.CodigoTurma));
                string parecerConclusivo = RetornaNomeParecerConclusivoAluno(pareceresConclusivosDoPeriodo, pareceresConclusivosDoPeriodoAnoAnterior, consolidadoConselhoClasse.ParecerConclusivoId);

                lista.Add(new ConselhoClasseAlunoDto()
                {
                    NumeroChamada = aluno.NumeroAlunoChamada ?? 0,
                    AlunoCodigo = aluno.CodigoAluno.ToString(),
                    NomeAluno = aluno.NomeAluno,
                    SituacaoFechamento = consolidadoConselhoClasse.Status.Name(),
                    SituacaoFechamentoCodigo = (int)consolidadoConselhoClasse.Status,
                    FrequenciaGlobal = frequenciaGlobal,
                    PodeExpandir = true,
                    ParecerConclusivo = parecerConclusivo,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                });
            }

            return situacaoConselhoClasse != -99
                ? lista.Where(l => l.SituacaoFechamentoCodigo == situacaoConselhoClasse).OrderBy(a => a.NomeAluno).ToList()
                : lista.OrderBy(a => a.NomeAluno).ToList();
        }
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        public string RetornaNomeParecerConclusivoAluno(IEnumerable<ConselhoClasseParecerConclusivoDto> pareceresAtivosDoPeriodo, IEnumerable<ConselhoClasseParecerConclusivoDto> pareceresConclusivosDoPeriodoAnoAnterior, long? parecerConclusivoAlunoId)
        {
            if (parecerConclusivoAlunoId == null)
                return "Sem parecer";
            else
            {
                if (pareceresAtivosDoPeriodo.FirstOrDefault(a => a.Id == parecerConclusivoAlunoId) != null)
                    return pareceresAtivosDoPeriodo.FirstOrDefault(a => a.Id == parecerConclusivoAlunoId).Nome;
                else
                    return pareceresConclusivosDoPeriodoAnoAnterior.FirstOrDefault(a => a.Id == parecerConclusivoAlunoId)?.Nome ?? "Sem parecer";
            }
        }
    }
}
