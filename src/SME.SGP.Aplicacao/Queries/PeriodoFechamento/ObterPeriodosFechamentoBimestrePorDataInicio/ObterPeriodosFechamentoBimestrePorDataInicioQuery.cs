using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoBimestrePorDataInicioQuery : IRequest<IEnumerable<PeriodoFechamentoBimestre>>
    {
        public ObterPeriodosFechamentoBimestrePorDataInicioQuery(ModalidadeTipoCalendario modalidade, DateTime dataAbertura)
        {
            Modalidade = modalidade;
            DataAbertura = dataAbertura;
        }

        public ModalidadeTipoCalendario Modalidade { get; set; }
        public DateTime DataAbertura { get; set; }
    }

    public class ObterPeriodosFechamentoBimestrePorDataInicioQueryValidator : AbstractValidator<ObterPeriodosFechamentoBimestrePorDataInicioQuery>
    {
        public ObterPeriodosFechamentoBimestrePorDataInicioQueryValidator()
        {
            RuleFor(c => c.DataAbertura)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de abertura do período deve ser informada.");
        }
    }
}
