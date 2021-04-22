using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarPlanoAEEObservacaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarPlanoAEEObservacaoCommand(long id, long planoAEEId, string observacao, IEnumerable<long> usuarios)
        {
            Id = id;
            PlanoAEEId = planoAEEId;
            Observacao = observacao;
            Usuarios = usuarios;
        }

        public long Id { get; }
        public long PlanoAEEId { get; }
        public string Observacao { get; }
        public IEnumerable<long> Usuarios { get; }

        public bool PossuiUsuarios { get => Usuarios != null && Usuarios.Any(); }
    }

    public class AlterarPlanoAEEObservacaoCommandValidator : AbstractValidator<AlterarPlanoAEEObservacaoCommand>
    {
        public AlterarPlanoAEEObservacaoCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O Id da observação deve ser informado para alteração");

            RuleFor(a => a.Observacao)
                .NotEmpty()
                .WithMessage("A observação deve ser informada para alteração");
        }
    }
}
