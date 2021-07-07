using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestrePorModalidadeQuery : IRequest<List<FiltroBimestreDto>>
    {
        public ObterBimestrePorModalidadeQuery(bool opcaoTodos, bool opcaoFinal, Modalidade modalidade)
        {
            OpcaoTodos = opcaoTodos;
            OpcaoFinal = opcaoFinal;
            Modalidade = modalidade;
        }
        public bool OpcaoTodos { get; set; }
        public bool OpcaoFinal { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterBimestrePorModalidadeQueryValidator : AbstractValidator<ObterBimestrePorModalidadeQuery>
    {
        public ObterBimestrePorModalidadeQueryValidator()
        {
            RuleFor(x => x.Modalidade)
                .NotEmpty()
                .WithMessage("A Modalidade deve ser informada para a consulta de bimestres.");
        }
    }
}
