﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPossuiPlanoAulaQuery: IRequest<bool>
    {
        public ObterAulaPossuiPlanoAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class AulaPossuiPlanoAulaQueryValidator: AbstractValidator<ObterAulaPossuiPlanoAulaQuery>
    {
        public AulaPossuiPlanoAulaQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado para verificar a existência de plano de aula registrado.");
        }
    }
}
