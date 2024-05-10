using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPossuiFrequenciaQuery : IRequest<bool>
    {
        public ObterAulaPossuiFrequenciaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterAulaPossuiFrequenciaQueryValidator: AbstractValidator<ObterAulaPossuiFrequenciaQuery>
    {
        public ObterAulaPossuiFrequenciaQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da aula para verificar a existência de frequência registrada.");
        }
    }
}
