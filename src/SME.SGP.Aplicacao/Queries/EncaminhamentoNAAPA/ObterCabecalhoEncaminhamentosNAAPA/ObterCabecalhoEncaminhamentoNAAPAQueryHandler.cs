using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCabecalhoEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterCabecalhoEncaminhamentoNAAPAQuery, EncaminhamentoNAAPA>
    {
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }


        public ObterCabecalhoEncaminhamentoNAAPAQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<EncaminhamentoNAAPA> Handle(ObterCabecalhoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterCabecalhoEncaminhamentoPorId(request.EncaminhamentoNAAPAId);
        }
    }
}
