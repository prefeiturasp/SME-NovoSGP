using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoNotaConselhoClasseCommand : IRequest
    {
        public GerarWFAprovacaoNotaConselhoClasseCommand(long conselhoClasseNotaId,
                                                         long componenteCurricularCodigo,
                                                         double? nota,
                                                         long? conceitoId,
                                                         Dominio.Turma turma,
                                                         int? bimestre,
                                                         Usuario usuarioLogado,
                                                         string alunoCodigo,
                                                         double? notaAnterior,
                                                         long? conceitoIdAnterior)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            Nota = nota;
            ConceitoId = conceitoId;
            Turma = turma;
            Bimestre = bimestre;
            UsuarioLogado = usuarioLogado;
            AlunoCodigo = alunoCodigo;
            NotaAnterior = notaAnterior;
            ConceitoIdAnterior = conceitoIdAnterior;
        }

        public long ConselhoClasseNotaId { get; }
        public long ComponenteCurricularCodigo { get; }
        public double? Nota { get; }
        public long? ConceitoId { get; }
        public Turma Turma { get; }
        public int? Bimestre { get; }
        public Usuario UsuarioLogado { get; }
        public string AlunoCodigo { get; }
        public double? NotaAnterior { get; }
        public long? ConceitoIdAnterior { get; }
    }

    public class GerarWFAprovacaoNotaConselhoClasseCommandValidator : AbstractValidator<GerarWFAprovacaoNotaConselhoClasseCommand>
    {
        public GerarWFAprovacaoNotaConselhoClasseCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseNotaId)
                .NotEmpty()
                .WithMessage("O id da nota do conselho de classe deve ser informado para geração do workflow de aprovação");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para geração do workflow de aprovação");

            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para geração do workflow de aprovação");

            RuleFor(a => a.UsuarioLogado)
                .NotEmpty()
                .WithMessage("O usuário ser informado para geração do workflow de aprovação");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno ser informado para geração do workflow de aprovação");
        }
    }
}
