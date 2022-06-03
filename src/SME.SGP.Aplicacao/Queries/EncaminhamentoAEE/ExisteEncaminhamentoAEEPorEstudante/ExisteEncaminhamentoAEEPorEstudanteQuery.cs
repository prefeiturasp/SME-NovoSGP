using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoAEEPorEstudanteQuery : IRequest<bool>
    {
        public ExisteEncaminhamentoAEEPorEstudanteQuery(string codigoEstudante, long ueId)
        {
            CodigoEstudante = codigoEstudante;
            UeId = ueId;
        }

        public string CodigoEstudante { get; }
        public long UeId { get; }
    }

    public class ExisteEncaminhamentoAEEPorEstudanteQueryValidator : AbstractValidator<ExisteEncaminhamentoAEEPorEstudanteQuery>
    {
        public ExisteEncaminhamentoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Encaminhamento AEE");
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O id da ue deve ser informado para consulta de seu Encaminhamento AEE");
        }
    }
}
