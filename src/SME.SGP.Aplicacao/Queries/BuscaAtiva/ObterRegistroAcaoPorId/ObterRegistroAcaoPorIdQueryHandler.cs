using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroAcaoPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRegistroAcaoPorIdQuery, RegistroAcaoBuscaAtiva>
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao;
        public ObterRegistroAcaoPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao) : base(contextoAplicacao)
        {
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
        }

        public async Task<RegistroAcaoBuscaAtiva> Handle(ObterRegistroAcaoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroAcao.ObterRegistroAcaoPorId(request.Id);
        }
    }
}
