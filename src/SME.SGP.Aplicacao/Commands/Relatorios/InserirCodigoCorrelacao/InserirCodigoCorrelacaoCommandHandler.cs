using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirCodigoCorrelacaoCommandHandler : IRequestHandler<InserirCodigoCorrelacaoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;

        public InserirCodigoCorrelacaoCommandHandler(IMediator mediator, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio ?? throw new ArgumentNullException(nameof(repositorioCorrelacaoRelatorio));
        }
        public async Task<bool> Handle(InserirCodigoCorrelacaoCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(request.UsuarioRf));

            if (usuario == null)
                throw new NegocioException("Usuário não encontrado!");

            var novoRelatorioCorrelacao = new RelatorioCorrelacao()
            {
                Codigo = request.CodigoCorrelacao,
                TipoRelatorio = request.TipoRelatorio,
                UsuarioSolicitanteId = usuario.Id,
                Formato = request.Formato
            };

            await repositorioCorrelacaoRelatorio.SalvarAsync(novoRelatorioCorrelacao);

            return await Task.FromResult(true);
        }
    }
}
