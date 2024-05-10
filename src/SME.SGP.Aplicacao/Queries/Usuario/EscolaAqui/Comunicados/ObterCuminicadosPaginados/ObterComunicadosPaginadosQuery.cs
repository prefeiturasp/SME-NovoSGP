using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosQuery : IRequest<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>
    {
        public ObterComunicadosPaginadosQuery(FiltroComunicadoDto filtro)
        {
            AnoLetivo = filtro.AnoLetivo;
            DreCodigo = filtro.DreCodigo;
            UeCodigo = filtro.UeCodigo;
            Modalidades = filtro.Modalidades;
            Semestre = filtro.Semestre;
            DataEnvioInicio = filtro.DataEnvioInicio;
            DataEnvioFim = filtro.DataEnvioFim;
            DataExpiracaoInicio = filtro.DataExpiracaoInicio;
            DataExpiracaoFim = filtro.DataExpiracaoFim;
            Titulo = filtro.Titulo;
            TurmasCodigo = filtro.TurmasCodigo;
            AnosEscolares = filtro.AnosEscolares;
            TiposEscolas = filtro.TiposEscolas;
        }

        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public DateTime? DataEnvioInicio { get; set; }
        public DateTime? DataEnvioFim { get; set; }
        public DateTime? DataExpiracaoInicio { get; set; }
        public DateTime? DataExpiracaoFim { get; set; }
        public string Titulo { get; set; }
        public string[] TurmasCodigo { get; set; }
        public string[] AnosEscolares { get; set; }
        public int[] TiposEscolas { get; set; }
    }

    public class ObterComunicadosPaginadosQueryValidator : AbstractValidator<ObterComunicadosPaginadosQuery>
    {
        public ObterComunicadosPaginadosQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
            RuleFor(a => a.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da DRE deve ser informado.");
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");            
            RuleFor(a => a.Modalidades)
                .NotEmpty()
                .WithMessage("Pelo menos uma modalidade deve ser informada.");
        }
    }
}
