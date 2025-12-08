using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarAtendimentoNAAPASecaoCommandHandler : IRequestHandler<RegistrarAtendimentoNAAPASecaoCommand, EncaminhamentoNAAPASecao>
    {
        private readonly IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public RegistrarAtendimentoNAAPASecaoCommandHandler(IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<EncaminhamentoNAAPASecao> Handle(RegistrarAtendimentoNAAPASecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = MapearParaEntidade(request);
            await repositorioEncaminhamentoNAAPASecao.SalvarAsync(secao);
            return secao;
        }

        private EncaminhamentoNAAPASecao MapearParaEntidade(RegistrarAtendimentoNAAPASecaoCommand request)
            => new ()
            {
                SecaoEncaminhamentoNAAPAId = request.SecaoId,
                Concluido = request.Concluido,
                EncaminhamentoNAAPAId = request.EncaminhamentoNAAPAId
            };
    }
}
