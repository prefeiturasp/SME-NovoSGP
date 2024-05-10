using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoParecerPorAlunoCommand : IRequest
    {
        public ExcluirWFAprovacaoParecerPorAlunoCommand(long conselhoClasseAlunoId)
        {
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
        }

        public long ConselhoClasseAlunoId { get; }
    }

    public class ExcluirWFAprovacaoParecerPorAlunoCommandValidator : AbstractValidator<ExcluirWFAprovacaoParecerPorAlunoCommand>
    {
        public ExcluirWFAprovacaoParecerPorAlunoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("O identificador do conselho de classe do aluno deve ser informado para exclusão de workflows existentes");
        }
    }
}
