using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma
{
    public class ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase : IObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DadosLeituraAlunosComunicadoPorTurmaDto>> Executar(FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto request)
        {
            return await mediator.Send(new ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery(request.ComunicadoId, request.CodigoTurma));
        }
    }
}