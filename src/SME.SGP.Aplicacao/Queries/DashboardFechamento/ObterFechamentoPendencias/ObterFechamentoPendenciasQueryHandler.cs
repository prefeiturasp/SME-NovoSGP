using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class
        ObterFechamentoPendenciasQueryHandler : IRequestHandler<ObterFechamentoPendenciasQuery,IEnumerable<FechamentoPendenciaQuantidadeDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorio;

        public ObterFechamentoPendenciasQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }



        public async Task<IEnumerable<FechamentoPendenciaQuantidadeDto>> Handle(ObterFechamentoPendenciasQuery request,
            CancellationToken cancellationToken)
            => await repositorio.ObterSituacaoPendenteFechamento(request.UeId,
                request.Ano,
                request.DreId,
                request.Modalidade,
                request.Semestre,
                request.Bimestre);
    }
}