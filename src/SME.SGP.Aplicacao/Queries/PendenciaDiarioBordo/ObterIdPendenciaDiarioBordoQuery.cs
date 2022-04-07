using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PendenciaDiarioBordo
{
    public class ObterIdPendenciaDiarioBordoQuery : IRequest<bool>
    {
        public ObterIdPendenciaDiarioBordoQuery(long pendenciaId)
        {
            PendenciaID = pendenciaId;
        }
        public long PendenciaID { get; set; }
    }
    public class ObterIdPendenciaDiarioBordoQueryValidator : AbstractValidator<ObterIdPendenciaDiarioBordoQuery>
    {
        public ObterIdPendenciaDiarioBordoQueryValidator()
        {
            RuleFor(c => c.PendenciaID)
               .NotEmpty()
               .WithMessage("O Id deve ser informado para obter a pendência.");

        }
    }
}

