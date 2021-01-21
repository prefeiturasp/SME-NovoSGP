using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoQuery : IRequest<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
        public Turma Turma { get; set; }
        public IEnumerable<ProfessorTitularDisciplinaEol> ProfessoresDaTurma { get; set; }
        public long? ObservacaoId { get; set; }

        public ObterUsuarioNotificarDiarioBordoObservacaoQuery(Turma turma, IEnumerable<ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            Turma = turma;
            ProfessoresDaTurma = professoresDaTurma;
        }

        public ObterUsuarioNotificarDiarioBordoObservacaoQuery(Turma turma, IEnumerable<ProfessorTitularDisciplinaEol> professoresDaTurma, long? observacaoId)
            : this(turma, professoresDaTurma)
        {
            ObservacaoId = observacaoId;
        }
    }

    public class ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator : AbstractValidator<ObterUsuarioNotificarDiarioBordoObservacaoQuery>
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleForEach(x => x.ProfessoresDaTurma)
                .NotEmpty()
                .WithMessage("Todos os professores da turma devem ser informados.");

            RuleFor(x => x.ObservacaoId)
                .NotEmpty()
                .When(x => x.ObservacaoId != null)
                .WithMessage("A observação informada é inválida.");
        }
    }
}