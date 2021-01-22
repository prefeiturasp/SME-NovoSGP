using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaProfessorPorPendenciaIdQuery : IRequest<bool>
    {
        public ExistePendenciaProfessorPorPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ExistePendenciaProfessorPorPendenciaIdQueryValidator : AbstractValidator<ExistePendenciaProfessorPorPendenciaIdQuery>
    {
        public ExistePendenciaProfessorPorPendenciaIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para verificar a existência de pendência do professor.");
        }
    }
}
