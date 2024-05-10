using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQueryValidator : AbstractValidator<ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery>
    {
        public ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .GreaterThan(0)
                .WithMessage("O ID da turma percisa ser informado");

            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado");
        }
    }
}
