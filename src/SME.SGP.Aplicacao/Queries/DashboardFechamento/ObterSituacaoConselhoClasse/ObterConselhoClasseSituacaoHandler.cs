using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseSituacaoHandler : IRequestHandler<ObterConselhoClasseSituacaoQuery,
            IEnumerable<ConselhoClasseSituacaoQuantidadeDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorio;

        public ObterConselhoClasseSituacaoHandler(IRepositorioFechamentoTurmaDisciplina repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public async Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> Handle(ObterConselhoClasseSituacaoQuery request, 
            CancellationToken cancellationToken)
        {
           return await repositorio.ObterConselhoClasseSituacao(request.UeId,
                request.Ano, request.DreId, request.Modalidade,
                request.Semestre, request.Bimestre);
        }
    }
}
