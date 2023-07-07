using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand : IRequest<bool>
    {
        public SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand(string alunoCodigo, 
                                                                        int bimestre, 
                                                                        long turmaId, 
                                                                        long componenteCurricularId, 
                                                                        double? nota, 
                                                                        long? conceitoId,
                                                                        TipoAlteracao tipoAlteracao)
        {
             AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            Nota = nota;
            ConceitoId = conceitoId;
            TipoAlteracao = tipoAlteracao;
        }
        public string AlunoCodigo { get; }
        public int Bimestre { get; }
        public long TurmaId { get; }
        public long ComponenteCurricularId { get; }
        public double? Nota { get; }
        public long? ConceitoId { get; }
        public TipoAlteracao TipoAlteracao { get; }
    }

    public class SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommandValidator : AbstractValidator<SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand>
    {
        public SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommandValidator()
        {
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage(
                    "Código do aluno deve ser informado.");
            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage(
                    "Id do componente curricular deve ser informado.");
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage(
                    "Id da turma deve ser informado.");
            RuleFor(c => c.Bimestre)
                .InclusiveBetween(0, 4)
                .WithMessage(
                    "Bimestre deve ser informado (0...4).");
            RuleFor(c => (int)c.TipoAlteracao)
                .InclusiveBetween(1, 2)
                .WithMessage(
                    "Tipo Alteração deve ser informada.");
            RuleFor(c => c.TipoAlteracao)
               .NotNull()
               .WithMessage(
                   "Tipo Alteração deve ser informada.");
        }
    }

    public enum TipoAlteracao
    {
        [Display(Name = "Fechamento Nota")]
        FechamentoNota = 1,

        [Display(Name = "Conselho Classe Nota")]
        ConselhoClasseNota = 2
    }
}