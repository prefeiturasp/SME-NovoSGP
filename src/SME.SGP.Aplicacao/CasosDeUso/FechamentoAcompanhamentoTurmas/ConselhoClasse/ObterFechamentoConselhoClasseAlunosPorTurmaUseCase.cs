using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConselhoClasseAlunosPorTurmaUseCase : AbstractUseCase, IObterFechamentoConselhoClasseAlunosPorTurmaUseCase
    {
        public ObterFechamentoConselhoClasseAlunosPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ConselhoClasseAlunoDto>> Executar(FiltroConselhoClasseConsolidadoTurmaBimestreDto param)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(param.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var alunosTurmaEol = await mediator.Send(new ObterAlunosEolPorTurmaQuery(turma.CodigoTurma, true));

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, param.Bimestre));
            if (periodoEscolar == null && param.Bimestre != 0)
                throw new NegocioException("Periodo escolar não encontrado");

            if (param.Bimestre == 0)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, 1));
            
            var consolidadoConselhosClasses = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaBimestreQuery(turma.Id, param.Bimestre, param.SituacaoConselhoClasse));

            var codigosAlunos = consolidadoConselhosClasses.Select(c => c.AlunoCodigo).ToArray();
            var alunosEol = alunosTurmaEol.Where(a => codigosAlunos.Contains(a.CodigoAluno));
            
            if(periodoEscolar != null)
            {
                var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(periodoEscolar.TipoCalendarioId, turma.EhTurmaInfantil, periodoEscolar.Bimestre));

                if(periodoFechamento != null)
                    alunosEol = alunosEol.Where(a => !a.Inativo || a.Inativo && a.DataSituacao >= periodoFechamento.InicioDoFechamento);
            }

            return await MontarRetorno(alunosEol.DistinctBy(a => a.CodigoAluno), consolidadoConselhosClasses, turma.CodigoTurma, turma.Id, periodoEscolar.Bimestre, param.SituacaoConselhoClasse);
        }

        private async Task<IEnumerable<ConselhoClasseAlunoDto>> MontarRetorno(IEnumerable<AlunoPorTurmaResposta> alunos, IEnumerable<ConselhoClasseConsolidadoTurmaAluno> consolidadoConselhosClasses, string codigoTurma, long turmaId, int bimestre, int situacaoConselhoClasse)
        {
            List<ConselhoClasseAlunoDto> lista = new List<ConselhoClasseAlunoDto>();
            var pareceresConclusivos = await mediator.Send(new ObterPareceresConclusivosQuery());

            var dadosStatusAlunoConselhoConsolidado = await mediator.Send(new ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery(turmaId, bimestre));

            foreach (var aluno in alunos)
            {
                var consolidadoConselhoClasse = consolidadoConselhosClasses.FirstOrDefault(a => a.AlunoCodigo == aluno.CodigoAluno.ToString());
                var situacaoConselhoClasseAlunoAjustada = dadosStatusAlunoConselhoConsolidado.FirstOrDefault(s => s.AlunoCodigo == aluno.CodigoAluno.ToString());

                if (consolidadoConselhoClasse == null)
                    continue;

                var frequenciaGlobal = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(aluno.CodigoAluno.ToString(), codigoTurma));
                string parecerConclusivo = consolidadoConselhoClasse.ParecerConclusivoId != null ? pareceresConclusivos.FirstOrDefault(a => a.Id == consolidadoConselhoClasse.ParecerConclusivoId).Nome : "Sem parecer";

                lista.Add(new ConselhoClasseAlunoDto()
                {
                    NumeroChamada = aluno.NumeroAlunoChamada ?? 0,
                    AlunoCodigo = aluno.CodigoAluno.ToString(),
                    NomeAluno = aluno.NomeAluno,
                    SituacaoFechamento = situacaoConselhoClasseAlunoAjustada != null ? ((SituacaoConselhoClasse)situacaoConselhoClasseAlunoAjustada.StatusConselhoClasseAluno).Name() : consolidadoConselhoClasse.Status.Name(),
                    SituacaoFechamentoCodigo = situacaoConselhoClasseAlunoAjustada != null ? situacaoConselhoClasseAlunoAjustada.StatusConselhoClasseAluno : (int)consolidadoConselhoClasse.Status,
                    FrequenciaGlobal = frequenciaGlobal,
                    PodeExpandir = true,
                    ParecerConclusivo = parecerConclusivo
                });
            }

            return situacaoConselhoClasse != -99
                ? lista.Where(l => l.SituacaoFechamentoCodigo == situacaoConselhoClasse).OrderBy(a => a.NomeAluno).ToList() 
                : lista.OrderBy(a => a.NomeAluno).ToList();

        }
    }
}
