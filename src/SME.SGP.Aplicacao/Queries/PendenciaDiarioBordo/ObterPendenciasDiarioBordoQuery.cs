using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioBordoQuery : IRequest<IEnumerable<Aula>>
    {
        public long DreId { get; set; }
        public int AnoLetivo { get; set; }

        public ObterPendenciasDiarioBordoQuery(long dreId, int? anoLetivo = null)
        {
            DreId = dreId;
            AnoLetivo = anoLetivo ?? DateTime.Today.Year;
        }
    }

    public class ObterPendenciasDiarioBordoQueryValidator : AbstractValidator<ObterPendenciasDiarioBordoQuery>
    {
        public ObterPendenciasDiarioBordoQueryValidator()
        {
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da dre para obter as pendências de diário de bordo");
        }        
    }
}
