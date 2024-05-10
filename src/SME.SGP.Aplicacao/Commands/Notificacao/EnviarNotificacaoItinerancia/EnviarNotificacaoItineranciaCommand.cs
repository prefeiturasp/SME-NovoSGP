﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoItineranciaCommand : IRequest<long>
    {
        public EnviarNotificacaoItineranciaCommand(long itineranciaId, string titulo, string mensagem, NotificacaoCategoria notificacaoCategoria, NotificacaoTipo tipoNotificacao, Cargo[] cargos, string dreCodigo = "", string ueCodigo = "", string turmaCodigo = "")
        {
            ItineranciaId = itineranciaId;
            Titulo = titulo;
            Mensagem = mensagem;
            CategoriaNotificacao = notificacaoCategoria;
            TipoNotificacao = tipoNotificacao;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            Cargos = cargos;
        }

        public string Titulo { get; set; }
        public long ItineranciaId { get; set; }
        public string Mensagem { get; set; }
        public NotificacaoCategoria CategoriaNotificacao { get; set; }
        public NotificacaoTipo TipoNotificacao { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public Cargo[] Cargos { get; set; }
    }

    public class EnviarNotificacaoItineranciaCommandValidator : AbstractValidator<EnviarNotificacaoItineranciaCommand>
    {
        public EnviarNotificacaoItineranciaCommandValidator()
        {
            RuleFor(c => c.Titulo)
               .NotEmpty()
               .WithMessage("O título deve ser informado para gerar notificação.");

            RuleFor(c => c.Mensagem )
               .NotEmpty()
               .WithMessage("A mensagem deve ser informada para gerar notificação.");

            RuleFor(c => c.Cargos)
               .Must(a => a.Length > 0)
               .WithMessage("Os cargos devem ser informados para gerar notificação.");
        }
    }
}
