using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoQueryHandler  : IRequestHandler<ObterAusenciaParaCompensacaoQuery,IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterAusenciaParaCompensacaoQueryHandler(IRepositorioCompensacaoAusencia repositorioCompensacao)
        {
            repositorioCompensacaoAusencia = repositorioCompensacao ?? throw new ArgumentNullException(nameof(repositorioCompensacao));
        }

        public async Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> Handle(ObterAusenciaParaCompensacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCompensacaoAusencia.ObterAusenciaParaCompensacao(request.Filtro);
        }
    }
}