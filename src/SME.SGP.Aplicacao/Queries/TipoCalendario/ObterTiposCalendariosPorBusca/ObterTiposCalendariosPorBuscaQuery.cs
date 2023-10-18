using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosPorBuscaQuery : IRequest<IEnumerable<TipoCalendarioBuscaDto>>
    {
        public ObterTiposCalendariosPorBuscaQuery(string descricao)
        {
            Descricao = descricao;
        }
        public string Descricao { get; set; }
    }
}
