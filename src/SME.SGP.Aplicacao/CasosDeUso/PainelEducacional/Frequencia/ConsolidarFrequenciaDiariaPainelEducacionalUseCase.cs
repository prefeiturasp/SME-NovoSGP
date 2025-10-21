using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia
{
    public class ConsolidarFrequenciaDiariaPainelEducacionalUseCase : AbstractUseCase, IConsolidarFrequenciaDiariaPainelEducacionalUseCase
    {
        public ConsolidarFrequenciaDiariaPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {

            var listagemFrequencia = await mediator.Send(new ObterFrequenciaDiariaQuery(2024));



            var frequencia = new List<RegistroFrequenciaPorDisciplinaAlunoDto>();
            var listagemDre = await mediator.Send(new ObterTodasDresQuery());
            foreach (var dre in listagemDre)
            {
                var listagemUe = await mediator.Send(new ObterUesCodigosPorDreQuery(dre.Id));
                var listagemTurmas = await mediator.Send(new ObterTurmasPainelEducacionalQuery(DateTime.Now.Year));
                foreach (var ue in listagemUe)
                {
                    var turmasDaUe = listagemTurmas.Where(t => t.CodigoUe == ue).ToList();

                    var turmasFiltro = turmasDaUe.Select(x => x.TurmaId).ToList();

                    turmasFiltro.Add("2853538");
                    //frequencia.AddRange(await mediator.Send(new ObterFrequenciaDiariaQuery(turmasFiltro)));
                }

            }

            return true;
        }
    }
}
