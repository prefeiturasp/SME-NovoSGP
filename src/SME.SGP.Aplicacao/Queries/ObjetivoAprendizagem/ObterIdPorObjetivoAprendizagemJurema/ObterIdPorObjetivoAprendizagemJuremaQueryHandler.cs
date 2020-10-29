using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPorObjetivoAprendizagemJuremaQueryHandler : IRequestHandler<ObterIdPorObjetivoAprendizagemJuremaQuery, long>
    {
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;

        public ObterIdPorObjetivoAprendizagemJuremaQueryHandler(IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano)
        {
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }
        public async Task<long> Handle(ObterIdPorObjetivoAprendizagemJuremaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioObjetivosPlano.ObterIdPorObjetivoAprendizagemJuremaAsync(request.PlanoId, request.ObjetivoAprendizagemJuremaId);
        }
    }
}
