using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricoesRegistrosIndividuaisPorPeriodoQuery : IRequest<IEnumerable<RegistroIndividualResumoDto>>
    {
        public ObterDescricoesRegistrosIndividuaisPorPeriodoQuery(long turmaId, long alunoCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long TurmaId { get; }
        public long AlunoCodigo { get; }
        public long ComponenteCurricularId { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
    }

    public class ObterDescricoesRegistrosIndividuaisPorPeriodoQueryValidator : AbstractValidator<ObterDescricoesRegistrosIndividuaisPorPeriodoQuery>
    {
        public ObterDescricoesRegistrosIndividuaisPorPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O Código da Turma deve ser informado para consulta de Registros Individuais do Estudante");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do Aluno deve ser informado para consulta de Registros Individuais do Estudante");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O Código do Componente Curricular deve ser informado para consulta de Registros Individuais do Estudante");

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("A Data de Início deve ser informado para consulta de Registros Individuais do Estudante");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("A Data de Fim deve ser informado para consulta de Registros Individuais do Estudante");
        }
    }
}
