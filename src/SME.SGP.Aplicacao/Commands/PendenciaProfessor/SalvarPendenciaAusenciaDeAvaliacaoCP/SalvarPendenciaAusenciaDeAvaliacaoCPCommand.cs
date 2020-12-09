using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAusenciaDeAvaliacaoCPCommand : IRequest<bool>
    {
        public SalvarPendenciaAusenciaDeAvaliacaoCPCommand(long pendenciaId, long turmaId, long periodoEscolarId, string ueCodigo, IEnumerable<(long componenteCurricularId, string professorRf)> pendenciasProfessores)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
            PendenciasProfessores = pendenciasProfessores;
            PeriodoEscolarId = periodoEscolarId;
            UeCodigo = ueCodigo;
        }

        public long PendenciaId { get; set; }
        public long TurmaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public string UeCodigo { get; set; }
        public IEnumerable<(long componenteCurricularId, string professorRf)> PendenciasProfessores { get; set; }
    }

    public class SalvarPendenciaAusenciaDeAvaliacaoCPCommandValidator : AbstractValidator<SalvarPendenciaAusenciaDeAvaliacaoCPCommand>
    {
        public SalvarPendenciaAusenciaDeAvaliacaoCPCommandValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para geração da pendência do CP.");

            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para geração da pendência do CP.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O periodo escolar deve ser informado para geração da pendência do CP.");

            RuleFor(c => c.UeCodigo)
               .NotEmpty()
               .WithMessage("O código da UE deve ser informado para geração da pendência do CP.");

            RuleFor(c => c.PendenciasProfessores)
               .NotEmpty()
               .WithMessage("A lista de professores e componentes deve ser informada para geração da pendência do CP.");
        }
    }
}
