using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GravarConselhoClasseCommad : IRequest<ConselhoClasseNotaRetornoDto>
    {
        public FechamentoTurma FechamentoTurma { get; set; }
        public long ConselhoClasseId { get; set; }
        public string CodigoAluno { get; set; }
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; }
        public int? Bimestre { get; set; }
        public Usuario Usuario { get; set; }

        public GravarConselhoClasseCommad(
                    FechamentoTurma fechamentoTurma, 
                    long conselhoClasseId, 
                    string codigoAluno, 
                    ConselhoClasseNotaDto conselhoClasseNotaDto, 
                    int? bimestre,
                    Usuario usuario)
        {
            FechamentoTurma = fechamentoTurma;
            ConselhoClasseId = conselhoClasseId;
            CodigoAluno = codigoAluno;
            ConselhoClasseNotaDto = conselhoClasseNotaDto;
            Bimestre = bimestre;
            Usuario = usuario;
        }
    }

    public class GravarConselhoClasseCommadValidator : AbstractValidator<GravarConselhoClasseCommad>
    {
        public GravarConselhoClasseCommadValidator()
        {
            RuleFor(c => c.FechamentoTurma)
               .NotNull()
               .WithMessage("O fechamento de turma deve ser informado para efetuar a gravação.");

            RuleFor(c => c.CodigoAluno)
               .NotEmpty()
               .WithMessage("O código do aluno deve ser informado para efetuar a gravação.");

            RuleFor(c => c.ConselhoClasseNotaDto)
               .NotNull()
               .WithMessage("O dto de conselho de classe nota ser informado para efetuar a gravação.");
        }
    }
}
