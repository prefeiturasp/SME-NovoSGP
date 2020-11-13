using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaDataTurmaDisciplinaQuery : IRequest<AulaConsultaDto>
    {
        public ObterAulaDataTurmaDisciplinaQuery(DateTime data, string turmaId, string disciplinaId)
        {
            Data = data;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
        }

        public DateTime Data { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }

    public class ObterAulaDataTurmaDisciplinaQueryValidator : AbstractValidator<ObterAulaDataTurmaDisciplinaQuery>
    {
        public ObterAulaDataTurmaDisciplinaQueryValidator()
        {

            RuleFor(c => c.Data)
            .NotEmpty()
            .WithMessage("A data deve ser informada para consulta de aulas.");

            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("A turma deve ser informada para consulta de aulas.");

            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O componente curricular deve ser informadoa para consulta de aulas.");

        }
    }
}
