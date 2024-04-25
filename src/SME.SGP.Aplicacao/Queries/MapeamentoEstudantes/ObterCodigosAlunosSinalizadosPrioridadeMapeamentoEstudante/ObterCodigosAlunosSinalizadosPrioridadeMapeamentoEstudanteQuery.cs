using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery : IRequest<string[]>
    {
        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryValidator : AbstractValidator<ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery>
    {
        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para obter os alunos sinalizados como prioridade para o Mapeamento de Estudantes");
        }
    }
}
