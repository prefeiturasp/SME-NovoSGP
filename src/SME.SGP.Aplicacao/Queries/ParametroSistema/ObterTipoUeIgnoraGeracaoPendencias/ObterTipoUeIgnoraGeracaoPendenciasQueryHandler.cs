using MediatR;
using Minio.DataModel;
using Org.BouncyCastle.Asn1.Ocsp;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoUeIgnoraGeracaoPendenciasQueryHandler : IRequestHandler<ObterTipoUeIgnoraGeracaoPendenciasQuery, bool>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IMediator mediator;

        public ObterTipoUeIgnoraGeracaoPendenciasQueryHandler(IMediator mediator, IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(ObterTipoUeIgnoraGeracaoPendenciasQuery request, CancellationToken cancellationToken)
        {
            var dadosParametro = await repositorioParametrosSistema.ObterParametroPorTipoEAno(TipoParametroSistema.TiposUEIgnorarGeracaoPendencia, DateTimeExtension.HorarioBrasilia().Year);
            if (dadosParametro.EhNulo() || !dadosParametro.Ativo) return false;
            
            TipoEscola? tipoUe = request.TipoUe;
            if (tipoUe.EhNulo())
               tipoUe = await mediator.Send(new ObterTipoEscolaPorCodigoUEQuery(request.CodigoUe));
            return dadosParametro.NaoEhNulo() ? dadosParametro.Valor.Split(',').Contains(((int)tipoUe).ToString()) : false;           
        }
            
    }
}
