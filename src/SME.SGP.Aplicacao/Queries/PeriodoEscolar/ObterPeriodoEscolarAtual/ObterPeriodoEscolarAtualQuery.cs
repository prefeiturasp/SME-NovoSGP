using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarAtualQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarAtualQuery(long turmaId, DateTime dataReferencia)
        {
            TurmaId = turmaId;
            DataReferencia = dataReferencia;
        }

        public long TurmaId { get; }
        public DateTime DataReferencia { get; }
    }

    public class ObterPeriodoEscolarAtualQueryValidator : AbstractValidator<ObterPeriodoEscolarAtualQuery>
    {
        public ObterPeriodoEscolarAtualQueryValidator()
        {
            RuleFor(a => a.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta do periodo escolar.");

            RuleFor(x => x.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada para consulta do período escolar.");
        }
    }
}