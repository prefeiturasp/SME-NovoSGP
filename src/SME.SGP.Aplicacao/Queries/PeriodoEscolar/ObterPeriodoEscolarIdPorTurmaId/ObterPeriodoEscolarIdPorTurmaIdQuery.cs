using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaIdQuery : IRequest<long>
    {
        public ObterPeriodoEscolarIdPorTurmaIdQuery(Turma turma, DateTime dataReferencia)
        {
            Turma = turma;
            DataReferencia = dataReferencia;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }

    public class ObterPeriodoEscolarIdPorTurmaIdQueryValidator : AbstractValidator<ObterPeriodoEscolarIdPorTurmaIdQuery>
    {
        public ObterPeriodoEscolarIdPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informado para consulta do periodo escolar.");
        }
    }
}
