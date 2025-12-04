using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery : IRequest<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>>
    {
        public ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery(long encaminhamentoNAAPId)
        {
            EncaminhamentoNAAPId = encaminhamentoNAAPId;
        }

        public long EncaminhamentoNAAPId { get; set; }
    }

    public class ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQueryValidator()
        {
            RuleFor(x => x.EncaminhamentoNAAPId).GreaterThan(0).WithMessage("Informe o Id do atendimento NAAPA para consultar as Observações");
        }
    }
}
