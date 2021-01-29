using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoUseCase : AbstractUseCase, IObterUsuarioNotificarDiarioBordoObservacaoUseCase
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Executar(ObterUsuarioNotificarDiarioBordoObservacaoDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var professoresTurmaObrigatoriosReceberNotificacao = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));
            if (!professoresTurmaObrigatoriosReceberNotificacao?.Any() ?? true)
                throw new NegocioException("Nenhum professor para a turma informada foi encontrada.");

            // Caso um dos professores da turma for o usuário logado e for uma nova observação, ele não deve aparecer na listagem
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            if (usuarioLogado != null && dto.ObservacaoId is null && professoresTurmaObrigatoriosReceberNotificacao.Any(x => x.ProfessorRf == usuarioLogado.CodigoRf))
            {
                professoresTurmaObrigatoriosReceberNotificacao = professoresTurmaObrigatoriosReceberNotificacao.Where(x => x.ProfessorRf != usuarioLogado.CodigoRf).ToList();
            }

            return await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(turma, professoresTurmaObrigatoriosReceberNotificacao, dto.ObservacaoId));
        }
    }
}