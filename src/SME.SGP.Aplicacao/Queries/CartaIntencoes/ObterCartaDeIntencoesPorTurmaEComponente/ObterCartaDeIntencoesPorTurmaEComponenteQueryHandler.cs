using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaDeIntencoesPorTurmaEComponenteQueryHandler : IRequestHandler<ObterCartaDeIntencoesPorTurmaEComponenteQuery, IEnumerable<CartaIntencoes>>
    {
        private readonly IRepositorioCartaIntencoes repositorioCartaIntencoes;

        public ObterCartaDeIntencoesPorTurmaEComponenteQueryHandler(IRepositorioCartaIntencoes repositorioCartaIntencoes)
        {
            this.repositorioCartaIntencoes = repositorioCartaIntencoes ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoes));
        }

        public async Task<IEnumerable<CartaIntencoes>> Handle(ObterCartaDeIntencoesPorTurmaEComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioCartaIntencoes.ObterPorTurmaEComponente(request.TurmaCodigo, request.ComponenteCurricularId);
    }
}
