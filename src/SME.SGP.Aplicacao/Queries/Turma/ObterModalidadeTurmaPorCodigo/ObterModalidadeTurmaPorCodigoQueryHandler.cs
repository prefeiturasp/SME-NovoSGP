using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadeTurmaPorCodigoQueryHandler : IRequestHandler<ObterModalidadeTurmaPorCodigoQuery, Modalidade>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterModalidadeTurmaPorCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Modalidade> Handle(ObterModalidadeTurmaPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterModalidadePorCodigo(request.TurmaCodigo);
    }
}
