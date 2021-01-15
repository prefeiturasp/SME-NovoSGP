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

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));
            if (!professoresDaTurma?.Any() ?? true)
                throw new NegocioException("Nenhum professor para a turma informada foi encontrada.");

            return await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(turma, professoresDaTurma, dto.ObservacaoId));
        }
    }
}