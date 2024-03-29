﻿using MediatR;
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
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTiposCalendariosPorAnosLetivoModalidadesQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public Task<IEnumerable<TipoCalendarioBuscaDto>> Handle(ObterTiposCalendariosPorAnosLetivoModalidadesQuery request, CancellationToken cancellationToken)
        {
            return repositorioTipoCalendario.ListarPorAnosLetivoEModalidades(request.AnosLetivos, request.Modalidades, request.Descricao);
        }
    }
}
