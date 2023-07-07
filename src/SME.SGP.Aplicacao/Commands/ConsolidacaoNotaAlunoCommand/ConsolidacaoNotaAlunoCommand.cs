using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoNotaAlunoCommand : IRequest<bool>
    {
        public string AlunoCodigo { get; }
        public long TurmaId { get; }
        public int? Bimestre { get; }
        public int AnoLetivo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }

        public long ComponenteCurricularId { get; set; }
        public bool Inativo { get; set; }
        public bool ConselhoClasse { get; set; }


        public ConsolidacaoNotaAlunoCommand(ConsolidacaoNotaAlunoDto consolidacaoNotaAlunoDto)
        {
            AlunoCodigo = consolidacaoNotaAlunoDto.AlunoCodigo;
            TurmaId = consolidacaoNotaAlunoDto.TurmaId;
            Bimestre = consolidacaoNotaAlunoDto.Bimestre;
            AnoLetivo = consolidacaoNotaAlunoDto.AnoLetivo;
            Nota = consolidacaoNotaAlunoDto.Nota;
            ConceitoId = consolidacaoNotaAlunoDto.ConceitoId;
            ComponenteCurricularId = consolidacaoNotaAlunoDto.ComponenteCurricularId;
            ConselhoClasse = consolidacaoNotaAlunoDto.ConselhoClasse;
        }
    }

    public class ConsolidacaoNotaAlunoCommandValidator : AbstractValidator<ConsolidacaoNotaAlunoCommand>
    {
        public ConsolidacaoNotaAlunoCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para gerar a consolidação das notas");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para gerar a consolidação das notas");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para gerar a consilidação das notas");
        }
    }
}
