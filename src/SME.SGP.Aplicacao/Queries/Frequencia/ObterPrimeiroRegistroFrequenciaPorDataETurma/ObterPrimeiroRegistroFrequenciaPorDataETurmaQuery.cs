using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.Frequencia;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery : IRequest<ComponenteCurricularSugeridoDto>
    {
        public ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery(string turmaId, DateTime dataAula)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
        }

        public string TurmaId { get; set; }
        public DateTime DataAula { get; set; }
    }

    public class ObterPrimeiroRegistroFrequenciaPorDataETurmaQueryValidator : AbstractValidator<ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery>
    {
        public ObterPrimeiroRegistroFrequenciaPorDataETurmaQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("O Id da Turma deve ser informado.");

            RuleFor(x => x.DataAula)
                .NotEmpty()
                .WithMessage("A Data da Aula deve ser informada.");
        }
    }
}
