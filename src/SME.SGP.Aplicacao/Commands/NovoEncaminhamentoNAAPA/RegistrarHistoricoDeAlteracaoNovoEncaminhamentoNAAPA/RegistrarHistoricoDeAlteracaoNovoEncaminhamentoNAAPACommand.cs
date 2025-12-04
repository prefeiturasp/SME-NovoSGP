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

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPA
{
    public class RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand : IRequest<long>
    {
        public NovoEncaminhamentoNAAPASecaoDto NovoEncaminhamentoNAAPASecaoAlterado { get; set; }
        public EncaminhamentoNAAPASecao NovoEncaminhamentoNAAPASecaoExistente { get; set; }
        public TipoHistoricoAlteracoesAtendimentoNAAPA TipoHistoricoAlteracoes { get; set; }

        public RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand(
                                NovoEncaminhamentoNAAPASecaoDto novoEncaminhamentoNAAPASecaoAlterado,
                                EncaminhamentoNAAPASecao novoEncaminhamentoNAAPASecaoExistente,
                                TipoHistoricoAlteracoesAtendimentoNAAPA tipoHistoricoAlteracoes)
        {
            NovoEncaminhamentoNAAPASecaoAlterado = novoEncaminhamentoNAAPASecaoAlterado;
            NovoEncaminhamentoNAAPASecaoExistente = novoEncaminhamentoNAAPASecaoExistente;
            TipoHistoricoAlteracoes = tipoHistoricoAlteracoes;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand>
    {
        public RegistrarNovoEncaminhamentoNAAPAHistoricoDeAlteracaoCommandValidator()
        {
            RuleFor(c => c.NovoEncaminhamentoNAAPASecaoAlterado).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção alterado deve ser informado");
            RuleFor(c => c.NovoEncaminhamentoNAAPASecaoExistente).NotEmpty().WithMessage("O encaminhamentos NAAPA da seção atual deve ser informado");
            RuleFor(c => c.TipoHistoricoAlteracoes).NotEmpty().WithMessage("O tipo do histórico de alteração do encaminhamentos NAAPA deve ser informado");
        }
    }
}