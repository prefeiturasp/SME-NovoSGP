using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoAEEPorEstudanteQuery : IRequest<bool>
    {
        public ExisteEncaminhamentoAEEPorEstudanteQuery(string codigoEstudante)
        {
            CodigoEstudante = codigoEstudante;
        }

        public string CodigoEstudante { get; }
    }

    public class ExisteEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ExisteEncaminhamentoAEEPorEstudanteQuery>
    {
        public ExisteEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");
        }
    }
}
