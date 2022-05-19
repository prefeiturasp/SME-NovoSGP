using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesComponenteNaoLancaNotaQueryHandler : IRequestHandler<ObterTotalCompensacoesComponenteNaoLancaNotaQuery, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;
        public ObterTotalCompensacoesComponenteNaoLancaNotaQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta;
        }
        public async Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> Handle(ObterTotalCompensacoesComponenteNaoLancaNotaQuery request, CancellationToken cancellationToken)
        {
            if (request.Bimestre > 0)
                return await repositorioConselhoClasseConsulta.ObterTotalCompensacoesComponenteNaoLancaNotaPorBimestre(request.CodigoTurma, request.Bimestre);
            return await repositorioConselhoClasseConsulta.ObterTotalCompensacoesComponenteNaoLancaNota(request.CodigoTurma);
        }
    }
}
