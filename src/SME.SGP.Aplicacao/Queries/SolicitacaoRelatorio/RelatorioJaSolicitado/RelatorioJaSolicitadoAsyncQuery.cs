using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Aplicacao.Queries.SolicitacaoRelatorio.RelatorioJaSolicitado
{
    public class RelatorioJaSolicitadoAsyncQuery : IRequest<bool>
    {
        public RelatorioJaSolicitadoAsyncQuery(FiltroRelatorioBase filtros, TipoRelatorio tipoRelatorio, string usuarioQueSolicitou)
        {
            Filtros = filtros;
            TipoRelatorio = tipoRelatorio;
            UsuarioQueSolicitou = usuarioQueSolicitou;
        }

        public FiltroRelatorioBase Filtros { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
    }

    public class RelatorioJaSolicitadoAsyncQueryValidator : AbstractValidator<RelatorioJaSolicitadoAsyncQuery>
    {
        public RelatorioJaSolicitadoAsyncQueryValidator()
        {
            RuleFor(c => c.UsuarioQueSolicitou).NotEmpty().NaoEhNulo();
            RuleFor(c => c.TipoRelatorio).NotEmpty().NaoEhNulo();
            RuleFor(c => c.Filtros).NotEmpty().NaoEhNulo();
        }
    }
}
