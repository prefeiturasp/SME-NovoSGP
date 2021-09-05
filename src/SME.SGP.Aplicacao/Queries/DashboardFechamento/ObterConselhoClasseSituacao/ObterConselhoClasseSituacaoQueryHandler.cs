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
        private readonly IRepositorioConselhoClasse repositorio;

        public ObterConselhoClasseSituacaoQueryHandler(IRepositorioConselhoClasse repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public async Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> Handle(ObterConselhoClasseSituacaoQuery request, CancellationToken cancellationToken)
        {
           return await repositorio.ObterConselhoClasseSituacao(request.UeId,
                                                                request.Ano, 
                                                                request.DreId,
                                                                request.Modalidade,
                                                                request.Semestre,   
                                                                request.Bimestre);
        }
    }
}
