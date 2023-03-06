using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SolicitaRelatorioDevolutivasCommand : IRequest<Guid>
    {
        public SolicitaRelatorioDevolutivasCommand(long devolutivaId, string usuarioNome, string usuarioRF, long ueId, long turmaId)
        {
            DevolutivaId = devolutivaId;
            UsuarioNome = usuarioNome;
            UsuarioRF = usuarioRF;
            UeId = ueId;
            TurmaId = turmaId;
        }

        public long DevolutivaId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }

        public class SolicitaRelatorioDevolutivasCommandValidator : AbstractValidator<SolicitaRelatorioDevolutivasCommand>
        {
            public SolicitaRelatorioDevolutivasCommandValidator()
            {
                RuleFor(c => c.DevolutivaId)
                   .NotEmpty()
                   .WithMessage("O id da devolutiva deve ser informado para solicitação do relatório.");

                RuleFor(c => c.UsuarioNome)
                  .NotEmpty()
                  .WithMessage("O nome do usuário deve ser informado para solicitação do relatório.");

                RuleFor(c => c.UsuarioRF)
                  .NotEmpty()
                  .WithMessage("O Rf deve ser informado para solicitação do relatório.");

                RuleFor(c => c.UeId)
                  .NotEmpty()
                  .WithMessage("O id da ue deve ser informado para solicitação do relatório.");

                RuleFor(c => c.DevolutivaId)
                  .NotEmpty()
                  .WithMessage("O id da devolutiva deve ser informado para solicitação do relatório.");


            }
        }
    }
}
