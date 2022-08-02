using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class InserirConselhoClasseNotaCommad : IRequest<ConselhoClasseNotaRetornoDto>
    {
        public FechamentoTurma FechamentoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; }
        public int? Bimestre { get; set; }
        public Usuario UsuarioLogado { get; set; }

        public InserirConselhoClasseNotaCommad(
                    FechamentoTurma fechamentoTurma, 
                    string codigoAluno, 
                    ConselhoClasseNotaDto conselhoClasseNotaDto, 
                    int? bimestre,
                    Usuario usuarioLogado)
        {
            FechamentoTurma = fechamentoTurma;
            CodigoAluno = codigoAluno;
            ConselhoClasseNotaDto = conselhoClasseNotaDto;
            Bimestre = bimestre;
            UsuarioLogado = usuarioLogado;
        }
    }

    public class InserirConselhoClasseNotaCommadValidator : AbstractValidator<InserirConselhoClasseNotaCommad>
    {
        public InserirConselhoClasseNotaCommadValidator()
        {
            RuleFor(c => c.FechamentoTurma)
               .NotNull()
               .WithMessage("O fechamento de turma deve ser informado para efetuar a inserção.");

            RuleFor(c => c.CodigoAluno)
               .NotEmpty()
               .WithMessage("O código do aluno deve ser informado para efetuar a inserção.");

            RuleFor(c => c.ConselhoClasseNotaDto)
               .NotNull()
               .WithMessage("O dto de conselho de classe nota ser informado para efetuar a inserção.");

            RuleFor(c => c.UsuarioLogado)
               .NotNull()
               .WithMessage("O usuário deve ser informado para efetuar a inserção.");
        }
    }
}
