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

    public class ObterOpcoesRespostaPorQuestaoIdQueryHandler : IRequestHandler<ObterOpcoesRespostaPorQuestaoIdQuery, IEnumerable<OpcaoRespostaSimplesDto>>
    {
        private readonly IRepositorioOpcaoResposta repositorioOpcoesResposta;

        public ObterOpcoesRespostaPorQuestaoIdQueryHandler(IRepositorioOpcaoResposta repositorioQuestionario)
        {
            this.repositorioOpcoesResposta = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
        }

        public Task<IEnumerable<OpcaoRespostaSimplesDto>> Handle(ObterOpcoesRespostaPorQuestaoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioOpcoesResposta.ObterOpcoesRespostaPorQuestaoId(request.Id);
        }
    }

}
