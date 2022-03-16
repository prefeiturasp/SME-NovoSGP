using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseSituacaoQueryHandler : IRequestHandler<ObterConselhoClasseSituacaoQuery, IEnumerable<ConselhoClasseSituacaoQuantidadeDto>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterConselhoClasseSituacaoQueryHandler(IRepositorioConselhoClasse repositorio)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }
        public async Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> Handle(ObterConselhoClasseSituacaoQuery request, CancellationToken cancellationToken)
        {
           return await repositorioConselhoClasseConsulta.ObterConselhoClasseSituacao(request.UeId,
                                                                request.Ano, 
                                                                request.DreId,
                                                                request.Modalidade,
                                                                request.Semestre,   
                                                                request.Bimestre);
        }
    }
}
