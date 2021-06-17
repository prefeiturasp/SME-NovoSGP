using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasDaAtividadesAvaliativasQueryHandler : IRequestHandler<ObterAusenciasDaAtividadesAvaliativasQuery, IEnumerable<AusenciaAlunoDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterAusenciasDaAtividadesAvaliativasQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }
        public async Task<IEnumerable<AusenciaAlunoDto>> Handle(ObterAusenciasDaAtividadesAvaliativasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.ObterAusencias(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.AtividadesAvaliativasData, request.AlunosId);
        }
    }
}
