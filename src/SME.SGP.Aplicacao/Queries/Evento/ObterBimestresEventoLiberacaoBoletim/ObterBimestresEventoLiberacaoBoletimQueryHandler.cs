﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresEventoLiberacaoBoletimQueryHandler : IRequestHandler<ObterBimestresEventoLiberacaoBoletimQuery, int[]>
    {
        public readonly IRepositorioEventoBimestre repositorioEventoBimestre;

        public ObterBimestresEventoLiberacaoBoletimQueryHandler(IRepositorioEventoBimestre repositorioEventoBimestre)
        {
            this.repositorioEventoBimestre = repositorioEventoBimestre ?? throw new System.ArgumentNullException(nameof(repositorioEventoBimestre));
        }

        public async Task<int[]> Handle(ObterBimestresEventoLiberacaoBoletimQuery request, CancellationToken cancellationToken)
        {
            var bimestres = await repositorioEventoBimestre.ObterBimestresEventoPorTipoCalendarioDataReferencia(request.TipoCalendarioId, request.DataRefencia);

            if (bimestres.EhNulo() || !bimestres.Any())
                throw new NegocioException("Lista de bimestres não encontrada");

            return bimestres;
        }
    }
}
