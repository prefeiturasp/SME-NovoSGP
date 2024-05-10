using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorCodigoUEQuery : IRequest<long>
    {
        public ObterTipoCalendarioIdPorCodigoUEQuery(string ueCodigo, int anoLetivo, int semestre)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public string UeCodigo { get; }
        public int AnoLetivo { get; }
        public int Semestre { get; }
    }

    public class ObterTipoCalendarioIdPorCodigoUEQueryValidator : AbstractValidator<ObterTipoCalendarioIdPorCodigoUEQuery>
    {
        public ObterTipoCalendarioIdPorCodigoUEQueryValidator()
        {
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta do Tipo de Calendário");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta do TIpo de Calendário");
        }
    }
}
