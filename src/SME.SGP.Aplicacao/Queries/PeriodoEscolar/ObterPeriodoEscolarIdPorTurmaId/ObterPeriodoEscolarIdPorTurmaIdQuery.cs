using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaIdQuery : IRequest<long>
    {
        public ObterPeriodoEscolarIdPorTurmaIdQuery(long turmaId, DateTime dataReferencia)
        {
            TurmaId = turmaId;
            DataReferencia = dataReferencia;
        }

        public long TurmaId { get; set; }
        public DateTime DataReferencia { get; set; }
    }

    public class ObterPeriodoEscolarIdPorTurmaIdQueryValidator : AbstractValidator<ObterPeriodoEscolarIdPorTurmaIdQuery>
    {
        public ObterPeriodoEscolarIdPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta do periodo escolar.");
        }
    }
}
