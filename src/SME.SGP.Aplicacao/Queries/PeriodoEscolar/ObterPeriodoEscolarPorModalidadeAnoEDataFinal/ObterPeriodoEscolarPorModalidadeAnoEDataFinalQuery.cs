using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery(ModalidadeTipoCalendario modalidadeTipoCalendario, int ano, DateTime dataFim)
        {
            Modalidade = modalidadeTipoCalendario;
            Ano = ano;
            DataFim = dataFim;
        }

        public ModalidadeTipoCalendario Modalidade { get; set; }
        public int Ano { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterPeriodoEscolarPorModalidadeAnoEDataFinalQueryValidator : AbstractValidator<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery>
    {
        public ObterPeriodoEscolarPorModalidadeAnoEDataFinalQueryValidator()
        {
            RuleFor(c => c.Ano)
               .Must(a => a > 0)
               .WithMessage("O ano deve ser informado para consulta de periodo escolar.");

            RuleFor(c => c.DataFim)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de encerramento deve ser informada para consulta de periodo escolar.");
        }
    }
}
