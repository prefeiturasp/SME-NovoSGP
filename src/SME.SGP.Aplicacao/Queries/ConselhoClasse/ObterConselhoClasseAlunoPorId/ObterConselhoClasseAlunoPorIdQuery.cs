using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorIdQuery : IRequest<ConselhoClasseAluno>
    {
        public ObterConselhoClasseAlunoPorIdQuery(long conselhoClasseId)
        {
            ConselhoClasseAlunoId = conselhoClasseId;
        }

        public long ConselhoClasseAlunoId { get; set; }
    }

    public class ObterConselhoClasseAlunoPorIdQueryValidator : AbstractValidator<ObterConselhoClasseAlunoPorIdQuery>
    {

        public ObterConselhoClasseAlunoPorIdQueryValidator()
        {
            RuleFor(c => c.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("Para obter as informações do conselho de classe do aluno, o id conselho de classe aluno deve ser informado.");
        }
    }
}