using MediatR;
using MongoDB.Bson.Serialization.IdGenerators;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDasNotificacoesParaExclusaoPorDiasQueryHandler : IRequestHandler<ObterIdsDasNotificacoesParaExclusaoPorDiasQuery, IEnumerable<long>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacao repositorio;
        private List<ParametrosSistema> parametrosSistema;

        public ObterIdsDasNotificacoesParaExclusaoPorDiasQueryHandler(IMediator mediator, IRepositorioNotificacao repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsDasNotificacoesParaExclusaoPorDiasQuery request, CancellationToken cancellationToken)
        {
            this.parametrosSistema = (await ObterParametrosDiasExclusao()).ToList();

            if (!parametrosSistema.Any())
                return Enumerable.Empty<long>();

            return await repositorio.ObterIdsNotificacoesParaExclusao(
                                                                    DateTime.Now.Year,
                                                                    ObterDiasPorTipo(TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAlerta),
                                                                    ObterDiasPorTipo(TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAviso),
                                                                    ObterDiasPorTipo(TipoParametroSistema.DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta));
        }

        private async Task<IEnumerable<ParametrosSistema>> ObterParametrosDiasExclusao()
        {
            var tipoParametroDiasExclusao = new long[] { (long)TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAlerta,
                                                         (long)TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAviso,
                                                         (long)TipoParametroSistema.DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta };

            return await mediator.Send(new ObterParametrosSistemaPorTiposQuery() { Tipos = tipoParametroDiasExclusao });
        }

        private long? ObterDiasPorTipo(TipoParametroSistema tipoParametro)
        {
            var parametro = parametrosSistema.Find(p => p.Tipo == tipoParametro);

            if (parametro.NaoEhNulo())
                return long.Parse(parametro.Valor);

            return null;
        }
    }
}
