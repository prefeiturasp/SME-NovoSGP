using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosCargosPorUeCargosQuery : IRequest<IEnumerable<FuncionarioCargoDTO>>
    {
        public ObterFuncionariosCargosPorUeCargosQuery(string ueCodigo, IEnumerable<int> cargosIdsDaUe, string dreCodigo)
        {
            this.UeCodigo = ueCodigo;
            this.cargosIdsDaUe = cargosIdsDaUe;
            DreCodigo = dreCodigo;
        }

        public string UeCodigo { get; set; }
        public IEnumerable<int> cargosIdsDaUe { get; set; }
        public string DreCodigo { get; internal set; }
    }
}