using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorIdQuery : IRequest<AcompanhamentoAlunoFoto>
    {
        public ObterFotoSemestreAlunoPorIdQuery(long acompanhamentoAlunoFotoId)
        {
            AcompanhamentoAlunoFotoId = acompanhamentoAlunoFotoId;
        }

        public long AcompanhamentoAlunoFotoId { get; }
    }

    public class ObterFotoSemestreAlunoPorIdQueryValidator : AbstractValidator<ObterFotoSemestreAlunoPorIdQuery>
    {
        public ObterFotoSemestreAlunoPorIdQueryValidator()
        {
            RuleFor(a => a.AcompanhamentoAlunoFotoId)
                .NotEmpty()
                .WithMessage("O id da foto de acompanhamento do aluno no semestre deve ser informado para consulta");
        }
    }
}
