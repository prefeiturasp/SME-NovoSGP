using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeCodigoPorIdQueryHandler : IRequestHandler<ObterUeCodigoPorIdQueryHandler, string>
    {
        private readonly IRepositorioUeConsultas repositorioUeConsultas;

        public ObterUeCodigoPorIdQueryHandler(IRepositorioUeConsultas repositorioUeConsultas)
        {
            this.repositorioUeConsultas = repositorioUeConsultas ?? throw new ArgumentNullException(nameof(repositorioUeConsultas));
        }
    }
}
