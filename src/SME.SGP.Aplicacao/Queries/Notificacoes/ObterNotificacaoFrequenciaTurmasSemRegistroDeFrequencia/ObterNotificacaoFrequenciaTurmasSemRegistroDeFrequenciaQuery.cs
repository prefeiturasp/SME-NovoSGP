using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery : IRequest<IEnumerable<RegistroFrequenciaFaltanteDto>>
    {
        public TipoNotificacaoFrequencia Tipo { get; set; }

        public ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery(TipoNotificacaoFrequencia tipo)
        {
            Tipo = tipo;
        }
    }

    public class ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQueryValidator : AbstractValidator<ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery>
    {
        public ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQueryValidator()
        {
            RuleFor(x => x.Tipo)
                .NotEmpty()
                .WithMessage("O tipo deve ser informado.");
        }
    }
}