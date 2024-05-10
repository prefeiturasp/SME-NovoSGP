using System;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaIntervaloTurmaDisciplinaQuery : IRequest<AulaConsultaDto>
    {
        public ObterAulaIntervaloTurmaDisciplinaQuery(DateTime dataInicio, DateTime dataFim, string turmaId,
            long atividadeAvaliativaId)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            TurmaId = turmaId;
            AtividadeAvaliativaId = atividadeAvaliativaId;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TurmaId { get; set; }
        public long AtividadeAvaliativaId { get; set; }
    }

    public class ObterAulaIntervaloTurmaDisciplinaQueryValidator :
        AbstractValidator<ObterAulaIntervaloTurmaDisciplinaQuery>
    {
        public ObterAulaIntervaloTurmaDisciplinaQueryValidator()
        {
            RuleFor(a => a.DataInicio)
                .NotEmpty().WithMessage("A Data de Início deve ser informada para Obter Aula Intervalo Turma Disciplina");
            RuleFor(a => a.DataFim)
                .NotEmpty().WithMessage("A Data de Fim deve ser informada para Obter Aula Intervalo Turma Disciplina");
            RuleFor(a => a.TurmaId)
                .NotEmpty().WithMessage("A Turma deve ser informada para Obter Aula Intervalo Turma Disciplina");
            RuleFor(a => a.AtividadeAvaliativaId)
                .NotEmpty().WithMessage("A Atividade Avaliativa deve ser informada para Obter Aula Intervalo Turma Disciplina");
        }
    }
}