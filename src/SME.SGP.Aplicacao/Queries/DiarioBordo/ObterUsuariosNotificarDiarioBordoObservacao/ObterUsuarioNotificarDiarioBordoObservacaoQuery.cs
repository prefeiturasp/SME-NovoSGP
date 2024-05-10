using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoQuery : IRequest<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
        public IEnumerable<ProfessorTitularDisciplinaEol> ProfessoresDaTurma { get; set; }

        public ObterUsuarioNotificarDiarioBordoObservacaoQuery(IEnumerable<ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            ProfessoresDaTurma = professoresDaTurma;
        }

    }

    public class ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator : AbstractValidator<ObterUsuarioNotificarDiarioBordoObservacaoQuery>
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator()
        {
            RuleForEach(x => x.ProfessoresDaTurma)
                .NotEmpty()
                .WithMessage("Todos os professores da turma devem ser informados.");
        }
    }
}