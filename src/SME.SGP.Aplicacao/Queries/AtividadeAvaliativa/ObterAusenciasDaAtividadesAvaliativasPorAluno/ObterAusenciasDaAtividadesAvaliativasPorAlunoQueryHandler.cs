using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciasDaAtividadesAvaliativasPorAlunoQueryHandler : IRequestHandler<ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery, IEnumerable<AusenciaAlunoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ObterAusenciasDaAtividadesAvaliativasPorAlunoQueryHandler(IRepositorioFrequenciaConsulta repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<AusenciaAlunoDto>> Handle(ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.ObterAusenciasPorAluno(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.AtividadesAvaliativasData, request.CodigoAluno);
        }
    }
}