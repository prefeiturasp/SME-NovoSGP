﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteCommand : IRequest<bool>
    {
        public CriarAulasInfantilAutomaticamenteCommand(IEnumerable<DiaLetivoDto> diasLetivos, Turma turma, long tipoCalendarioId, IEnumerable<DateTime> diasForaDoPeriodoEscolar, IEnumerable<string> codigosDisciplinasConsideradas, DadosAulaCriadaAutomaticamenteDto dadosAulaCriadaAutomaticamente)
        {
            DiasLetivos = diasLetivos;
            Turma = turma;
            TipoCalendarioId = tipoCalendarioId;
            DiasForaDoPeriodoEscolar = diasForaDoPeriodoEscolar;
            CodigosDisciplinasConsideradas = codigosDisciplinasConsideradas;
            DadosAulaCriadaAutomaticamente = dadosAulaCriadaAutomaticamente;
        }

        public long TipoCalendarioId { get; set; }
        public IEnumerable<DiaLetivoDto> DiasLetivos { get; set; }
        public Turma Turma { get; set; }
        public IEnumerable<DateTime> DiasForaDoPeriodoEscolar { get; set; }
        public IEnumerable<string> CodigosDisciplinasConsideradas { get; set; }
        public DadosAulaCriadaAutomaticamenteDto DadosAulaCriadaAutomaticamente { get; set; }
    }

    public class CriarAulasInfantilAutomaticamenteCommandValidator : AbstractValidator<CriarAulasInfantilAutomaticamenteCommand>
    {
        public CriarAulasInfantilAutomaticamenteCommandValidator()
        {
            RuleFor(c => c.Turma)
                .NotEmpty()
                .WithMessage("As Turmas devem ser informadas.");

            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(c => c.DiasLetivos)
              .NotEmpty()
              .WithMessage("Os dias letivos e não letivos devem ser informados.");

            RuleFor(c => c.CodigosDisciplinasConsideradas)
              .NotEmpty()
              .WithMessage("Os códigos das disciplinas consideradas devem ser informados.");
        }
    }

}
