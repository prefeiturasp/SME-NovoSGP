using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadorMapeamentoEstudanteQuery : IRequest<long?>
    {
        public ObterIdentificadorMapeamentoEstudanteQuery(string codigoAluno, long turmaId, int bimestre)
        {
            CodigoAluno = codigoAluno;
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public string CodigoAluno { get; set; } 
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterIdentificadorMapeamentoEstudanteQueryValidator : AbstractValidator<ObterIdentificadorMapeamentoEstudanteQuery>
    {
        public ObterIdentificadorMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para obter o identificador do mapeamento do estudante");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para obter o identificador do mapeamento do estudante");

            RuleFor(c => c.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para obter o identificador do mapeamento do estudante");
        }
    }
}
