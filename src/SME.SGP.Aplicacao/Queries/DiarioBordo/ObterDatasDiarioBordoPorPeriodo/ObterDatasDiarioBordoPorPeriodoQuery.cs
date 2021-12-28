using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiarioBordoPorPeriodoQuery : IRequest<IEnumerable<DiarioBordoPorPeriodoDto>>
    {
        public string TurmaCodigo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public long ComponenteCurricularFilhoId { get; set; }
        public string ComponenteCurricularPaiCodigo { get; set; }

        public ObterDatasDiarioBordoPorPeriodoQuery(string turmaCodigo, DateTime dataInicio, DateTime dataFim, long componenteCurricularFilhoId, string componenteCurricularPaiCodigo)
        {
            TurmaCodigo = turmaCodigo;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComponenteCurricularFilhoId = componenteCurricularFilhoId;
            ComponenteCurricularPaiCodigo = componenteCurricularPaiCodigo;
        }
    }

    public class ObterDatasDiarioBordoPorPeriodoQueryValidator : AbstractValidator<ObterDatasDiarioBordoPorPeriodoQuery>
    {
        public ObterDatasDiarioBordoPorPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma para consultar as datas das aulas com/sem diário de bordo");

            RuleFor(a => a.DataInicio)
                .NotEmpty()
                .WithMessage("É necessário informar a data de início do período para consultar as datas das aulas com/sem diário de bordo");

            RuleFor(a => a.DataFim)
                .NotEmpty()
                .WithMessage("É necessário informar a data de término do período para consultar as datas das aulas com/sem diário de bordo");
        }
    }
}
