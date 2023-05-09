using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public EncaminhamentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoExistente { get; set; }

        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoExistente)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPASecaoExistente = encaminhamentoNAAPASecaoExistente;
        }
    }

    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção atual deve ser informado");
        }
    }
}
