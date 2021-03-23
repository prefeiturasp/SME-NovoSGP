using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

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
