using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorCodigoEModalidadeQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesComDrePorCodigoEModalidadeQuery(string dreCodigo, Modalidade modalidade)
        {
            DreCodigo = dreCodigo;
            Modalidade = modalidade;
        }

        public string DreCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterUesComDrePorCodigoDreQueryValidator : AbstractValidator<ObterUesComDrePorCodigoEModalidadeQuery>
    {
        public ObterUesComDrePorCodigoDreQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
            .NotEmpty()
            .WithMessage("O código da Dre deve ser informado para pesquisa de suas UEs.");
        }
    }
}
