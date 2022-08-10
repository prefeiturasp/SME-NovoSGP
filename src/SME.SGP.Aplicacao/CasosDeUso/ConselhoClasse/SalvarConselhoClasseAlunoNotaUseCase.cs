using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoNotaUseCase : ISalvarConselhoClasseAlunoNotaUseCase
    {
        private readonly IMediator mediator;

        public SalvarConselhoClasseAlunoNotaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Executar(SalvarConselhoClasseAlunoNotaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(dto.CodigoTurma));

            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(dto.FechamentoTurmaId,
                dto.CodigoAluno, ehAnoAnterior));

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();
            var periodoEscolar = new PeriodoEscolar();

            if (fechamentoTurma == null)
            {
                if (!ehAnoAnterior)
                    throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                periodoEscolar =
                    await mediator.Send(
                        new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, dto.Bimestre));

                if (periodoEscolar == null && dto.Bimestre > 0)
                    throw new NegocioException("Período escolar não encontrado");

                fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = turma.Id,
                    Migrado = false,
                    PeriodoEscolarId = periodoEscolar?.Id,
                    Turma = turma,
                    PeriodoEscolar = periodoEscolar
                };

                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = dto.ConselhoClasseNotaDto.CodigoComponenteCurricular
                };
            }
            else
            {
                if (fechamentoTurma.PeriodoEscolarId != null)
                    periodoEscolar =
                        await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
            }

            await mediator.Send(new GravarFechamentoTurmaConselhoClasseCommand(
                fechamentoTurma, fechamentoTurmaDisciplina, periodoEscolar?.Bimestre));

            return await mediator.Send(new GravarConselhoClasseCommad(fechamentoTurma, dto.ConselhoClasseId, dto.CodigoAluno,
                dto.ConselhoClasseNotaDto, periodoEscolar?.Bimestre));
        }
    }
}