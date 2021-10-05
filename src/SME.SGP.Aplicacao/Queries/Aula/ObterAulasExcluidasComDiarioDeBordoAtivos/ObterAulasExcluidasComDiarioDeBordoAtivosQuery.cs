using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasExcluidasComDiarioDeBordoAtivosQuery : IRequest<IEnumerable<Aula>>
    {
        public ObterAulasExcluidasComDiarioDeBordoAtivosQuery(string codigoTurma, long tipoCalendarioId)
        {
            CodigoTurma = codigoTurma;
            TipoCalendarioId = tipoCalendarioId;
        }

        public string CodigoTurma { get; set; }
        public long TipoCalendarioId { get; set; }
    }

    public class ObterAulasExcluidasComDiarioDeBordoAtivosQueryValidator : AbstractValidator<ObterAulasExcluidasComDiarioDeBordoAtivosQuery>
    {
        public ObterAulasExcluidasComDiarioDeBordoAtivosQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O id do tipo de claendário deve ser informado.");            
        }
    }
}
