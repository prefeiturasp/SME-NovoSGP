using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComComponentesQuery : IRequest<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>
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
        public bool ConsideraHistorico { get; set; }
        public DateTime PeriodoEscolarInicio { get; set; }
        public string AnosInfantilDesconsiderar { get; set; }

        public ObterTurmasComComponentesQuery(FiltroTurmaDto filtro, int qtdeRegistros, int qtdeRegistrosIgnorados, Usuario usuario, DateTime periodoEscolarInicio, string anosInfantilDesconsiderar)
        {
            UeCodigo = filtro.UeCodigo;
            DreCodigo = filtro.DreCodigo;
            Bimestre = filtro.Bimestre;
            TurmaCodigo = filtro.TurmaCodigo;
            AnoLetivo = filtro.AnoLetivo;
            QtdeRegistros = qtdeRegistros;
            QtdeRegistrosIgnorados = qtdeRegistrosIgnorados;
            Modalidade = filtro.Modalidade;
            Semestre = filtro.Semestre;
            EhProfessor = usuario.EhPerfilProfessor();
            CodigoRf = usuario.CodigoRf;
            ConsideraHistorico = filtro.ConsideraHistorico;
            PeriodoEscolarInicio = periodoEscolarInicio;
            AnosInfantilDesconsiderar = anosInfantilDesconsiderar;
        }

        public ObterTurmasComComponentesQuery(Turma turma, int qtdeRegistros, int qtdeRegistrosIgnorados, DateTime periodoEscolarInicio)
        {
            UeCodigo = turma.Ue.CodigoUe;
            DreCodigo = turma.Ue.Dre.CodigoDre;
            Bimestre = null;
            TurmaCodigo = turma.CodigoTurma;
            AnoLetivo = turma.AnoLetivo;
            QtdeRegistros = qtdeRegistros;
            QtdeRegistrosIgnorados = qtdeRegistrosIgnorados;
            Modalidade = turma.ModalidadeCodigo;
            Semestre = turma.Semestre;
            EhProfessor = false;
            CodigoRf = string.Empty;
            ConsideraHistorico = turma.Historica;
            PeriodoEscolarInicio = periodoEscolarInicio;
            AnosInfantilDesconsiderar = string.Empty;
        }

        public class ObterTurmasComComponentesQueryValidator : AbstractValidator<ObterTurmasComComponentesQuery>
        {
            public ObterTurmasComComponentesQueryValidator()
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
                RuleFor(c => c.PeriodoEscolarInicio)
                   .NotEmpty()
                   .WithMessage("O início do período escolar deve ser informado.");
            }
        }
    }
}
