using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioRegistroAcaoDtoQueryHandler : IRequestHandler<ObterSecoesQuestionarioRegistroAcaoDtoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoRegistroAcaoBuscaAtiva repositorioSecaoRegistroAcao;

        public ObterSecoesQuestionarioRegistroAcaoDtoQueryHandler(IRepositorioSecaoRegistroAcaoBuscaAtiva repositorioSecaoRegistroAcao)
        {
            this.repositorioSecaoRegistroAcao = repositorioSecaoRegistroAcao ?? throw new System.ArgumentNullException(nameof(repositorioSecaoRegistroAcao));
        }
        
        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesQuestionarioRegistroAcaoDtoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoRegistroAcao.ObterSecoesQuestionarioDto();
        }
    }
}
