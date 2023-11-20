using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroAcaoComTurmaPorIdQueryHandler : IRequestHandler<ObterRegistroAcaoComTurmaPorIdQuery, RegistroAcaoBuscaAtiva>
    {
        public IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao { get; }

        public ObterRegistroAcaoComTurmaPorIdQueryHandler(IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao)
        {
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
        }

        public async Task<RegistroAcaoBuscaAtiva> Handle(ObterRegistroAcaoComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioRegistroAcao.ObterRegistroAcaoComTurmaPorId(request.RegistroAcaoId);
    }
}
