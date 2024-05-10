using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFechamentosPorTurmasCodigosBimestreQuery(string[] turmasCodigos, string alunoCodigo, int bimestre,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, int? anoLetivo = null, long? tipoCalendario = null)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            AnoLetivo = anoLetivo;
            TipoCalendario = tipoCalendario;
        }
        
        public string[] TurmasCodigos { get; }
        public string AlunoCodigo { get; }
        public int Bimestre { get; }
        public DateTime? DataMatricula { get; }
        public DateTime? DataSituacao { get; }
        public int? AnoLetivo { get; }
        public long? TipoCalendario { get; set; }
    }
    
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQueryValidator : AbstractValidator<ObterNotasFechamentosPorTurmasCodigosBimestreQuery>
    {
        public ObterNotasFechamentosPorTurmasCodigosBimestreQueryValidator()
        {
            RuleFor(a => a.TurmasCodigos)
                .NotNull()
                .NotEmpty()
                .WithMessage("Necessário informar os códigos de turmas para obter as notas de fechamento");

            RuleFor(a => a.AlunoCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno para obter as notas de fechamento");

            RuleFor(a => a.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Necessário informar o bimestre para obter as notas de fechamento");
        }
    }    
}
