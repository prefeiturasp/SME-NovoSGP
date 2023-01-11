using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarEncaminhamentoNAAPASecaoCommandHandler : IRequestHandler<RegistrarEncaminhamentoNAAPASecaoCommand, EncaminhamentoNAAPASecao>
    {
        private readonly IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public RegistrarEncaminhamentoNAAPASecaoCommandHandler(IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<EncaminhamentoNAAPASecao> Handle(RegistrarEncaminhamentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = MapearParaEntidade(request);
            await repositorioEncaminhamentoNAAPASecao.SalvarAsync(secao);
            return secao;
        }

        private EncaminhamentoNAAPASecao MapearParaEntidade(RegistrarEncaminhamentoNAAPASecaoCommand request)
            => new ()
            {
                SecaoEncaminhamentoNAAPAId = request.SecaoId,
                Concluido = request.Concluido,
                EncaminhamentoNAAPAId = request.EncaminhamentoNAAPAId
            };
    }
}
