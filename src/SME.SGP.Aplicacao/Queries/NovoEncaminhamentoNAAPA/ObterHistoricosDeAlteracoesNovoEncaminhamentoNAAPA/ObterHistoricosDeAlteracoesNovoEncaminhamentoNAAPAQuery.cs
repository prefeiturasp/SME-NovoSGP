using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPA
{
    public class ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoEscolarHistoricoAlteracoes>
    {
        public NovoEncaminhamentoNAAPASecaoDto NovoEncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao NovoEncaminhamentoNAAPASecaoExistente { get; set; }
        public TipoHistoricoAlteracoesAtendimentoNAAPA TipoHistoricoAlteracoes { get; set; }

        public ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery(
                                    NovoEncaminhamentoNAAPASecaoDto novoEncaminhamentoNAAPASecaoAlterado,
                                    EncaminhamentoNAAPASecao novoEncaminhamentoNAAPASecaoExistente,
                                    TipoHistoricoAlteracoesAtendimentoNAAPA tipoHistoricoAlteracoes)
        {
            NovoEncaminhamentoNAAPASecaoAlterado = novoEncaminhamentoNAAPASecaoAlterado;
            NovoEncaminhamentoNAAPASecaoExistente = novoEncaminhamentoNAAPASecaoExistente;
            TipoHistoricoAlteracoes = tipoHistoricoAlteracoes;
        }
    }

    public class ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery>
    {
        public ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.NovoEncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.NovoEncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção atual deve ser informado");
            RuleFor(c => c.TipoHistoricoAlteracoes).NotEmpty().WithMessage("O tipo do histórico de alteração do encaminhamentos NAAPA deve ser informado");
        }
    }
}