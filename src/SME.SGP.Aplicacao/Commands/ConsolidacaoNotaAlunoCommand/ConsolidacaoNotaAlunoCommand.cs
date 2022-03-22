using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoNotaAlunoCommand : IRequest<bool>
    {
        public string AlunoCodigo { get; }
        public long TurmaId { get; }
        public int Bimestre { get; }
        public int AnoLetivo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }

        public ConsolidacaoNotaAlunoCommand(ConsolidacaoNotaAlunoDto consolidacaoNotaAlunoDto)
        {
            AlunoCodigo = consolidacaoNotaAlunoDto.AlunoCodigo;
            TurmaId = consolidacaoNotaAlunoDto.TurmaId;
            Bimestre = consolidacaoNotaAlunoDto.Bimestre;
            AnoLetivo = consolidacaoNotaAlunoDto.AnoLetivo;
            Nota = consolidacaoNotaAlunoDto.Nota;
            ConceitoId = consolidacaoNotaAlunoDto.ConceitoId;
        }
    }

    public class PersistirConselhoClasseNotaCommandValidator : AbstractValidator<ConsolidacaoNotaAlunoCommand>
    {
        public PersistirConselhoClasseNotaCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado");
        }
    }
}
