using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public class ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQueryHandler : IRequestHandler<ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery, IEnumerable<OpcaoRespostaSimplesDto>>
    {
        private readonly IRepositorioOpcaoResposta repositorioOpcoesResposta;

        public ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQueryHandler(IRepositorioOpcaoResposta repositorioQuestionario)
        {
            this.repositorioOpcoesResposta = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
        }

        public Task<IEnumerable<OpcaoRespostaSimplesDto>> Handle(ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery request, CancellationToken cancellationToken)
        {
            return repositorioOpcoesResposta.ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionario(request.NomeComponente, request.TipoQuestionario);
        }
    }

}
