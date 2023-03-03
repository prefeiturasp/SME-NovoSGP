using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.HistoricoEscolar
{
    public class ObterHistoricoEscolarObservacaoUseCase : IObterHistoricoEscolarObservacaoUseCase
    {
        private readonly IMediator mediator;

        public ObterHistoricoEscolarObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<HistoricoEscolarObservacaoDto> Executar(string alunoCodigo)
        {
            return await mediator.Send(new ObterHistoricoEscolarObservacaoPorAlunoQuery(alunoCodigo));
        }
    }
}
