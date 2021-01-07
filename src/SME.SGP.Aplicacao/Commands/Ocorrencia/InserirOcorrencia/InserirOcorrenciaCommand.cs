using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommand : IRequest<AuditoriaDto>
    {
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public long TurmaId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }

        public InserirOcorrenciaCommand()
        {
            CodigosAlunos = new List<long>();
        }

        public InserirOcorrenciaCommand(DateTime dataOcorrencia, string horaOcorrencia, 
                                        string titulo, string descricao, long ocorrenciaTipoId, long turmaId, 
                                        IEnumerable<long> codigosAlunos)
        {
            DataOcorrencia = dataOcorrencia;
            HoraOcorrencia = horaOcorrencia;
            Titulo = titulo;
            Descricao = descricao;
            OcorrenciaTipoId = ocorrenciaTipoId;
            TurmaId = turmaId;
            CodigosAlunos = codigosAlunos;
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

            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma da ocorrência deve ser informada.");

            RuleFor(x => x.CodigosAlunos)
                .NotEmpty()
                .WithMessage("Os alunos envolvidos na ocorrência devem ser informados.");

            RuleForEach(x => x.CodigosAlunos)
                .NotEmpty()
                .WithMessage("Um ou mais alunos selecionados são inválidos.");
        }
    }
}
