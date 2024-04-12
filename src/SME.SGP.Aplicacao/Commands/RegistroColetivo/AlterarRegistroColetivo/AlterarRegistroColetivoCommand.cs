using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroColetivoCommand : IRequest<ResultadoRegistroColetivoDto>
    {
        public AlterarRegistroColetivoCommand(RegistroColetivoDto registroColetivo)
        {
            RegistroColetivo = registroColetivo;
        }

        public RegistroColetivoDto RegistroColetivo { get; set; }
    }

    public class AlterarRegistroColetivoCommandValidator : AbstractValidator<AlterarRegistroColetivoCommand>
    {
        public AlterarRegistroColetivoCommandValidator()
        {
            RuleFor(a => a.RegistroColetivo.Id)
            .NotEmpty()
            .WithMessage("O Id do registro coletivo deve ser informado!");

            RuleFor(a => a.RegistroColetivo.DreId)
                .NotEmpty()
                .WithMessage("O Id da dre deve ser informado!");

            RuleFor(a => a.RegistroColetivo.UeIds)
                .NotEmpty()
                .Must(a => a.Any())
                .WithMessage("Deve ser informado o id da ue!");

            RuleFor(a => a.RegistroColetivo.TipoReuniaoId)
                .NotEmpty()
                .WithMessage("O Id do tipo de reunião deve ser informado!");

            RuleFor(a => a.RegistroColetivo.DataRegistro)
                .NotEmpty()
                .WithMessage("A data do registro não foi informada!");

            RuleFor(a => a.RegistroColetivo.QuantidadeParticipantes)
                .NotEmpty()
                .WithMessage("A quantidade de participantes não foi informado!");

            RuleFor(a => a.RegistroColetivo.QuantidadeEducadores)
                .NotEmpty()
                .WithMessage("A quantidade de educadores não foi informado!");

            RuleFor(a => a.RegistroColetivo.QuantidadeEducandos)
                .NotEmpty()
                .WithMessage("A quantidade de educandos não foi informado!");

            RuleFor(a => a.RegistroColetivo.QuantidadeCuidadores)
                .NotEmpty()
                .WithMessage("A quantidade de cuidadores não foi informado!");

            RuleFor(a => a.RegistroColetivo.QuantidadeCuidadores)
                .NotEmpty()
                .WithMessage("A quantidade de cuidadores não foi informado!");

            RuleFor(a => a.RegistroColetivo.Descricao)
                .NotEmpty()
                .WithMessage("A descrição da ação não foi informada!");
        }
    }
}
