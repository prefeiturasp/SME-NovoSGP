using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistrosFrequenciasAlunosCommand : IRequest<bool>
    {
        public InserirRegistrosFrequenciasAlunosCommand(IList<RegistroFrequenciaAlunoDto> frequencias, long registroFrequenciaId, long turmaId, long componenteCurricularId)
        {
            Frequencias = frequencias;
            RegistroFrequenciaId = registroFrequenciaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public IList<RegistroFrequenciaAlunoDto> Frequencias { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class InserirRegistrosFrequenciasAlunosCommandValidator : AbstractValidator<InserirRegistrosFrequenciasAlunosCommand>
    {
        public InserirRegistrosFrequenciasAlunosCommandValidator()
        {
            RuleFor(x => x.Frequencias)
               .NotEmpty()
               .WithMessage("As frequencias precisam ser informadas");
            RuleFor(x => x.TurmaId)
               .NotEmpty()
               .WithMessage("A turma precisa ser informada");
            RuleFor(x => x.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O componente curricular precisa ser informado");
            RuleFor(x => x.RegistroFrequenciaId)
              .NotEmpty()
              .WithMessage("O registro de frequência precisa ser informado");
        }
    }
}
