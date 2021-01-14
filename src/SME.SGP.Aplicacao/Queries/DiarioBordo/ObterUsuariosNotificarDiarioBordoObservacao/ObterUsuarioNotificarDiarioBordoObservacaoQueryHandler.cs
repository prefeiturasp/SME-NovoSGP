using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.DiarioBordo.ObterUsuariosNotificarDiarioBordoObservacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoQueryHandler : IRequestHandler<ObterUsuarioNotificarDiarioBordoObservacaoQuery, IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterUsuarioNotificarDiarioBordoObservacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Handle(ObterUsuarioNotificarDiarioBordoObservacaoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var professores = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(turma.CodigoTurma));
            if(!professores?.Any() ?? true)
                throw new NegocioException("Nenhum professor para a turma informada foi encontrada.");

            var usuariosDiarioDeBordoRf = professores.Select(x => x.ProfessorRf).ToList();
            if (request.ObservacaoId != null)
                await ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(request.ObservacaoId.GetValueOrDefault(), usuariosDiarioDeBordoRf);

            return await 
        }

        private Task ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(long v, List<string> usuariosDiarioDeBordoRf)
        {
            throw new NotImplementedException();
        }
    }
}
