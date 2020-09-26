using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterExecutarManutencaoAulasInfantilQueryHandler : IRequestHandler<ObterExecutarManutencaoAulasInfantilQuery, bool>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterExecutarManutencaoAulasInfantilQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }
        public async Task<bool> Handle(ObterExecutarManutencaoAulasInfantilQuery request, CancellationToken cancellationToken)
        {
            var valor = await this.repositorioParametrosSistema.ObterValorUnicoPorTipo<string>(Dominio.TipoParametroSistema.ExecutarManutencaoAulasInfantil);

            return valor.Equals("1");
        }
    }
}
