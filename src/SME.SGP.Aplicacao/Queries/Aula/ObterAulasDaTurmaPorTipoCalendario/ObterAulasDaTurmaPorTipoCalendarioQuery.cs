using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDaTurmaPorTipoCalendarioQuery : IRequest<IEnumerable<Dominio.Aula>>
    {
        public ObterAulasDaTurmaPorTipoCalendarioQuery(string turmaId, long tipoCalendarioId, string criadoPor = null)
        {
            TurmaId = turmaId;
            TipoCalendarioId = tipoCalendarioId;
            CriadoPor = criadoPor;
        }

        public string TurmaId { get; set; }
        public long TipoCalendarioId { get; set; }
        public string CriadoPor { get; set; }
    }

    public class ObterAulasDaTurmaQueryValidator : AbstractValidator<ObterAulasDaTurmaPorTipoCalendarioQuery>
    {
        public ObterAulasDaTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");
        }
    }
}
