using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries.SolicitacaoRelatorio.RelatorioJaSolicitado
{
    public class RelatorioJaSolicitadoQuery : IRequest<bool>
    {
        public RelatorioJaSolicitadoQuery(string filtrosUsados, TipoRelatorio tipoRelatorio, string usuarioQueSolicitou)
        {
            FiltrosUsados = filtrosUsados;
            TipoRelatorio = tipoRelatorio;
            UsuarioQueSolicitou = usuarioQueSolicitou;
        }

        public string FiltrosUsados { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
    }

    public class RelatorioJaSolicitadoAsyncQueryValidator : AbstractValidator<RelatorioJaSolicitadoQuery>
    {
        public RelatorioJaSolicitadoAsyncQueryValidator()
        {
            RuleFor(c => c.UsuarioQueSolicitou).NotEmpty().NaoEhNulo();
            RuleFor(c => c.TipoRelatorio).NotEmpty().NaoEhNulo();
            RuleFor(c => c.FiltrosUsados).NotEmpty().NaoEhNulo();
        }
    }
}
