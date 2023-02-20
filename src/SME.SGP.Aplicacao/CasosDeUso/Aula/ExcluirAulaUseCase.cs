using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUseCase : AbstractUseCase, IExcluirAulaUseCase
    {
        public ExcluirAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(ExcluirAulaDto excluirDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var aula = await mediator.Send(new ObterAulaPorIdQuery(excluirDto.AulaId));

            if (aula == null)
                throw new NegocioException($"Não foi possível localizar a aula de id : {excluirDto.AulaId}");

            var componentesProfessor = await mediator
                .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(aula.TurmaId, usuarioLogado.Login, usuarioLogado.PerfilAtual, usuarioLogado.EhProfessorInfantilOuCjInfantil()));

            var componenteCorrespondente = componentesProfessor
                .SingleOrDefault(cp => cp.Codigo.ToString().Equals(aula.DisciplinaId) || cp.CodigoComponenteTerritorioSaber.ToString().Equals(aula.DisciplinaId));

            var componenteCurricularNome = componenteCorrespondente != null && componenteCorrespondente.TerritorioSaber ? 
                componenteCorrespondente.Descricao : await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(long.Parse(aula.DisciplinaId)));

            if (excluirDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                return await mediator.Send(new ExcluirAulaUnicaCommand(usuarioLogado,
                                                                       excluirDto.AulaId,
                                                                       componenteCurricularNome));
            }
            else
            {
                try
                {
                    // TODO alterar para fila do RabbitMQ
                    await mediator.Send(new IncluirFilaExclusaoAulaRecorrenteCommand(excluirDto.AulaId,
                                                                                     excluirDto.RecorrenciaAula,
                                                                                     componenteCurricularNome,
                                                                                     usuarioLogado));

                    return new RetornoBaseDto("Serão excluidas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Exclusão de aulas recorrentes", LogNivel.Critico, LogContexto.Aula, ex.Message));
                }

                return new RetornoBaseDto("Ocorreu um erro ao solicitar a exclusão de aulas recorrentes, por favor tente novamente.");
            }
        }
    }
}
