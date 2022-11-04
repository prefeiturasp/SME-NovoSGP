using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommand : IRequest<AuditoriaDto>
    {
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public long? TurmaId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public IEnumerable<string> CodigosServidores { get; set; }

        public InserirOcorrenciaCommand(InserirOcorrenciaDto dto)
        {
            DataOcorrencia = dto.DataOcorrencia;
            HoraOcorrencia = dto.HoraOcorrencia;
            Titulo = dto.Titulo;
            Descricao = dto.Descricao;
            OcorrenciaTipoId = dto.OcorrenciaTipoId;
            TurmaId = dto.TurmaId;
            CodigosAlunos = dto.CodigosAlunos;
            ConsideraHistorico = dto.ConsideraHistorico;
            AnoLetivo = dto.AnoLetivo;
            DreId = dto.DreId;
            UeId = dto.UeId;
            Modalidade = dto.Modalidade;
            Semestre = dto.Semestre;
            CodigosServidores = dto.CodigosServidores;
        }
    }

    public class InserirOcorrenciaCommandValidator : AbstractValidator<InserirOcorrenciaCommand>
    {
        public InserirOcorrenciaCommandValidator()
        {
            RuleFor(x => x.DataOcorrencia)
                .NotEmpty()
                .WithMessage("A data da ocorrência deve ser informada.");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A descrição da ocorrência deve ser informada.");

            RuleFor(x => x.HoraOcorrencia)
                .Matches("^([01][0-9]|2[0-3]):([0-5][0-9])$")
                .When(x => !string.IsNullOrWhiteSpace(x.HoraOcorrencia))
                .WithMessage("A hora da ocorrência informada é inválida.");

            RuleFor(x => x.OcorrenciaTipoId)
                .NotEmpty()
                .WithMessage("O tipo da ocorrência deve ser informado.");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O título da ocorrência deve ser informado.");

            RuleFor(x => x.UeId)
                .NotEmpty()
                .WithMessage("A Ue da ocorrência deve ser informada.");
        }
    }
}
