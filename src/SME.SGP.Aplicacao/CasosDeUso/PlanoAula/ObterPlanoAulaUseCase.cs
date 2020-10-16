using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAulaUseCase : AbstractUseCase, IObterPlanoAulaUseCase
    {
        public ObterPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PlanoAulaRetornoDto> Executar(FiltroObterPlanoAulaDto filtro)
        {
            var aulaDto = await mediator.Send(new ObterAulaPorIdQuery(filtro.AulaId));

            var planoAula = await mediator.Send(new ObterPlanoAulaEObjetivosAprendizagemQuery(filtro.AulaId));
            var planoAulaDto = MapearParaDto(planoAula) ?? new PlanoAulaRetornoDto();

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(aulaDto.TipoCalendarioId, aulaDto.DataAula.Date));
            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não localizado.");

            var planejamentoAnualPeriodoId = await mediator.Send(new ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(filtro.TurmaId, periodoEscolar.Id, long.Parse(aulaDto.DisciplinaId)));
            if (planejamentoAnualPeriodoId == 0)
                throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");

            var atividadeAvaliativa = await mediator.Send(new ObterAtividadeAvaliativaQuery(aulaDto.DataAula.Date, aulaDto.DisciplinaId, aulaDto.TurmaId, aulaDto.UeId));

            planoAulaDto.PossuiPlanoAnual = planejamentoAnualPeriodoId > 0;
            planoAulaDto.IdAtividadeAvaliativa = atividadeAvaliativa?.Id;
            planoAulaDto.PodeLancarNota = planoAulaDto.IdAtividadeAvaliativa.HasValue && aulaDto.DataAula.Date <= DateTime.Now.Date;
            return planoAulaDto;
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAulaObjetivosAprendizagemDto plano) =>
            plano == null ? null :
            new PlanoAulaRetornoDto()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
                DesenvolvimentoAula = plano.DesenvolvimentoAula,
                RecuperacaoAula = plano.RecuperacaoAula,
                LicaoCasa = plano.LicaoCasa,
                AulaId = plano.AulaId,
                QtdAulas = plano.Quantidade,

                Migrado = plano.Migrado,
                CriadoEm = plano.CriadoEm,
                CriadoPor = plano.CriadoPor,
                CriadoRf = plano.CriadoRf,
                AlteradoEm = plano.AlteradoEm,
                AlteradoPor = plano.AlteradoPor,
                AlteradoRf = plano.AlteradoRf,

                ObjetivosAprendizagemAula = plano.ObjetivosAprendizagemAula
            };
    }
}
