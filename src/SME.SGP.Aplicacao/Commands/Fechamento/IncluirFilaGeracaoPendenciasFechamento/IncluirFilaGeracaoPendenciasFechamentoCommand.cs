using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaGeracaoPendenciasFechamentoCommand : IRequest<bool>
    {
        public IncluirFilaGeracaoPendenciasFechamentoCommand(long componenteCurricularId, string turmaCodigo, string turmaNome, DateTime periodoEscolarInicio, DateTime periodoEscolarFim, int bimestre, Usuario usuario, long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, long turmaId, bool componenteSemNota = false, bool registraFrequencia = true)
        {
            ComponenteCurricularId = componenteCurricularId;
            TurmaCodigo = turmaCodigo;
            TurmaNome = turmaNome;
            PeriodoEscolarInicio = periodoEscolarInicio;
            PeriodoEscolarFim = periodoEscolarFim;
            Bimestre = bimestre;
            Usuario = usuario;
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            Justificativa = justificativa;
            CriadoRF = criadoRF;
            ComponenteSemNota = componenteSemNota;
            RegistraFrequencia = registraFrequencia;
            TurmaId = turmaId;
        }

        public long ComponenteCurricularId { get; }
        public string TurmaCodigo { get; }
        public string TurmaNome { get; }
        public DateTime PeriodoEscolarInicio { get; }
        public DateTime PeriodoEscolarFim { get; }
        public int Bimestre { get; }
        public Usuario Usuario { get; }
        public long FechamentoTurmaDisciplinaId { get; }
        public string Justificativa { get; }
        public string CriadoRF { get; }
        public bool ComponenteSemNota { get; }
        public bool RegistraFrequencia { get; }
        public long TurmaId { get; set; }
    }

    public class IncluirFilaGeracaoPendenciasFechamentoCommandValidator : AbstractValidator<IncluirFilaGeracaoPendenciasFechamentoCommand>
    {
        public IncluirFilaGeracaoPendenciasFechamentoCommandValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do Componente Curricular é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da Turma é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.TurmaNome)
                .NotEmpty()
                .WithMessage("O nome da Turma é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.PeriodoEscolarInicio)
                .NotEmpty()
                .WithMessage("A data de inicio do Período Escolar é necessária para verificação de pendências no fechamento.");

            RuleFor(a => a.PeriodoEscolarFim)
                .NotEmpty()
                .WithMessage("A data de fim do Período Escolar é necessária para verificação de pendências no fechamento.");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O Bimestre é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.Usuario)
                .NotEmpty()
                .WithMessage("O Usuário é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("O id do Fechamento Turma x Disciplina é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.CriadoRF)
                .NotEmpty()
                .WithMessage("O RF do criador do Fechamento Turma x Disciplina é necessário para verificação de pendências no fechamento.");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("A turma Id é necessário para verificação de pendências no fechamento.");
        }
    }
}
