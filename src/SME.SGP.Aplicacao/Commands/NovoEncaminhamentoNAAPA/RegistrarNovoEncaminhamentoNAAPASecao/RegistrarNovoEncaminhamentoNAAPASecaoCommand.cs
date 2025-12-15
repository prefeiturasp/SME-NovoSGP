using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecao
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoCommand : IRequest<EncaminhamentoNAAPASecao>
    {
        public long? EncaminhamentoNAAPAId { get; set; }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public long? EncaminhamentoEscolarId { get; set; } = null;

        public RegistrarNovoEncaminhamentoNAAPASecaoCommand(long? encaminhamentoNaapaId, long secaoId, bool concluido, long? encaminhamentoEscolarId = null)
        {
            EncaminhamentoNAAPAId = encaminhamentoNaapaId;
            SecaoId = secaoId;
            Concluido = concluido;
            EncaminhamentoEscolarId = encaminhamentoEscolarId;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPASecaoCommandValidator : AbstractValidator<RegistrarNovoEncaminhamentoNAAPASecaoCommand>
    {
        public RegistrarNovoEncaminhamentoNAAPASecaoCommandValidator()
        {
            RuleFor(x => x.EncaminhamentoEscolarId)
                   .GreaterThan(0)
                   .WithMessage("O Id do encaminhamento deve ser informado para registar Encaminhamento NAAPA!");
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Seção do encaminhamento deve ser informada para registar Encaminhamento NAAPA!");
        }
    }
}