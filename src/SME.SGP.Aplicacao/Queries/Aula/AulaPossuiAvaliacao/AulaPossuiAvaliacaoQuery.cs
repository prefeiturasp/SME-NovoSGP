using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AulaPossuiAvaliacaoQuery: IRequest<bool>
    {
        public AulaPossuiAvaliacaoQuery(Aula aula, string codigoRf)
        {
            Aula = aula;
            CodigoRf = codigoRf;
        }

        public Aula Aula { get; set; }
        public string CodigoRf { get; set; }
    }

    public class AulaPossuiAvaliacaoQueryValidator: AbstractValidator<AulaPossuiAvaliacaoQuery>
    {
        public AulaPossuiAvaliacaoQueryValidator()
        {
            RuleFor(a => a.Aula)
                .NotNull()
                .WithMessage("Aula deve ser informada para consulta de avaliações existentes.");

            RuleFor(a => a.CodigoRf)
                .NotEmpty()
                .WithMessage("O Código RF do professor deve ser informado para consulta de avaliações existentes.");
        }
    }
}
