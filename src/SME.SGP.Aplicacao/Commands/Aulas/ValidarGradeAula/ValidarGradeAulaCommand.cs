﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ValidarGradeAulaCommand: IRequest<(bool resultado, string mensagem)>
    {
        public ValidarGradeAulaCommand(string turmaCodigo,
                                       Modalidade turmaModalidade,
                                       long[] componentesCurricularesCodigo,
                                       DateTime data,
                                       string usuarioRf,
                                       int quantidade,
                                       bool ehRegencia,
                                       IEnumerable<AulaConsultaDto> aulasExistentes, bool ehGestor = false)
        {
            TurmaCodigo = turmaCodigo;
            TurmaModalidade = turmaModalidade;
            ComponenteCurricularesCodigo = componentesCurricularesCodigo;
            Data = data;
            UsuarioRf = usuarioRf;
            Quantidade = quantidade;
            EhRegencia = ehRegencia;
            AulasExistentes = aulasExistentes;
            EhGestor = ehGestor;
        }

        public string TurmaCodigo { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public long[] ComponenteCurricularesCodigo { get; set; }
        public DateTime Data { get; set; }
        public string UsuarioRf { get; set; }
        public int Quantidade { get; set; }
        public bool EhRegencia { get; set; }
        public bool EhGestor { get; set; }
        public IEnumerable<AulaConsultaDto> AulasExistentes { get; set; }
    }

    public class ValidarGradeAulaCommandValidator: AbstractValidator<ValidarGradeAulaCommand>
    {
        public ValidarGradeAulaCommandValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("A turma deve ser informada para validação de grade da matriz curricular");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("A modalidade da turma deve ser informada para validação de grade da matriz curricular");

            RuleFor(c => c.ComponenteCurricularesCodigo)
            .NotEmpty()
            .WithMessage("O componente curricular deve ser informado para validação de grade da matriz curricular");

            RuleFor(c => c.Data)
            .NotEmpty()
            .WithMessage("A data deve ser informada para validação de grade da matriz curricular");

            RuleFor(c => c.UsuarioRf)
            .NotEmpty()
            .WithMessage("O usuário deve ser informado para validação de grade da matriz curricular");

            RuleFor(c => c.AulasExistentes)
            .NotNull()
            .WithMessage("A lista de aulas existentes é necessária para validação de grade da matriz curricular");
        }
    }
}
