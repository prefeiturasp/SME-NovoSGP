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

        public async Task<PlanoAulaRetornoDto> Executar(long aulaId)
        {
            PlanoAulaRetornoDto planoAulaDto = new PlanoAulaRetornoDto();

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var planoAula = await mediator.Send(new ObterPlanoAulaEObjetivosAprendizagemQuery(aulaId));

            if (planoAula == null)
                throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");

            var atividadeAvaliativa = await mediator.Send(new ObterAtividadeAvaliativaQuery(planoAula.AulaConsultaSimples.DataAula.Date, planoAula.AulaConsultaSimples.DisciplinaId, planoAula.AulaConsultaSimples.TurmaId, planoAula.AulaConsultaSimples.UeId));

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(long.Parse(planoAula.AulaConsultaSimples.DisciplinaId), planoAula.AulaConsultaSimples.DataAula.Date));

            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não localizado.");

            var planoAnualDto = await mediator.Send(new ObterPlanejamentoAnualSimplificadoPorTurmaQuery( long.Parse(planoAula.AulaConsultaSimples.TurmaId)));

            if (planoAnualDto == null && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ))
                throw new NegocioException("Não foi possível carregar o plano de aula porque não há plano anual cadastrado");


            planoAulaDto.PossuiPlanoAnual = planoAnualDto.Id > 0;
            planoAulaDto.ObjetivosAprendizagemOpcionais = false;
            planoAulaDto.AulaId = planoAula.AulaConsultaSimples.Id;
            planoAulaDto.QtdAulas = planoAula.AulaConsultaSimples.Quantidade;
            planoAulaDto.IdAtividadeAvaliativa = atividadeAvaliativa?.Id;
            planoAulaDto.PodeLancarNota = planoAulaDto.IdAtividadeAvaliativa.HasValue && planoAula.AulaConsultaSimples.DataAula.Date <= DateTime.Now.Date;
            return planoAulaDto;
        }

    }
}
