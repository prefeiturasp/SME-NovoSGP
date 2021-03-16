using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Turma
{
    public class ObterPeriodoLetivoTurmaUseCase : AbstractUseCase, IObterPeriodoLetivoTurmaUseCase
    {
        public ObterPeriodoLetivoTurmaUseCase(IMediator mediator)
            : base (mediator)
        {
        }

        public async Task<PeriodoEscolarLetivoTurmaDto> Executar(string codigoTurma)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException($"A turma com o código {codigoTurma} não foi localizada.");

            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            if (periodos != null && periodos.Any())
            {
                return new PeriodoEscolarLetivoTurmaDto()
                {
                    TurmaCodigo = codigoTurma,
                    PeriodoInicio = periodos.OrderBy(p => p.Bimestre).First().PeriodoInicio,
                    PeriodoFim = periodos.OrderBy(p => p.Bimestre).Last().PeriodoFim
                };
            }

            return null;
        }
    }
}
