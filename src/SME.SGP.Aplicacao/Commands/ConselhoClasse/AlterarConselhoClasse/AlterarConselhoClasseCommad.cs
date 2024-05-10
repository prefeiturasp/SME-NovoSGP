using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarConselhoClasseCommad : IRequest<ConselhoClasseNotaRetornoDto>
    {
        public long ConselhoClasseId { get; set; } 
        public long FechamentoTurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public Turma Turma { get; set; }
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; }
        public int? Bimestre { get; set; }
        public Usuario UsuarioLogado { get; set; }

        public AlterarConselhoClasseCommad(
                long conselhoClasseId, 
                long fechamentoTurmaId, 
                string codigoAluno, 
                Turma turma, 
                ConselhoClasseNotaDto conselhoClasseNotaDto, 
                int? bimestre, 
                Usuario usuarioLogado)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            CodigoAluno = codigoAluno;
            Turma = turma;
            ConselhoClasseNotaDto = conselhoClasseNotaDto;
            Bimestre = bimestre;
            UsuarioLogado = usuarioLogado;
        }
    }

    public class AlterarConselhoClasseCommadValidator : AbstractValidator<AlterarConselhoClasseCommad>
    {
        public AlterarConselhoClasseCommadValidator()
        {
            RuleFor(c => c.FechamentoTurmaId)
               .NotNull()
               .WithMessage("O id do fechamento de turma deve ser informado para efetuar a alteração.");

            RuleFor(c => c.ConselhoClasseId)
               .NotEmpty()
               .WithMessage("O id do conselho classe deve ser informado para efetuar a alteração.");

            RuleFor(c => c.CodigoAluno)
               .NotEmpty()
               .WithMessage("O código do aluno deve ser informado para efetuar a alteração.");

            RuleFor(c => c.ConselhoClasseNotaDto)
               .NotNull()
               .WithMessage("O dto de conselho de classe nota ser informado para efetuar a alteração.");

            RuleFor(c => c.UsuarioLogado)
               .NotNull()
               .WithMessage("O usuário deve ser informado para efetuar a alteração.");
        }
    }
}
