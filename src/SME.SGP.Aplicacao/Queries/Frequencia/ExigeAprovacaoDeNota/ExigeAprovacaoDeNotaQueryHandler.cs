using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExigeAprovacaoDeNotaQueryHandler : IRequestHandler<ExigeAprovacaoDeNotaQuery, bool>
    {
        private readonly IMediator mediator;
        public ExigeAprovacaoDeNotaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExigeAprovacaoDeNotaQuery request, CancellationToken cancellationToken)
        {
            return request.Turma.AnoLetivo < DateTime.Today.Year
                    && !(await mediator.Send(ObterUsuarioLogadoQuery.Instance)).EhGestorEscolar()
                    && await ParametroAprovacaoAtivo(request.Turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, anoLetivo));
            if (parametro.EhNulo())
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotafechamento' para o ano {anoLetivo}");

            return parametro.Ativo;
        }
    }
}
