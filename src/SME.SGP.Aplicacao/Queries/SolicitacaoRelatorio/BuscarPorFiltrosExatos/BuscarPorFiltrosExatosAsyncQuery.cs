using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Queries.SolicitacaoRelatorio.RelatorioJaSolicitado;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class BuscarPorFiltrosExatosAsyncQuery : IRequest<IEnumerable<SolicitacaoRelatorio>>
    {
        public BuscarPorFiltrosExatosAsyncQuery(FiltroRelatorioBase filtros, TipoRelatorio? tipoRelatorio, StatusSolicitacao? statusSolicitacao)
        {
            Filtros = filtros;
            TipoRelatorio = tipoRelatorio;
            StatusSolicitacao = statusSolicitacao;
        }

        public FiltroRelatorioBase Filtros { get; set; } 
        public TipoRelatorio? TipoRelatorio { get; set; }
        public StatusSolicitacao? StatusSolicitacao { get; set; }
    }

    public class BuscarPorFiltrosExatosAsyncQueryValidator : AbstractValidator<BuscarPorFiltrosExatosAsyncQuery>
    {
        public BuscarPorFiltrosExatosAsyncQueryValidator()
        {

            RuleFor(c => c.Filtros).NotEmpty().NaoEhNulo();
        }
    }
}
