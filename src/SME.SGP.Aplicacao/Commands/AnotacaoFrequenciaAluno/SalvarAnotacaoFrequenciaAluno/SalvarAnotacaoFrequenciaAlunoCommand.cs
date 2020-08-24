using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFrequenciaAlunoCommand : IRequest<AuditoriaDto>
    {
        public SalvarAnotacaoFrequenciaAlunoCommand(long? motivoAusenciaId, long aulaId, string anotacao, string codigoAluno, bool ehInfantil)
        {
            MotivoAusenciaId = motivoAusenciaId;
            AulaId = aulaId;
            Anotacao = anotacao;
            CodigoAluno = codigoAluno;
            EhInfantil = ehInfantil;
        }

        public SalvarAnotacaoFrequenciaAlunoCommand(SalvarAnotacaoFrequenciaAlunoDto dto)
        {
            MotivoAusenciaId = dto.MotivoAusenciaId;
            AulaId = dto.AulaId;
            Anotacao = dto.Anotacao;
            CodigoAluno = dto.CodigoAluno;
            EhInfantil = dto.EhInfantil;
        }

        public long? MotivoAusenciaId { get; set; }
        public long AulaId { get; set; }
        public string Anotacao { get; set; }
        public string CodigoAluno { get; set; }
        public bool EhInfantil { get; set; }
    }

    public class SalvarAnotacaoFrequenciaAlunoCommandValidator : AbstractValidator<SalvarAnotacaoFrequenciaAlunoCommand>
    {
        public SalvarAnotacaoFrequenciaAlunoCommandValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado.");

            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage(c => $"O código {(c.EhInfantil ? "da criança" : "do aluno")} deve ser informado.");

            RuleFor(c => c.MotivoAusenciaId)
                .NotEmpty()
                .WithMessage("A anotação ou o motivo da ausência devem ser informados.")
                .When(c => string.IsNullOrWhiteSpace(c.Anotacao));
        }
    }
}
