using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ListagemTurmasComComponenteQuery : IRequest<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>
    {
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
        public int? Bimestre { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public int QtdeRegistros { get; set; }
        public int QtdeRegistrosIgnorados { get; set; }
        public Modalidade? Modalidade{ get; set; }
        public int? Semestre { get; set; }
        public bool EhProfessor { get; set; }
        public string CodigoRf { get; set; }

        public ListagemTurmasComComponenteQuery(string ueCodigo, string dreCodigo, string turmaCodigo, int anoLetivo, int qtdeRegistros, int qtdeRegistrosIgnorados, int? bimestre, Modalidade? modalidade, int? semestre, bool ehProfessor, string codigoRf)
        {
            UeCodigo = ueCodigo;
            DreCodigo = dreCodigo;
            Bimestre = bimestre;
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            QtdeRegistros = qtdeRegistros;
            QtdeRegistrosIgnorados = qtdeRegistrosIgnorados;
            Modalidade = modalidade;
            Semestre = semestre;
            EhProfessor = ehProfessor;
            CodigoRf = codigoRf;
        }

        public class ListagemTurmasComComponenteQueryValidator : AbstractValidator<ListagemTurmasComComponenteQuery>
        {
            public ListagemTurmasComComponenteQueryValidator()
            {
                RuleFor(c => c.UeCodigo)
                    .NotEmpty()
                    .WithMessage("O código da UE deve ser informado.");
                RuleFor(c => c.DreCodigo)
                   .NotEmpty()
                   .WithMessage("O código da DRE deve ser informado.");
                RuleFor(c => c.AnoLetivo)
                   .NotEmpty()
                   .WithMessage("O ano letivo deve ser informado.");
            }
        }
    }
}
