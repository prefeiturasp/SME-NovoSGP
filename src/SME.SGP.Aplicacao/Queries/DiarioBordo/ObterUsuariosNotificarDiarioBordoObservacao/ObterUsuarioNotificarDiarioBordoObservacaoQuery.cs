using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoQuery : IRequest<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
        public long TurmaId { get; set; }
        public long? ObservacaoId { get; set; }

        public ObterUsuarioNotificarDiarioBordoObservacaoQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public ObterUsuarioNotificarDiarioBordoObservacaoQuery(long turmaId, long? observacaoId)
            : this(turmaId)
        {
            ObservacaoId = observacaoId;
        }
    }

    public class ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator : AbstractValidator<ObterUsuarioNotificarDiarioBordoObservacaoQuery>
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(x => x.ObservacaoId)
                .NotEmpty()
                .When(x => x.ObservacaoId != null)
                .WithMessage("A observação informada é inválida.");
        }
    }
}