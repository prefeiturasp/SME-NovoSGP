using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartasDeIntencoesPorTurmaEComponenteUseCase : AbstractUseCase, IObterCartasDeIntencoesPorTurmaEComponenteUseCase
    {
        public ObterCartasDeIntencoesPorTurmaEComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<CartaIntencoesRetornoDto>> Executar(ObterCartaIntencoesDto param)
        {
            var cartas = await mediator.Send(new ObterCartaDeIntencoesPorTurmaEComponenteQuery(param.TurmaCodigo, param.ComponenteCurricularId));
            return await CarregarDto(param.TurmaCodigo, cartas, param.ComponenteCurricularId);
        }

        private async Task<long> ObterTipoCalendarioId(Turma turma)
        {
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == 0)
                throw new NegocioException("Tipo de calendário não localizado para a turma");

            return tipoCalendarioId;
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de código [{turmaCodigo}] não localizada");

            return turma;
        }

        private async Task<IEnumerable<CartaIntencoesRetornoDto>> CarregarDto(string turmaCodigo, IEnumerable<CartaIntencoes> cartas, long componenteCurricularId)
        {
            var turma = await ObterTurma(turmaCodigo);
            var tipoCalendarioId = await ObterTipoCalendarioId(turma);

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            return await MapearParaDto(periodosEscolares, cartas, turma, componenteCurricularId);
        }

        private async Task<IEnumerable<CartaIntencoesRetornoDto>> MapearParaDto(IEnumerable<PeriodoEscolar> periodosEscolares, IEnumerable<CartaIntencoes> cartas, Turma turma, long componenteCurricularId)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var listaCartasDto = new List<CartaIntencoesRetornoDto>();
            var usuarioTemAtribuicao = false;

            foreach (var periodoEscolar in periodosEscolares.OrderBy(x => x.Bimestre))
            {
                var carta = cartas?.FirstOrDefault(a => a.PeriodoEscolarId == periodoEscolar.Id);

                if(!usuarioLogado.EhProfessorCj())
                    usuarioTemAtribuicao = await UsuarioTemAtribuicao(usuarioLogado, turma.CodigoTurma, componenteCurricularId, periodoEscolar);
              
                listaCartasDto.Add(new CartaIntencoesRetornoDto()
                {
                    Id = carta?.Id ?? 0,
                    Planejamento = carta?.Planejamento,
                    PeriodoEscolarId = periodoEscolar.Id,
                    Bimestre = periodoEscolar.Bimestre,
                    PeriodoAberto = await TurmaEmPeridoAberto(turma, periodoEscolar.Bimestre),
                    UsuarioTemAtribuicao = usuarioTemAtribuicao || usuarioLogado.EhAbrangenciaUEECP() || usuarioLogado.EhProfessorCj(),
                    Auditoria = (AuditoriaDto)carta
                });
            }

            return listaCartasDto;
        }

        private async Task<bool> UsuarioTemAtribuicao(Usuario usuario, string turmaCodigo, long componenteCurricularId, PeriodoEscolar periodoEscolar)
        {
            var validacao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(componenteCurricularId, turmaCodigo, usuario.CodigoRf, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim));
            return validacao;
        }

        private async Task<bool> TurmaEmPeridoAberto(Turma turma, int bimestre)
        {
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, true));
        }
    }
}
