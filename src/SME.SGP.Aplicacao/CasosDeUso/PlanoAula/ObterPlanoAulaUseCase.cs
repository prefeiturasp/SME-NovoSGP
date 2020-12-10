using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
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
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var planoAula = await mediator.Send(new ObterPlanoAulaEObjetivosAprendizagemQuery(filtro.AulaId));            
            var planoAulaDto = MapearParaDto(planoAula) ?? new PlanoAulaRetornoDto();

            DisciplinaDto disciplinaDto = null;

            if (filtro.ComponenteCurricularId.HasValue)
            {
                var disciplinasRetorno = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { filtro.ComponenteCurricularId.Value }));
                disciplinaDto = disciplinasRetorno.SingleOrDefault();
            }

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(aulaDto.TipoCalendarioId, aulaDto.DataAula.Date));

            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não localizado.");           

            var planejamentoAnualPeriodoId = await mediator.Send(new ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(filtro.TurmaId, periodoEscolar.Id, disciplinaDto != null ? disciplinaDto.Id : long.Parse(aulaDto.DisciplinaId)));
            
            if (planejamentoAnualPeriodoId == 0 && periodoEscolar.TipoCalendario.AnoLetivo.Equals(DateTime.Now.Year) && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ) && !(disciplinaDto != null && disciplinaDto.TerritorioSaber))
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

                ObjetivosAprendizagemComponente = plano.ObjetivosAprendizagemComponente
            };
    }
}
