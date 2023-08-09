using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery : IRequest<bool>
    {
        public ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; } 
    }

    public class ExisteEncaminhamentoNAAPAAtivoParaAlunoQueryValidator : AbstractValidator<ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery>
    {
        public ExisteEncaminhamentoNAAPAAtivoParaAlunoQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a verificação de existência do encaminhamento naapa para o aluno.");
        }
    }
}
