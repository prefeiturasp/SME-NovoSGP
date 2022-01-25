using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAtividadeAvaliativaQuery : IRequest<bool>
    {
        public ObterIndicativoPendenciasAtividadeAvaliativaQuery(string disciplinaId, string turmaId, int? anoLetivo = null)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo ?? DateTime.Today.Year;
        }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterIndicativoPendenciasAtividadeAvaliativaQueryValidator : AbstractValidator<ObterIndicativoPendenciasAtividadeAvaliativaQuery>
    {
        public ObterIndicativoPendenciasAtividadeAvaliativaQueryValidator()
        {
            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O código da disciplina deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de pendência na aula.");

        }
    }
}
