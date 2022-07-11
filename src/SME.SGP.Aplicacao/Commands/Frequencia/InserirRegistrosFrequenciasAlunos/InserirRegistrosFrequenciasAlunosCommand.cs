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
        public InserirRegistrosFrequenciasAlunosCommand(IList<RegistroFrequenciaAlunoDto> frequencias, long registroFrequenciaId, long turmaId, long componenteCurricularId,long aulaId)
        {
            Frequencias = frequencias;
            RegistroFrequenciaId = registroFrequenciaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AulaId = aulaId;
        }

        public IList<RegistroFrequenciaAlunoDto> Frequencias { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AulaId { get; set; }
    }

    public class InserirRegistrosFrequenciasAlunosCommandValidator : AbstractValidator<InserirRegistrosFrequenciasAlunosCommand>
    {
        public InserirRegistrosFrequenciasAlunosCommandValidator()
        {
            RuleFor(x => x.Frequencias)
                .NotEmpty()
                .WithMessage("As frequencias precisam ser informadas para criar o Registro de Frequência do Aluno");
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma precisa ser informada para criar o Registro de Frequência do Aluno");
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("A Aula precisa ser informada para criar o Registro de Frequência do Aluno");
            RuleFor(x => x.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular precisa ser informado para criar o Registro de Frequência do Aluno");
            RuleFor(x => x.RegistroFrequenciaId)
                .NotEmpty()
                .WithMessage("O código da frequência precisa ser informado para criar o Registro de Frequência do Aluno");
        }
    }
}
