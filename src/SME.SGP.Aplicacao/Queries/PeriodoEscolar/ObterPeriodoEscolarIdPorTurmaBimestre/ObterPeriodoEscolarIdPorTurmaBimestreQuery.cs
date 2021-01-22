using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaBimestreQuery : IRequest<long>
    {
        public ObterPeriodoEscolarIdPorTurmaBimestreQuery(string turmaCodigo, int bimestre)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
        }

        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterPeriodoEscolarIdPorTurmaBimestreQueryValidator : AbstractValidator<ObterPeriodoEscolarIdPorTurmaBimestreQuery>
    {
        public ObterPeriodoEscolarIdPorTurmaBimestreQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para consulta do periodo escolar.");
        }
    }
}
