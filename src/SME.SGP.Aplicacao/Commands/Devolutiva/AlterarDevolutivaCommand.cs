using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarDevolutivaCommand: IRequest<AuditoriaDto>
    {
        public Devolutiva Devolutiva { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public IEnumerable<long> DiariosBordoIds { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public string Descricao { get; set; }

        public AlterarDevolutivaCommand(Devolutiva devolutiva, long codigoComponenteCurricular, IEnumerable<long> diariosBordoIds, DateTime periodoInicio, DateTime periodoFim, string descricao)
        {
            Devolutiva = devolutiva;
            CodigoComponenteCurricular = codigoComponenteCurricular;
            DiariosBordoIds = diariosBordoIds;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            Descricao = descricao;
        }
    }

    public class AlterarDevolutivaCommandValidator: AbstractValidator<AlterarDevolutivaCommand>
    {
        public AlterarDevolutivaCommandValidator()
        {
            RuleFor(a => a.Devolutiva)
                   .NotEmpty()
                   .WithMessage("A Devolutiva deve ser informada!");

            RuleFor(a => a.CodigoComponenteCurricular)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.DiariosBordoIds)
                   .NotEmpty()
                   .WithMessage("Os diários de bordos devem ser informados!");

            RuleFor(a => a.PeriodoInicio)
                   .NotEqual(DateTime.MinValue)
                   .WithMessage("O início do período deve ser informado!");

            RuleFor(a => a.PeriodoFim)
                   .NotEqual(DateTime.MinValue)
                   .LessThanOrEqualTo(x => x.PeriodoInicio.AddDays(31))
                   .WithMessage("O fim do período deve ser informado e não pode ser mais longe que 31 dias do início!");

            RuleFor(a => a.Descricao)
                   .NotEmpty()
                   .WithMessage("A descrição deve ser informada!");
        }
    }
}
