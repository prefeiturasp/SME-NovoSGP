using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaMediaQuery : IRequest<double>
    {
        public ObterFrequenciaMediaQuery(DisciplinaDto disciplina, int anoLetivo)
        {
            Disciplina = disciplina;
            AnoLetivo = anoLetivo;
        }

        public DisciplinaDto Disciplina { get; set; } 
        public int AnoLetivo { get; set; }
    }

    public class ObterFrequenciaMediaQueryValidator : AbstractValidator<ObterFrequenciaMediaQuery>
    {
        public ObterFrequenciaMediaQueryValidator()
        {
            RuleFor(a => a.Disciplina)
                .NotEmpty()
                .WithMessage("É preciso informar a disciplina para consultar a frequência");
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É preciso informar o ano letivo para consultar a frequência");
        }
    }
}
