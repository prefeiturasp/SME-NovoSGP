using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class RegistrarMapeamentoEstudanteSecaoCommandHandler : IRequestHandler<RegistrarMapeamentoEstudanteSecaoCommand, MapeamentoEstudanteSecao>
    {
        private readonly IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao;

        public RegistrarMapeamentoEstudanteSecaoCommandHandler(IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao)
        {
            this.repositorioMapeamentoSecao = repositorioMapeamentoSecao ?? throw new ArgumentNullException(nameof(repositorioMapeamentoSecao));
        }

        public async Task<MapeamentoEstudanteSecao> Handle(RegistrarMapeamentoEstudanteSecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = MapearParaEntidade(request);
            await repositorioMapeamentoSecao.SalvarAsync(secao);
            return secao;
        }

        private MapeamentoEstudanteSecao MapearParaEntidade(RegistrarMapeamentoEstudanteSecaoCommand request)
            => new ()
            {
                SecaoMapeamentoEstudanteId = request.SecaoId,
                Concluido = request.Concluido,
                MapeamentoEstudanteId = request.MapeamentoEstudanteId
            };
    }
}
