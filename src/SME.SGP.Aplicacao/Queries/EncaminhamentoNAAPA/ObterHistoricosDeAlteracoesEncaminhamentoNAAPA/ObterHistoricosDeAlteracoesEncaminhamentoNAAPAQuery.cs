using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public AtendimentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoExistente { get; set; }
        public TipoHistoricoAlteracoesEncaminhamentoNAAPA TipoHistoricoAlteracoes { get; set; }

        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(
                                    AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, 
                                    EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoExistente,
                                    TipoHistoricoAlteracoesEncaminhamentoNAAPA tipoHistoricoAlteracoes)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPASecaoExistente = encaminhamentoNAAPASecaoExistente;
            TipoHistoricoAlteracoes = tipoHistoricoAlteracoes;
        }
    }

    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção atual deve ser informado");
            RuleFor(c => c.TipoHistoricoAlteracoes).NotEmpty().WithMessage("O tipo do histórico de alteração do encaminhamentos NAAPA deve ser informado");
        }
    }
}
