using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAComTurmaPorIdQueryHandler : IRequestHandler<ObterAtendimentoNAAPAComTurmaPorIdQuery, EncaminhamentoNAAPA>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public ObterAtendimentoNAAPAComTurmaPorIdQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<EncaminhamentoNAAPA> Handle(ObterAtendimentoNAAPAComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioEncaminhamentoNAAPA.ObterEncaminhamentoComTurmaPorId(request.EncaminhamentoId);
    }
}
