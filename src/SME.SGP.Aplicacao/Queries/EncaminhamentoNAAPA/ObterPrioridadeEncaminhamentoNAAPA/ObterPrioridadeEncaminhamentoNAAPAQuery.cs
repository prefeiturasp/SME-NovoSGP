﻿using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPrioridadeEncaminhamentoNAAPAQuery :  IRequest<IEnumerable<PrioridadeEncaminhamentoNAAPADto>>
    {
        private static ObterPrioridadeEncaminhamentoNAAPAQuery _instance;
        public static ObterPrioridadeEncaminhamentoNAAPAQuery Instance => _instance ??= new();
    }
}
