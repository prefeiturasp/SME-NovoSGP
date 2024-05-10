using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery : IRequest<IEnumerable<TipoDocumentoDto>>
    {
        public ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery(string[] perfil)
        {
            Perfil = perfil;
        }

        public string[] Perfil { get; set; }
    }

    public class ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQueryValidator : AbstractValidator<ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery>
    {
        public ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQueryValidator()
        {
            RuleFor(c => c.Perfil)
            .NotEmpty()
            .WithMessage("O perfil do usuário deve ser informado");

        }
    }
}
