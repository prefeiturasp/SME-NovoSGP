using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesAtendimentoNAAPAQuery : IRequest<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public AtendimentoNAAPASecaoDto EncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoExistente { get; set; }
        public TipoHistoricoAlteracoesAtendimentoNAAPA TipoHistoricoAlteracoes { get; set; }

        public ObterHistoricosDeAlteracoesAtendimentoNAAPAQuery(
                                    AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, 
                                    EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoExistente,
                                    TipoHistoricoAlteracoesAtendimentoNAAPA tipoHistoricoAlteracoes)
        {
            EncaminhamentoNAAPASecaoAlterado = encaminhamentoNAAPASecaoAlterado;
            EncaminhamentoNAAPASecaoExistente = encaminhamentoNAAPASecaoExistente;
            TipoHistoricoAlteracoes = tipoHistoricoAlteracoes;
        }
    }

    public class ObterHistoricosDeAlteracoesAtendimentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesAtendimentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O atendimento NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.EncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O atendimento NAAPA da seção atual deve ser informado");
            RuleFor(c => c.TipoHistoricoAlteracoes).NotEmpty().WithMessage("O tipo do histórico de alteração do atendimento NAAPA deve ser informado");
        }
    }
}
