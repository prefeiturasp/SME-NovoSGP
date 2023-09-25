using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaCommand : IRequest<AuditoriaDto>
    {
        public long Id { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public IEnumerable<string> CodigosServidores { get; set; }
        public long TurmaId { get; set; }

        public long UeId { get; set; }
        public AlterarOcorrenciaCommand()
        {
            CodigosAlunos = new List<long>();
            CodigosServidores = new List<string>();
        }

        public AlterarOcorrenciaCommand(AlterarOcorrenciaDto dto)
        {
            Id = dto.Id;
            DataOcorrencia = dto.DataOcorrencia;
            HoraOcorrencia = dto.HoraOcorrencia;
            Titulo = dto.Titulo;
            Descricao = dto.Descricao;
            OcorrenciaTipoId = dto.OcorrenciaTipoId;
            CodigosAlunos = dto.CodigosAlunos;
            CodigosServidores = dto.CodigosServidores;
            UeId = dto.UeId;
            TurmaId = dto.TurmaId;
        }
    }

    public class AlterarOcorrenciaCommandValidator : AbstractValidator<AlterarOcorrenciaCommand>
    {
        public AlterarOcorrenciaCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("A ocorrência deve ser informada apra alteração.");

            RuleFor(x => x.DataOcorrencia)
                .NotEmpty()
                .WithMessage("A data da ocorrência deve ser informada.");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A descrição da ocorrência deve ser informada.");

            RuleFor(x => x.HoraOcorrencia)
                .Matches(RegexConstants.EXPRESSAO_HORA)
                .When(x => !string.IsNullOrWhiteSpace(x.HoraOcorrencia))
                .WithMessage("A hora da ocorrência informada está em um formato inválido.");

            RuleFor(x => x.OcorrenciaTipoId)
                .NotEmpty()
                .WithMessage("P tipo da ocorrência deve ser informada.");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O título da ocorrência deve ser informado.");
            
        }
    }
}
