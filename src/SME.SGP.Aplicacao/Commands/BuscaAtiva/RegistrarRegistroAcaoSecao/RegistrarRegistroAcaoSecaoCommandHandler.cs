using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarRegistroAcaoSecaoCommandHandler : IRequestHandler<RegistrarRegistroAcaoSecaoCommand, RegistroAcaoBuscaAtivaSecao>
    {
        private readonly IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao;

        public RegistrarRegistroAcaoSecaoCommandHandler(IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao)
        {
            this.repositorioRegistroAcaoSecao = repositorioRegistroAcaoSecao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcaoSecao));
        }

        public async Task<RegistroAcaoBuscaAtivaSecao> Handle(RegistrarRegistroAcaoSecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = MapearParaEntidade(request);
            await repositorioRegistroAcaoSecao.SalvarAsync(secao);
            return secao;
        }

        private RegistroAcaoBuscaAtivaSecao MapearParaEntidade(RegistrarRegistroAcaoSecaoCommand request)
            => new ()
            {
                SecaoRegistroAcaoBuscaAtivaId = request.SecaoId,
                Concluido = request.Concluido,
                RegistroAcaoBuscaAtivaId = request.RegistroAcaoId
            };
    }
}
