using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand : IRequest<bool>
    {
        public ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand(long aulaId, long componenteCurricularId)
        {
            AulaId = aulaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ExcluirPendenciaDiarioBordoCommandValidator : AbstractValidator<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand>
    {
        public ExcluirPendenciaDiarioBordoCommandValidator()
        {
            RuleFor(x => x.AulaId)
                    .NotEmpty()
                    .WithMessage("O Id da aula deve ser informado para executar a exclusão da pendência de diário de bordo.");

            RuleFor(x => x.ComponenteCurricularId)
                    .NotEmpty()
                    .WithMessage("O Id do componente curricular deve ser informado para executar a exclusão da pendência de diário de bordo.");
        }
    }
}
