using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPA
{
    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand : IRequest<long>
    {
        public EncaminhamentoEscolar encaminhamentoEscolar { get; set; }
        public SituacaoNAAPA SituacaoAlterada { get; set; }

        public RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand(EncaminhamentoEscolar encaminhamentoEscolar, SituacaoNAAPA situacaoAlterada)
        {
            this.encaminhamentoEscolar = encaminhamentoEscolar;
            this.SituacaoAlterada = situacaoAlterada;
        }
    }

    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand>
    {
        public RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.encaminhamentoEscolar).NotEmpty().WithMessage("O encaminhamento NAAPA deve ser informado");
            RuleFor(c => c.SituacaoAlterada).NotEmpty().WithMessage("A situação deve ser informada");
        }
    }
}