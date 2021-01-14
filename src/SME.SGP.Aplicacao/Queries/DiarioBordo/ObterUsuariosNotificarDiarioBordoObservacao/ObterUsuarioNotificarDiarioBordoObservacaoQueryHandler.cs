using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IMediator mediator;

        public ObterUsuarioNotificarDiarioBordoObservacaoQueryHandler(IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao, IMediator mediator)
        {
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao;
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

            return request.ObservacaoId != null
                ? await ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(request.ObservacaoId.GetValueOrDefault(), professores)
                : await ObterUsuariosDosProfessoresDaTurmaAsync(professores);
        }

        private async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> ObterUsuariosDosProfessoresDaTurmaAsync(IEnumerable<ProfessorTitularDisciplinaEol> professores)
        {
            var professoresRf = professores.Select(x => x.ProfessorRf).ToList();
            var usuarios = await mediator.Send(new ObterUsuariosPorCodigosRfQuery(professoresRf));
            if(!usuarios?.Any () ?? true)
                throw new NegocioException("Os usuários dos professores da turma não foram encontrados.");

            return professores
                .Select(x => new UsuarioNotificarDiarioBordoObservacaoDto
                {
                    Nome = $"{x.ProfessorNome} ({x.ProfessorRf})",
                    PodeRemover = false,
                    UsuarioId = usuarios.FirstOrDefault(y => y.CodigoRf == x.ProfessorRf).Id
                })
                .ToList();
        }

        private Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(long observacaoId, IEnumerable<ProfessorTitularDisciplinaEol> professores)
        {
            var usuarios = await repositorioDiarioBordoObservacaoNotificacao.ObterObservacaoPorId
        }
    }
}
