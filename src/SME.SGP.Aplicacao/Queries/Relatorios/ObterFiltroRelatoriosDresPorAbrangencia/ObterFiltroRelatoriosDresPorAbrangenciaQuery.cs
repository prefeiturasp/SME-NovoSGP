using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Relatorios.ObterFiltroRelatoriosDresPorAbrangencia
{
    public class ObterFiltroRelatoriosDresPorAbrangenciaQuery : IRequest<IEnumerable<AbrangenciaDreRetorno>>
    {
        public ObterFiltroRelatoriosDresPorAbrangenciaQuery(Usuario usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
        }

        public Usuario UsuarioLogado { get; set; }
    }

    public class ObterFiltroRelatoriosDresPorAbrangenciaQueryValidator : AbstractValidator<ObterFiltroRelatoriosDresPorAbrangenciaQuery>
    {
        public ObterFiltroRelatoriosDresPorAbrangenciaQueryValidator()
        {

            RuleFor(c => c.UsuarioLogado)
            .NotEmpty()
            .WithMessage("O usuário deve ser informado.");
        }
    }
}
