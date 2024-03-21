﻿using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesNiveisCargosQuery : IRequest<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>>
    {
        public ObterNotificacoesNiveisCargosQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
    }
}
