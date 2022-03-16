using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaPorDataEAulaIdQuery : IRequest<IEnumerable<RegistroFrequencia>>
    {
        public ObterRegistroFrequenciaPorDataEAulaIdQuery(string disciplinaId, string turmaId, DateTime dataInicio, DateTime dataFim)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterRegistroFrequenciaPorDataEAulaIdQueryValidator : AbstractValidator<ObterRegistroFrequenciaPorDataEAulaIdQuery>
    {
        public ObterRegistroFrequenciaPorDataEAulaIdQueryValidator()
        {
            RuleFor(x => x.DisciplinaId)
             .NotEmpty()
             .WithMessage("O id da aula deve ser informado.");

            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");

            RuleFor(x => x.DataInicio)
                .NotEmpty()
                .WithMessage("A Data Inicio da Aula deve ser informado.");

            RuleFor(x => x.DataFim)
                .NotEmpty()
                .WithMessage("A Data Fim da Aula deve ser informado.");
        }
    }
}
