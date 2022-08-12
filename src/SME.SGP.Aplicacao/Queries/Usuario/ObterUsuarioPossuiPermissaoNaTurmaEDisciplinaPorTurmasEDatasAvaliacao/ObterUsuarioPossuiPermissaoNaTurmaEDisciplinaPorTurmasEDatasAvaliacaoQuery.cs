using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery : IRequest<IEnumerable<UsuarioPossuiAtribuicaoEolDto>>
    {

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery(long disciplinaId, IEnumerable<string> turmasIds, Usuario usuarioLogado)
        {
            DisciplinaId = disciplinaId;
            TurmasIds = turmasIds;
            UsuarioLogado = usuarioLogado;
        }
        public long DisciplinaId { get; set; }
        public IEnumerable<string> TurmasIds { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }

    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryValidator : AbstractValidator<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery>
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryValidator()
        {

            RuleFor(c => c.DisciplinaId)
                .NotEmpty()
                .WithMessage("O código da Disciplina deve ser informado.");

            RuleFor(c => c.UsuarioLogado)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.TurmasIds)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
