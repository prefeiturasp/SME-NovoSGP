using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarAprovacaoPainelEducacionalUseCase : ConsolidacaoBaseUseCase, IConsolidarAprovacaoPainelEducacionalUseCase
    {
        public ConsolidarAprovacaoPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            // obter Dres
            var listagensDres = await mediator.Send(new ObterTodasDresQuery());
            // obter ues
            var listagemUe = (await mediator.Send(new ObterTodasUesQuery()))?.ToList();

            foreach (var dre in listagensDres)
            {
                var uesDaDre = FiltrarUesValidasParaConsolidacao(dre.Id, listagemUe);

                // obter turmas da modalidade Ensino médio e ensino fundamental
                var listagemTurmas = await mediator.Send(
                    new ObterTurmasComModalidadePorModalidadeAnoQuery(
                        DateTime.Now.Year,
                        uesDaDre.Select(u => u.Id)?.ToList(),
                        ModalidadesTurmas.Select(m => (int)m)
                    )
                );
                
                // obter indicadores das turmas para consolidar
                var dadosConsolidados = await mediator.Send(new ObterAprovacaoParaConsolidacaoQuery(listagemTurmas.Select(t => t.TurmaId).ToArray()));
            }

           
            // agrupar por ano da turma
            // agrupar por turma
            // salvar a consolidação por ano turma
            // salvaar consolidação por turma

            //var filtro = param.ObterObjetoMensagem<FiltroConsolidacaoProficienciaIdepDto>();

            //var dadosConsolidados = await mediator.Send(new ObterProficienciaIdepParaConsolidacaoQuery(filtro.AnoLetivo));

            //await mediator.Send(new SalvarConsolidacaoProficienciaIdepCommand(dadosConsolidados.ToList()));
            return true;
        }
    }
}
