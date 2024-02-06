using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaRegistroAcaoPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaRegistroAcaoPorIdQuery, RespostaRegistroAcaoBuscaAtiva>
    {
        public IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioResposta { get; }

        public ObterRespostaRegistroAcaoPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaRegistroAcaoBuscaAtiva repositorioResposta) : base(contextoAplicacao)
        {
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<RespostaRegistroAcaoBuscaAtiva> Handle(ObterRespostaRegistroAcaoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioResposta.ObterPorIdAsync(request.Id);
        }
    }
}
