using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterCabecalhoEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterCabecalhoEncaminhamentoNAAPAQuery, EncaminhamentoNAAPA>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }


        public ObterCabecalhoEncaminhamentoNAAPAQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) 
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<EncaminhamentoNAAPA> Handle(ObterCabecalhoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterCabecalhoEncaminhamentoPorId(request.EncaminhamentoNAAPAId);
        }
    }
}
