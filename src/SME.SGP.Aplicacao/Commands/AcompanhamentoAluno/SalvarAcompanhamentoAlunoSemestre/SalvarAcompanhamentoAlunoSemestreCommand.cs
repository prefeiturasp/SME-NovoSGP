using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoAlunoSemestreCommand : IRequest<AcompanhamentoAlunoSemestre>
    {
        public SalvarAcompanhamentoAlunoSemestreCommand(AcompanhamentoAlunoSemestre acompanhamento)
        {
            Acompanhamento = acompanhamento;
        }

        public AcompanhamentoAlunoSemestre Acompanhamento { get; }
    }

    public class SalvarAcompanhamentoAlunoSemestreCommandValidator : AbstractValidator<SalvarAcompanhamentoAlunoSemestreCommand>
    {
        public SalvarAcompanhamentoAlunoSemestreCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("O acompanhamento do aluno no semestre deve ser informado para persistência");
        }
    }
}
