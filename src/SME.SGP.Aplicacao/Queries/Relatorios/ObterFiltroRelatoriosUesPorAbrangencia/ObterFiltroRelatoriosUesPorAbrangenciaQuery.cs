﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosUesPorAbrangenciaQuery : IRequest<List<AbrangenciaUeRetorno>>
    {
        public ObterFiltroRelatoriosUesPorAbrangenciaQuery(Usuario usuarioLogado, string codigoDre, bool consideraNovosTiposUE = false, bool consideraHistorico = false)
        {
            UsuarioLogado = usuarioLogado;
            CodigoDre = codigoDre;
            ConsideraNovosTiposUE = consideraNovosTiposUE;
            ConsideraHistorico= consideraHistorico;
        }

        public Usuario UsuarioLogado { get; }
        public string CodigoDre { get; }
        public bool ConsideraNovosTiposUE { get; }
        public bool ConsideraHistorico { get; set; }
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
