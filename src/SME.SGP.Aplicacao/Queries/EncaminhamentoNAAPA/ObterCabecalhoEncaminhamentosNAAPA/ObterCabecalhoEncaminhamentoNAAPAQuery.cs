using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCabecalhoEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterCabecalhoEncaminhamentoNAAPAQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; set; }
    }

    public class ObterCabecalhoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterCabecalhoEncaminhamentoNAAPAQuery>
    {
        public ObterCabecalhoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId).NotEmpty().WithMessage("O Id do Encaminhamento NAAPA deve ser informado para busca");
        }
    }
}
