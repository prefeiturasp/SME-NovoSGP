using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaQuery : IRequest<List<AbrangenciaUeRetorno>>
    {
        public ObterFiltroRelatoriosUesPorAbrangenciaQuery(Usuario usuarioLogado, string codigoDre)
        {
            UsuarioLogado = usuarioLogado;
            CodigoDre = codigoDre;
        }

        public Usuario UsuarioLogado { get; }
        public string CodigoDre { get; }
    }

    public class ObterFiltroRelatoriosUesPorAbrangenciaQueryValidator : AbstractValidator<ObterFiltroRelatoriosUesPorAbrangenciaQuery>
    {
        public ObterFiltroRelatoriosUesPorAbrangenciaQueryValidator()
        {

            RuleFor(c => c.UsuarioLogado)
            .NotEmpty()
            .WithMessage("O usuário deve ser informado.");
        }
    }
}
