using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoTurmaCommand : IRequest<AcompanhamentoTurma>
    {
        public SalvarAcompanhamentoTurmaCommand(AcompanhamentoTurma acompanhamento)
        {
            Acompanhamento = acompanhamento;
        }

        public AcompanhamentoTurma Acompanhamento { get; }
    }

    public class SalvarAcompanhamentoTurmaCommandValidator : AbstractValidator<SalvarAcompanhamentoTurmaCommand>
    {
        public SalvarAcompanhamentoTurmaCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("O acompanhamento da turma deve ser informado para persistência");
        }
    }
}
