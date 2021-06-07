using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoFrequencia : IServicoCalculoFrequencia
    {
        private readonly IMediator mediator;

        public ServicoCalculoFrequencia(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId)
        {
            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, dataAula, turmaId, disciplinaId));
        }
    }
}