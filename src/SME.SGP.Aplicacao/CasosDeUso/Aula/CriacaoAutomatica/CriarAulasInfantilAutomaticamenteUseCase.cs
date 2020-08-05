using MediatR;
using SME.SGP.Aplicacao.Queries.Aula.ObterAulasDaTurma;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteUseCase : ICriarAulasInfantilAutomaticamenteUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Executar()
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Infantil, anoAtual, null));
            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {

                    var turmas = await mediator.Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual));
                    if (turmas != null && turmas.Any())
                    {
                        foreach (var turma in turmas)
                        {
                            var aulas = await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId));
                            if (aulas == null)
                            {
                                //TODO Criar todas aulas da turma
                            }
                            else
                            {
                                if (!aulas.Any())
                                {
                                    //TODO Criar todas aulas da turma
                                }
                                //TODO validar aulas que devem ser criadas e aulas que devem ser excluidas
                            }
                        }
                    }
                }
            }
        }
    }
}
