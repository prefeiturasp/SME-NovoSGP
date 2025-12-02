using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPA
{
    public class ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery : IRequest<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public NovoEncaminhamentoNAAPASecaoDto NovoEncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao NovoEncaminhamentoNAAPASecaoExistente { get; set; }
        public TipoHistoricoAlteracoesEncaminhamentoNAAPA TipoHistoricoAlteracoes { get; set; }

        public ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAQuery(
                                    NovoEncaminhamentoNAAPASecaoDto novoEncaminhamentoNAAPASecaoAlterado,
                                    EncaminhamentoNAAPASecao novoEncaminhamentoNAAPASecaoExistente,
                                    TipoHistoricoAlteracoesEncaminhamentoNAAPA tipoHistoricoAlteracoes)
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