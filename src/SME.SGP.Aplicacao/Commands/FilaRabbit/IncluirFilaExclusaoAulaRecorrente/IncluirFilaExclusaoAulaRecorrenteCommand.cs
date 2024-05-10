using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExclusaoAulaRecorrenteCommand : IRequest<bool>
    {
        public IncluirFilaExclusaoAulaRecorrenteCommand(long aulaId, RecorrenciaAula recorrenciaAula, string componenteCurricularNome, Usuario usuario)
        {
            AulaId = aulaId;
            Recorrencia = recorrenciaAula;
            ComponenteCurricularNome = componenteCurricularNome;
            Usuario = usuario;
        }

        public long AulaId { get; set; }
        public RecorrenciaAula Recorrencia { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class IncluirFilaExclusaoAulaRecorrenteCommanddValidator : AbstractValidator<IncluirFilaExclusaoAulaRecorrenteCommand>
    {
        public IncluirFilaExclusaoAulaRecorrenteCommanddValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula é necessário para exclusão da mesma.");

            RuleFor(a => a.Recorrencia)
                .NotEmpty()
                .WithMessage("O tipo de recorrência é necessário para exclusão da aula.");

            RuleFor(a => a.ComponenteCurricularNome)
                .NotEmpty()
                .WithMessage("O nome do componente curricular é necessário para exclusão da aula.");

            RuleFor(a => a.Usuario)
                .NotEmpty()
                .WithMessage("O usuário é necessário para exclusão da aula.");
        }
    }
}
