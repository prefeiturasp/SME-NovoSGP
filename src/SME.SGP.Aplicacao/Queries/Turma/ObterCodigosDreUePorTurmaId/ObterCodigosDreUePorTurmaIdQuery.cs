using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDreUePorTurmaIdQuery : IRequest<DreUeDaTurmaDto>
    {
        public ObterCodigosDreUePorTurmaIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; }
    }

    public class ObterCodigosDreUePorTurmaIdQueryValidator : AbstractValidator<ObterCodigosDreUePorTurmaIdQuery>
    {
        public ObterCodigosDreUePorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma é necessário para consulta o ");
        }
    }
}
