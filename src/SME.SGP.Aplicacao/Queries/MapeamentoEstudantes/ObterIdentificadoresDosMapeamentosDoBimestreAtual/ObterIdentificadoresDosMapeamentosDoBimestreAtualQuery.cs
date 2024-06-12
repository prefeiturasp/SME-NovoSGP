using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdentificadoresDosMapeamentosDoBimestreAtualQuery(DateTime dataBase)
        {
            DataBase = dataBase;
        }

        public DateTime DataBase { get; set; }
    }
}
