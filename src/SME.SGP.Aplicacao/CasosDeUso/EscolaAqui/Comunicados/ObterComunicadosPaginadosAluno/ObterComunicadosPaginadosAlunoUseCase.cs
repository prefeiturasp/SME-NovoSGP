using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosAlunoUseCase : AbstractUseCase, IObterComunicadosPaginadosAlunoUseCase
    {
        public ObterComunicadosPaginadosAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> Executar(FiltroTurmaAlunoSemestreDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(dto.TurmaId));
            if (turma == null)
                throw new NegocioException("A Turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(dto.AlunoCodigo.ToString(), turma.AnoLetivo));
            if (aluno == null)
                throw new NegocioException("O Aluno informado não foi encontrado");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares == null)
                throw new NegocioException("Não foi possível encontrar o período escolar da turma.");

            var periodo = new PeriodoOcorrenciaPorAlunoDto();

            if (dto.Semestre == 1)
            {
                periodo.DataInicio = periodosEscolares.First(a => a.Bimestre == 1).PeriodoInicio;
                periodo.DataFim = periodosEscolares.First(a => a.Bimestre == 2).PeriodoFim;  
            }
            else
            {
                periodo.DataInicio = periodosEscolares.First(a => a.Bimestre == 3).PeriodoInicio;
                periodo.DataFim = periodosEscolares.First(a => a.Bimestre == 4).PeriodoFim;
            }

            return await mediator.Send(new ListarComunicadosPaginadosQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, dto.AlunoCodigo.ToString()));

        }
    }
}
