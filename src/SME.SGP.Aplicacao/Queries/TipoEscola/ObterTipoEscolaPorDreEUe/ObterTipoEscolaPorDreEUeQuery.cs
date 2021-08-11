using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorDreEUeQuery : IRequest<IEnumerable<TipoEscolaDto>>
    {
        public ObterTipoEscolaPorDreEUeQuery(string dreCodigo, string ueCodigo, int[] modalidades)
        {
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Modalidades = modalidades;
        }

        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int[] Modalidades { get; set; }
    }
    public class ObterTipoEscolaPorDreEUeQueryValidator : AbstractValidator<ObterTipoEscolaPorDreEUeQuery>
    {
        public ObterTipoEscolaPorDreEUeQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()                
                .WithMessage("O código da Dre deve ser informado.");

            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");

        }
    }
}
