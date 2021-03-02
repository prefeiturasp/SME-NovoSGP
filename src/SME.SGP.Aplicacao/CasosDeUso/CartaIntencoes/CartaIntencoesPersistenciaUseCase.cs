using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class CartaIntencoesPersistenciaUseCase : AbstractUseCase, ICartaIntencoesPersistenciaUseCase
    {
        public CartaIntencoesPersistenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<List<CartaIntencoesRetornoPersistenciaDto>> Executar(CartaIntencoesPersistenciaDto param)
        {
            List<CartaIntencoesRetornoPersistenciaDto> auditorias = new List<CartaIntencoesRetornoPersistenciaDto>();

            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(param.CodigoTurma));

            if (turmaId == 0)
            {
                throw new NegocioException("Não foi possível encontrar a turma.");
            }

            var aberto = await AulaDentroDoPeriodo(mediator, param.CodigoTurma, DateTime.Now);
            if (!aberto)
            {
                throw new NegocioException("Não é possível salvar carta de intenções pois o período não está aberto.");
            }

            AuditoriaDto auditoria = null;

            foreach (CartaIntencoesDto carta in param.Cartas)
            {

                if (carta.Id == 0)
                {
                    auditoria = await mediator.Send(new InserirCartaIntencoesCommand(carta, turmaId, param.ComponenteCurricularId));
                }
                else
                {
                    var existente = await mediator.Send(new ObterCartaIntentocesPorIdQuery(carta.Id));

                    auditoria = await mediator.Send(new AlterarCartaIntencoesCommand(carta, existente, turmaId, param.ComponenteCurricularId));
                }

                auditorias.Add(new CartaIntencoesRetornoPersistenciaDto
                {
                    PeriodoEscolarId = carta.PeriodoEscolarId,
                    Auditoria = auditoria
                });
            }

            return auditorias;
        }

        private static async Task<bool> AulaDentroDoPeriodo(IMediator mediator, string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(dataAula,turma));

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }
    }
}
