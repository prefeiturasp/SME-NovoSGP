using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorMiniaturaIdQuery : IRequest<AcompanhamentoAlunoFoto>
    {
        public ObterFotoSemestreAlunoPorMiniaturaIdQuery(long miniaturaId)
        {
            MiniaturaId = miniaturaId;
        }

        public long MiniaturaId { get; }
    }

    public class ObterFotoSemestreAlunoPorMiniaturaIdQueryValidator : AbstractValidator<ObterFotoSemestreAlunoPorMiniaturaIdQuery>
    {
        public ObterFotoSemestreAlunoPorMiniaturaIdQueryValidator()
        {
            RuleFor(a => a.MiniaturaId)
                .NotEmpty()
                .WithMessage("O id da miniatura da foto deve ser informado para consulta");
        }
    }
}
