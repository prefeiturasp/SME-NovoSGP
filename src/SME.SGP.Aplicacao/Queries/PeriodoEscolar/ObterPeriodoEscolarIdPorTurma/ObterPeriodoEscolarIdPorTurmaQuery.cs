using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaQuery : IRequest<long>
    {
        public ObterPeriodoEscolarIdPorTurmaQuery(string turmaCodigo, DateTime dataReferencia)
        {
            TurmaCodigo = turmaCodigo;
            DataReferencia = dataReferencia;
        }

        public string TurmaCodigo { get; set; }
        public DateTime DataReferencia { get; set; }
    }

    public class ObterPeriodoEscolarIdPorTurmaQueryValidator : AbstractValidator<ObterPeriodoEscolarIdPorTurmaQuery>
    {
        public ObterPeriodoEscolarIdPorTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para consulta do periodo escolar.");
        }
    }
}
