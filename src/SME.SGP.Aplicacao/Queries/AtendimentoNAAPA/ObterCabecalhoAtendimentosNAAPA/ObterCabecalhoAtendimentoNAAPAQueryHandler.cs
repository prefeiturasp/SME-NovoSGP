using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCabecalhoAtendimentoNAAPAQueryHandler : IRequestHandler<ObterCabecalhoAtendimentoNAAPAQuery, EncaminhamentoNAAPA>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }


        public ObterCabecalhoAtendimentoNAAPAQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) 
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<EncaminhamentoNAAPA> Handle(ObterCabecalhoAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterCabecalhoEncaminhamentoPorId(request.EncaminhamentoNAAPAId);
        }
    }
}
