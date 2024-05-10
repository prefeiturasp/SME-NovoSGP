using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{

    public class ListarCartaIntencoesObservacaoQuery : IRequest<IEnumerable<CartaIntencoesObservacaoDto>>
    {
        public ListarCartaIntencoesObservacaoQuery(long turmaId, long componenteCurricularId, long usuarioLogadoId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            UsuarioLogadoId = usuarioLogadoId;
        }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long UsuarioLogadoId { get; set; }
    }

    public class ListarObservacaoCartaIntencoesQueryValidator : AbstractValidator<ListarCartaIntencoesObservacaoQuery>
    {
        public ListarObservacaoCartaIntencoesQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado.");

            RuleFor(c => c.UsuarioLogadoId)
               .NotEmpty()
               .WithMessage("O id do usuário logado deve ser informado.");
        }
    }

}
