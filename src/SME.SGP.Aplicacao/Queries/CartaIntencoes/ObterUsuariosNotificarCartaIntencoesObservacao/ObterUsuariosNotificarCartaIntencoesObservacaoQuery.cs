using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosNotificarCartaIntencoesObservacaoQuery : IRequest<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        public IEnumerable<ProfessorTitularDisciplinaEol> ProfessoresDaTurma { get; set; }

        public ObterUsuariosNotificarCartaIntencoesObservacaoQuery(IEnumerable<ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            ProfessoresDaTurma = professoresDaTurma;
        }        
    }
    public class ObterUsuarioNotificarCartaIntencoesObservacaoQueryValidator : AbstractValidator<ObterUsuariosNotificarCartaIntencoesObservacaoQuery>
    {
        public ObterUsuarioNotificarCartaIntencoesObservacaoQueryValidator()
        {
            RuleForEach(x => x.ProfessoresDaTurma)
                .NotEmpty()
                .WithMessage("Todos os professores da turma devem ser informados.");
        }
    }
}
