using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterChaveEValorPorTipoEAnoQueryHandler : IRequestHandler<ObterChaveEValorPorTipoEAnoQuery, IEnumerable<ParametroSistemaRetornoDto>>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterChaveEValorPorTipoEAnoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public Task<IEnumerable<ParametroSistemaRetornoDto>> Handle(ObterChaveEValorPorTipoEAnoQuery request, CancellationToken cancellationToken)
                        => repositorioParametrosSistema.ObterChaveEValorPorTipoEAno(request.Tipo, request.Ano);
    }
}
