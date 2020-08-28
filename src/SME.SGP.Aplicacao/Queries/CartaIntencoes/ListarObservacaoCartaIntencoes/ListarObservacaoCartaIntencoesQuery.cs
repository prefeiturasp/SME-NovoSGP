using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{

    public class ListarObservacaoCartaIntencoesQuery : IRequest<IEnumerable<ListarObservacaoCartaIntencoesDto>>
    {
        public ListarObservacaoCartaIntencoesQuery(long cartaIntencoesId, long usuarioLogadoId)
        {
            CartaIntencoesId = cartaIntencoesId;
            UsuarioLogadoId = usuarioLogadoId;
        }
        public long CartaIntencoesId { get; set; }
        public long UsuarioLogadoId { get; set; }
    }

    public class ListarObservacaoCartaIntencoesQueryValidator : AbstractValidator<ListarObservacaoCartaIntencoesQuery>
    {
        public ListarObservacaoCartaIntencoesQueryValidator()
        {
            RuleFor(c => c.CartaIntencoesId)
                .NotEmpty()
                .WithMessage("O id da carta de intenções deve ser informado.");

            RuleFor(c => c.UsuarioLogadoId)
               .NotEmpty()
               .WithMessage("O id do usuário logado deve ser informado.");
        }
    }

}
