using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarAtualPorTurmaQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarAtualPorTurmaQuery(Turma turma, DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
            Turma = turma;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }

    public class ObterPeriodoEscolarAtualPorTurmaQueryValidator : AbstractValidator<ObterPeriodoEscolarAtualPorTurmaQuery>
    {
        public ObterPeriodoEscolarAtualPorTurmaQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta do período escolar.");

            RuleFor(x => x.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada para consulta do período escolar.");
        }
    }
}