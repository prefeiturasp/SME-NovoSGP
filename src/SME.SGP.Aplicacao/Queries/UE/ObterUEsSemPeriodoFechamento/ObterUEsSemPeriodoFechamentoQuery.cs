using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsSemPeriodoFechamentoQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUEsSemPeriodoFechamentoQuery(long periodoEscolarId, int ano, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            PeriodoEscolarId = periodoEscolarId;
            Ano = ano;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public long PeriodoEscolarId { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public int Ano { get; set; }
    }

    public class ObterUEsSemPeriodoFechamentoQueryValidator : AbstractValidator<ObterUEsSemPeriodoFechamentoQuery>
    {
        public ObterUEsSemPeriodoFechamentoQueryValidator()
        {
            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O id do periodo escolar deve ser informado para buscar de UEs sem periodo de fechamento.");

            RuleFor(c => c.Ano)
               .Must(a => a > 0)
               .WithMessage("O ano deve ser informado para buscar de UEs sem periodo de fechamento.");
        }
    }
}
