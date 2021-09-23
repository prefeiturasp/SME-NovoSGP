using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosPorAnosLetivoModalidadesQueryHandler : IRequestHandler<ObterTiposCalendariosPorAnosLetivoModalidadesQuery, IEnumerable<TipoCalendarioBuscaDto>>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTiposCalendariosPorAnosLetivoModalidadesQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<IEnumerable<TipoCalendarioBuscaDto>> Handle(ObterTiposCalendariosPorAnosLetivoModalidadesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ListarPorAnosLetivoEModalidades(request.AnosLetivos, request.Modalidades, request.Descricao);
        }
    }
}
