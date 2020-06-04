using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RelatorioGamesCommand : IRequest<bool>
    {
        public int Ano { get; set; }
    }
}
