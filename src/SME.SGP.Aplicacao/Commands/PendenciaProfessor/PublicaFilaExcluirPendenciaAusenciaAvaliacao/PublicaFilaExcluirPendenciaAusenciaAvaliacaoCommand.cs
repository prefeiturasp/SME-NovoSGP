using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommand : IRequest<bool>
    {
        public PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommand(string turmaCodigo, string[] componentesCurriculares, Dominio.Usuario usuario, DateTime dataAvaliacao)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            Usuario = usuario;
            DataAvaliacao = dataAvaliacao;
        }

        public string TurmaCodigo { get; set; }
        public string[] ComponentesCurriculares { get; set; }
        public Dominio.Usuario Usuario { get; set; }
        public DateTime DataAvaliacao { get; set; }
    }

    public class PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommandValidator : AbstractValidator<PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommand>
    {
        public PublicaFilaExcluirPendenciaAusenciaAvaliacaoCommandValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para verificação de exlusão de pendencia do avaliação.");

            RuleFor(c => c.ComponentesCurriculares)
               .NotEmpty()
               .WithMessage("Os componentes curriculares devem ser informados para verificação de exlusão de pendencia do avaliação.");

            RuleFor(c => c.Usuario)
               .NotEmpty()
               .WithMessage("O usuário deve ser informado para verificação de exlusão de pendencia do avaliação.");
        }
    }
}
