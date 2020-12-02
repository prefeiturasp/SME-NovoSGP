using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoBimestrePorDataFinalQuery : IRequest<IEnumerable<PeriodoFechamentoBimestre>>
    {
        public ObterPeriodosFechamentoBimestrePorDataFinalQuery(ModalidadeTipoCalendario modalidade, DateTime dataEncerramento)
        {
            Modalidade = modalidade;
            DataEncerramento = dataEncerramento;
        }

        public ModalidadeTipoCalendario Modalidade { get; set; }
        public DateTime DataEncerramento { get; set; }
    }

    public class ObterPeriodosFechamentoBimestrePorDataFinalQueryValidator : AbstractValidator<ObterPeriodosFechamentoBimestrePorDataFinalQuery>
    {
        public ObterPeriodosFechamentoBimestrePorDataFinalQueryValidator()
        {
            RuleFor(c => c.DataEncerramento)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de encerramento do período deve ser informada.");
        }
    }
}
