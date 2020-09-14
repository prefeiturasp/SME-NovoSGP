using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaCommand: IRequest<AuditoriaDto>
    {
        public long CodigoComponenteCurricular { get; set; }
        public long TurmaId { get; set; }
        public IEnumerable<long> DiariosBordoIds { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public string Descricao { get; set; }

        public InserirDevolutivaCommand(long codigoComponenteCurricular, IEnumerable<long> diariosBordoIds, DateTime periodoInicio, DateTime periodoFim, string descricao, long turmaId)
        {
            CodigoComponenteCurricular = codigoComponenteCurricular;
            DiariosBordoIds = diariosBordoIds;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            Descricao = descricao;
            TurmaId = turmaId;
        }
    }

    public class InserirDevolutivaCommandValidator: AbstractValidator<InserirDevolutivaCommand>
    {
        public InserirDevolutivaCommandValidator()
        {
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
