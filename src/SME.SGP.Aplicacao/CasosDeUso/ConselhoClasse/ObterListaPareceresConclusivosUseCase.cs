using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaPareceresConclusivosUseCase : AbstractUseCase, IObterListaPareceresConclusivosUseCase
    {
        public ObterListaPareceresConclusivosUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<IEnumerable<ParecerConclusivoDto>> Executar(string turmaCodigo)
        {
            var listaPareceres = new List<ParecerConclusivoDto>() { new ParecerConclusivoDto() { Id = 0, Nome = "Todos" } };

            listaPareceres.AddRange(await mediator.Send(new ObterPareceresConclusivosPorTurmaQuery(turmaCodigo, DateTime.Today)));

            return listaPareceres;
        }

    }
}
