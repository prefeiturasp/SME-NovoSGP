using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery : IRequest<IEnumerable<PeriodoFechamentoBimestre>>
    {
        public ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery(int modalidadeTipoCalendario, DateTime dataFechamento)
        {
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
            DataFechamento = dataFechamento;
        }

        
        public int ModalidadeTipoCalendario { get; set; }
        public DateTime DataFechamento { get; set; }
    }


    public class ObterPeriodoEscolarePorModalidadeDataFechamentoQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery>
    {
        public ObterPeriodoEscolarePorModalidadeDataFechamentoQueryValidator()
        {
            RuleFor(c => c.ModalidadeTipoCalendario)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada");
            RuleFor(c => c.DataFechamento)
                .NotEmpty()
                .WithMessage("A Data de Fechamento deve ser informada");
        }
    }
}
