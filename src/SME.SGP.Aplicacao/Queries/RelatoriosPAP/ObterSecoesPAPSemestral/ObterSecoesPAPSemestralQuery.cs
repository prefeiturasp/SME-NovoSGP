﻿using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPSemestralQuery : IRequest<IEnumerable<SecaoRelatorioSemestralPAP>>
    {
    }
}
