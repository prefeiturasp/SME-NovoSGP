using MediatR;
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
        private readonly IMediator mediator;

        public ObterUsuarioNotificarDiarioBordoObservacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Handle(ObterUsuarioNotificarDiarioBordoObservacaoQuery request, CancellationToken cancellationToken)
            => await ObterUsuariosDosProfessoresDaTurmaAsync(request.ProfessoresDaTurma);

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
    }
}