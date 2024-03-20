using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosCargosPorUeCargosQuery : IRequest<IEnumerable<FuncionarioCargoDTO>>
    {
        public string UeCodigo;
        public IEnumerable<int> cargosIdsDaUe;

        public ObterFuncionariosCargosPorUeCargosQuery(string ueCodigo, IEnumerable<int> cargosIdsDaUe, string dreCodigo)
        {
            this.UeCodigo = ueCodigo;
            this.cargosIdsDaUe = cargosIdsDaUe;
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; internal set; }
    }
}