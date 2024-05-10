using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery, TipoCalendario>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTipoCalendarioPorAnoLetivoEModalidadeQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<TipoCalendario> Handle(ObterTipoCalendarioPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
         => await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(request.AnoLetivo, request.Modalidade, request.Semestre);
    }
}
