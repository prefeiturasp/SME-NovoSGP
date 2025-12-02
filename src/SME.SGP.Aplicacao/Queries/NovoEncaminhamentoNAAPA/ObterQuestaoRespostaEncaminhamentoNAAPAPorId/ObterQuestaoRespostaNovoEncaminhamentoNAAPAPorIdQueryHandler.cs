using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterQuestaoRespostaEncaminhamentoNAAPAPorId
{
    public class ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery, IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>>
    {
        public IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA { get; }

        public ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioQuestaoNovoEncaminhamentoNAAPA repositorioQuestaoNovoEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioQuestaoNovoEncaminhamentoNAAPA = repositorioQuestaoNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoNovoEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> Handle(ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoNovoEncaminhamentoNAAPA.ObterRespostasEncaminhamento(request.Id);
        }
    }
}