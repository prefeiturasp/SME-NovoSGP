using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoAtualECodigoUeQuery : IRequest<IEnumerable<TurmaNaoHistoricaDto>>
    {
        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }

        public ObterTurmasPorAnoAtualECodigoUeQuery(string codigoUe, int anoLetivo)
        {
            UeCodigo = codigoUe;
            AnoLetivo = anoLetivo;
        }
    }
    public class ObterTurmasPorAnoAtualECodigoUeQueryValidator : AbstractValidator<ObterTurmasPorAnoAtualECodigoUeQuery>
    {
        public ObterTurmasPorAnoAtualECodigoUeQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
               .NotEmpty()
               .WithMessage("O código da UE deve ser informado para consulta das turmas.");

            RuleFor(c => c.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado para consulta das turmas.");
        }
    }
}
