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
            
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            
            var planoAula = await mediator.Send(new ObterPlanoAulaEObjetivosAprendizagemQuery(filtro.AulaId));
            
            var planoAulaDto = MapearParaDto(planoAula) ?? new PlanoAulaRetornoDto();

            DisciplinaDto disciplinaDto = null;

            if (filtro.ComponenteCurricularId.HasValue)
            {
                var disciplinasRetorno = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { filtro.ComponenteCurricularId.Value }, codigoTurma: aulaDto.TurmaId));
                disciplinaDto = disciplinasRetorno.FirstOrDefault();
            }

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(aulaDto.TipoCalendarioId, aulaDto.DataAula.Date));

            if (periodoEscolar.EhNulo())
                throw new NegocioException("Período escolar não localizado.");           

            var planejamentoAnualPeriodoId = await mediator.Send(new ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery(filtro.TurmaId, periodoEscolar.Id, disciplinaDto.NaoEhNulo() ? disciplinaDto?.CodigoComponenteCurricular ?? 0 : long.Parse(aulaDto.DisciplinaId)));
            
            if (planejamentoAnualPeriodoId == 0 
                && periodoEscolar.TipoCalendario.AnoLetivo.Equals(DateTime.Now.Year) 
                && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ)
                && !(disciplinaDto != null && disciplinaDto.TerritorioSaber))
                throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");

            var atividadeAvaliativa = await mediator.Send(new ObterAtividadeAvaliativaQuery(aulaDto.DataAula.Date, aulaDto.DisciplinaId, aulaDto.TurmaId, aulaDto.UeId));

            planoAulaDto.PossuiPlanoAnual = planejamentoAnualPeriodoId > 0;
            planoAulaDto.IdAtividadeAvaliativa = atividadeAvaliativa?.Id;
            planoAulaDto.PodeLancarNota = planoAulaDto.IdAtividadeAvaliativa.HasValue && aulaDto.DataAula.Date <= DateTime.Now.Date;
            planoAulaDto.QtdAulas = aulaDto.Quantidade;
            return planoAulaDto;
        }

        private PlanoAulaRetornoDto MapearParaDto(PlanoAulaObjetivosAprendizagemDto plano) =>
            plano.EhNulo() ? null :
            new PlanoAulaRetornoDto()
            {
                Id = plano.Id,
                Descricao = plano.Descricao,
                RecuperacaoAula = plano.RecuperacaoAula,
                LicaoCasa = plano.LicaoCasa,
                AulaId = plano.AulaId,
                QtdAulas = plano.Quantidade,
                AulaCj = plano.AulaCj,
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
