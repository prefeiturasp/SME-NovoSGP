using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaDataPeriodoFimQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string TurmaCodigo { get; set; }
        public DateTime DataPeriodoFim { get; set; }

        public ObterAlunosPorTurmaDataPeriodoFimQuery(string turmaCodigo, DateTime dataPeriodoFim)
        {
            TurmaCodigo = turmaCodigo;
            DataPeriodoFim = dataPeriodoFim;
        }
    }

    public class ObterAlunosPorTurmaDataPeriodoFimQueryValidator : AbstractValidator<ObterAlunosPorTurmaDataPeriodoFimQuery>
    {
        public ObterAlunosPorTurmaDataPeriodoFimQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.DataPeriodoFim)
                .NotEmpty()
                .WithMessage("A data de período fim deve ser informado.");
        }
    }
}
