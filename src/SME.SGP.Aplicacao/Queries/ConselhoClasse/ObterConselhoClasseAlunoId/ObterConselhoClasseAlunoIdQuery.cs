using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoIdQuery : IRequest<long>
    {
        public ObterConselhoClasseAlunoIdQuery(long conselhoClasseId, string alunoCodigo)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
        }
        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ObterConselhoClasseAlunoIdQueryValidator : AbstractValidator<ObterConselhoClasseAlunoIdQuery>
    {
        public ObterConselhoClasseAlunoIdQueryValidator()
        {
            RuleFor(a => a.ConselhoClasseId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id do conselho de classe");
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno");
        }
    }
}
