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
        public InserirRegistrosFrequenciasAlunosCommand(IList<RegistroFrequenciaAlunoDto> frequencias,
            long registroFrequenciaId, long turmaId, long componenteCurricularId, long aulaId, DateTime dataAula)
        {
            Frequencias = frequencias;
            RegistroFrequenciaId = registroFrequenciaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AulaId = aulaId;
            DataAula = dataAula;
        }

        public DateTime DataAula { get; set; }
        public IList<RegistroFrequenciaAlunoDto> Frequencias { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long TurmaId { get; set; }
        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class InserirRegistrosFrequenciasAlunosCommandValidator : AbstractValidator<InserirRegistrosFrequenciasAlunosCommand>
    {
        public InserirRegistrosFrequenciasAlunosCommandValidator()
        {
            RuleFor(x => x.Frequencias)
               .NotEmpty()
               .WithMessage("As frequencias precisam ser informadas para a adição de registro frequência alunos");
            RuleFor(x => x.TurmaId)
               .NotEmpty()
               .WithMessage("A turma precisa ser informada para a adição de registro frequência alunos");
            RuleFor(x => x.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O componente curricular precisa ser informado para a adição de registro frequência alunos");
            RuleFor(x => x.RegistroFrequenciaId)
              .NotEmpty()
              .WithMessage("O registro de frequência precisa ser informado para a adição de registro frequência alunos");
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("A aula precisa ser informada para a adição de registro frequência alunos");
            RuleFor(x => x.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula precisa ser informada para a adição de registro frequência alunos");
           
        }
    }
}
