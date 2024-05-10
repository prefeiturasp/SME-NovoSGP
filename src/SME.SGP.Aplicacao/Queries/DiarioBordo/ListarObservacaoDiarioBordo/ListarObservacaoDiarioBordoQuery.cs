using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoDiarioBordoQuery:IRequest<IEnumerable<ListarObservacaoDiarioBordoDto>>
    {
        public ListarObservacaoDiarioBordoQuery(long diarioBordoId, long usuarioLogadoId)
        {
            DiarioBordoId = diarioBordoId;
            UsuarioLogadoId = usuarioLogadoId;
        }
        public long DiarioBordoId { get; set; }
        public long UsuarioLogadoId { get; set; }
    }

    public class ListarObservacaoDiarioBordoQueryValidator : AbstractValidator<ListarObservacaoDiarioBordoQuery>
    {
        public ListarObservacaoDiarioBordoQueryValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O id do diário de bordo deve ser informado.");

            RuleFor(c => c.UsuarioLogadoId)
               .NotEmpty()
               .WithMessage("O id do usuário logado deve ser informado.");
        }
    }
}
