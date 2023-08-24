using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterExecutarManutencaoAulasInfantilQuery : IRequest<bool>
    {
        private static ObterExecutarManutencaoAulasInfantilQuery _instance;
        public static ObterExecutarManutencaoAulasInfantilQuery Instance => _instance ??= new();
    }
}
