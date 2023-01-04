using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSrmPaeeColaborativoUseCase : IObterSrmPaeeColaborativoUseCase
    {
        public ObterSrmPaeeColaborativoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private readonly IMediator mediator;
        public async Task<IEnumerable<SrmPaeeColaborativoSgpDto>> Executar(FiltroSrmPaeeColaborativoDto param)
        {
            var dados = new List<SrmPaeeColaborativoSgpDto>();
            var dadosSrmEol = (await mediator.Send(new ObterDadosSrmPaeeColaborativoEolQuery(param.CodigoAluno))).ToList();


            if(dadosSrmEol.Any())
               await MontarDados(dadosSrmEol,dados);
            
            return dados;
        }

        private async Task MontarDados(List<DadosSrmPaeeColaborativoEolDto> dadosSrmPaeeColaborativoEolDtos, List<SrmPaeeColaborativoSgpDto> srmPaeeColaborativoSgpDtos)
        {
            var idsTurmas = dadosSrmPaeeColaborativoEolDtos.Select(x => x.CodigoTurma).ToArray();
            var turmas = (await mediator.Send(new ObterTurmasPorIdsQuery(idsTurmas))).ToList();

            var idsUes = dadosSrmPaeeColaborativoEolDtos.Select(x => x.CodigoEscola).ToArray();
            var ues = (await mediator.Send(new ObterUesComDrePorCodigoUesQuery(idsUes))).ToList();

            foreach (var dadoEol in dadosSrmPaeeColaborativoEolDtos)
            {
                var dados = new SrmPaeeColaborativoSgpDto
                {
                    TurmaTurno = turmas.FirstOrDefault(x => x.CodigoTurma == dadoEol.CodigoTurma.ToString())?.NomeFiltro + " - " + dadoEol.Turno,
                    ComponenteCurricular = dadoEol.Componente,
                    DreUe = ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)!.Dre.Abreviacao + " - "+ ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)?.TipoEscola.ShortName() + " "
                            + ues.FirstOrDefault(x => x.CodigoUe == dadoEol.CodigoEscola)?.Nome
                };
                
                srmPaeeColaborativoSgpDtos.Add(dados);
            }
        }


    }
}