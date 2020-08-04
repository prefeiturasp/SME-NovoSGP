using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Aula.CriacaoAutomatica
{
    public class CriarAulasInfantilAutomaticamenteUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Executar()
        {
            var anoAtual = DateTime.Now.Year;
            var idTipoCalendario = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Infantil, anoAtual, null));
            if (idTipoCalendario > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(idTipoCalendario));
                if(periodosEscolares!=null && periodosEscolares.Any())
                {
                    
                    var turmas = await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));
                    if(turmas!=null && turmas.Any())
                    {
                        var aulas = await mediator.Send();
                    }
                }
            }
        }
    }
}
