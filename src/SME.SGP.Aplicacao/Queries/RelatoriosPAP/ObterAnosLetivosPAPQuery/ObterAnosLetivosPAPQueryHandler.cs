using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosPAPQueryHandler : IRequestHandler<ObterAnosLetivosPAPQuery, IEnumerable<ObterAnoLetivoPAPRetornoDto>>
    {
        private readonly IMediator mediator;

        public ObterAnosLetivosPAPQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<ObterAnoLetivoPAPRetornoDto>> Handle(ObterAnosLetivosPAPQuery request, CancellationToken cancellationToken)
        {
            var parametroAnoInicial = await mediator.Send(new ObterParametrosSistemaPorTiposQuery() { Tipos = new long[] { (long)TipoParametroSistema.PAPInicioAnoLetivo } });
            if (parametroAnoInicial == null || !parametroAnoInicial.Any())
                throw new NegocioException("Não foi possível localizar o parâmetro do sistema de início de ano letivo PAP.");

            var anoInicial = int.Parse(parametroAnoInicial.FirstOrDefault().Valor);
         
            var retorno = new List<ObterAnoLetivoPAPRetornoDto>();

            while (anoInicial <= request.AnoAtual)
            {
                retorno.Add(new ObterAnoLetivoPAPRetornoDto() { Ano = anoInicial, EhSugestao = anoInicial == request.AnoAtual });
                anoInicial++;
            }
            return retorno;
        }
    }
}
