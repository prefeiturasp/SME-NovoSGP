using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
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
            => request.ObservacaoId != null
                ? await ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(request.ObservacaoId.GetValueOrDefault(), request.ProfessoresDaTurma)
                : await ObterUsuariosDosProfessoresDaTurmaAsync(request.ProfessoresDaTurma);

        private async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> ObterUsuariosDosProfessoresDaTurmaAsync(IEnumerable<ProfessorTitularDisciplinaEol> professores)
        {
            if (professores != null && professores.Any())
            {
                var professoresRf = professores.Select(x => x.ProfessorRf).ToList();
                var usuarios = await mediator.Send(new ObterUsuariosPorCodigosRfQuery(professoresRf));
                if (usuarios != null && usuarios.Any())
                {
                    return professores
                        .Select(x => new UsuarioNotificarDiarioBordoObservacaoDto
                        {
                            Nome = $"{x.ProfessorNome} ({x.ProfessorRf})",
                            PodeRemover = false,
                            UsuarioId = usuarios.FirstOrDefault(y => y.CodigoRf == x.ProfessorRf).Id
                        })
                        .ToList();
                }
            }

            return default;
        }

        private async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> ObterUsuariosAdicionadosNaObservacaoParaSeremNotificadosAsync(long observacaoId, IEnumerable<ProfessorTitularDisciplinaEol> professores)
        {
            var usuariosNotificacao = await repositorioDiarioBordoObservacaoNotificacao.ObterUsuariosIdNotificadosPorObservacaoId(observacaoId);
            if (!usuariosNotificacao?.Any() ?? true)
                return await ObterUsuariosDosProfessoresDaTurmaAsync(professores);

            var usuariosNotificacaoEol = await mediator.Send(new ObterListaNomePorListaRFQuery(usuariosNotificacao.Select(x => x.CodigoRf)));
            if (!usuariosNotificacaoEol?.Any() ?? true)
                throw new NegocioException("Os usuários das notificações enviadas não foram encontrados.");

            return usuariosNotificacaoEol
                .Select(x => new UsuarioNotificarDiarioBordoObservacaoDto
                {
                    Nome = $"{x.Nome} ({x.CodigoRF})",
                    PodeRemover = !professores.Any(y => y.ProfessorRf == x.CodigoRF),
                    UsuarioId = usuariosNotificacao.FirstOrDefault(y => y.CodigoRf == x.CodigoRF).Id
                })
                .OrderBy(x => x.PodeRemover)
                .ToList();
        }
    }
}