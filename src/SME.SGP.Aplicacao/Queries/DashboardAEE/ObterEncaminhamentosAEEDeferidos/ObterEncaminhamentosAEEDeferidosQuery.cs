using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEDeferidosQuery : IRequest<IEnumerable<AEETurmaDto>>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public long DreId { get; set; }

        public ObterEncaminhamentosAEEDeferidosQuery(int ano, long dreId, long ueId)
        {
            Ano = ano;
            DreId = dreId;
            UeId = ueId;
        }
    }
}
