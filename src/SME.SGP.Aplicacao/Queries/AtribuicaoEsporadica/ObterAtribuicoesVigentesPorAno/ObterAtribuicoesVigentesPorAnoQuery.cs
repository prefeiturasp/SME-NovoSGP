using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesVigentesPorAnoQuery : IRequest<IEnumerable<AtribuicaoEsporadicaVigenteProfDto>>
    {
        public ObterAtribuicoesVigentesPorAnoQuery(int anoLetivo, DateTime? data)
        {
            AnoLetivo = anoLetivo;
            Data = data;
        }

        public int AnoLetivo { get; set; }
        public DateTime? Data { get; set; }
    }
}
