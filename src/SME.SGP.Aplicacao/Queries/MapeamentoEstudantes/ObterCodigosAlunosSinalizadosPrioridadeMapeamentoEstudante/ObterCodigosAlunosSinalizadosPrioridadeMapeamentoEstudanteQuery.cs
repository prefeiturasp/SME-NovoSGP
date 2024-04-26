using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery : IRequest<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]>
    {
        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryValidator : AbstractValidator<ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery>
    {
        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para obter os alunos sinalizados como prioridade para o Mapeamento de Estudantes");
            RuleFor(c => c.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para obter os alunos sinalizados como prioridade para o Mapeamento de Estudantes");
        }
    }
}
