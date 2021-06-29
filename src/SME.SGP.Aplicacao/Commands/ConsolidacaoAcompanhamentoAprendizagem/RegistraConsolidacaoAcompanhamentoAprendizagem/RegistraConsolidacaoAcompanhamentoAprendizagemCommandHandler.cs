using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoAcompanhamentoAprendizagemCommandHandler : IRequestHandler<RegistraConsolidacaoAcompanhamentoAprendizagemCommand, long>
    {
        private readonly IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio;

        public RegistraConsolidacaoAcompanhamentoAprendizagemCommandHandler(IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(RegistraConsolidacaoAcompanhamentoAprendizagemCommand request, CancellationToken cancellationToken)
        {
            var consolidacao = new ConsolidacaoAcompanhamentoAprendizagemAluno(request.TurmaId, request.QuantidadeComAcompanhamento, request.QuantidadeSemAcompanhamento, request.Semestre);
            return await repositorio.Inserir(consolidacao);
        }
    }
}
