using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPorConselhoClasseAlunoCodigoQuery : IRequest<ConselhoClasseAluno>
    {
        public string AlunoCodigo { get; set; }
        public long ConselhoClasseId { get; set; }

        public ObterPorConselhoClasseAlunoCodigoQuery(long conselhoClasseId, string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
            ConselhoClasseId = conselhoClasseId;
        }

        public class ObterPorConselhoClasseAlunoCodigoQueryValidator : AbstractValidator<ObterPorConselhoClasseAlunoCodigoQuery>
        {
            public ObterPorConselhoClasseAlunoCodigoQueryValidator()
            {
                RuleFor(c => c.AlunoCodigo)
                    .NotEmpty()
                    .WithMessage("O código do aluno deve ser informado.");

                RuleFor(c => c.ConselhoClasseId)
                    .NotEmpty()
                    .WithMessage("O código do aluno deve ser informado.");
            }
        }
    }
}
