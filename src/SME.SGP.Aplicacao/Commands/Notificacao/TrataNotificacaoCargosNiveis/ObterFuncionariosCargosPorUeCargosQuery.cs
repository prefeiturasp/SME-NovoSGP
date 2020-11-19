using System.Collections.Generic;
using MediatR;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosCargosPorUeCargosQuery : IRequest<object>
    {
        private int key;
        private IEnumerable<int> cargosIdsDaUe;

        public ObterFuncionariosCargosPorUeCargosQuery(int key, IEnumerable<int> cargosIdsDaUe)
        {
            this.key = key;
            this.cargosIdsDaUe = cargosIdsDaUe;
        }
    }
}