using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosNotificarCartaIntencoesObservacaoQueryHandler : IRequestHandler<ObterUsuariosNotificarCartaIntencoesObservacaoQuery, IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterUsuariosNotificarCartaIntencoesObservacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Handle(ObterUsuariosNotificarCartaIntencoesObservacaoQuery request, CancellationToken cancellationToken)
            => await ObterUsuariosDosProfessoresDaTurmaAsync(request.ProfessoresDaTurma);

        private async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> ObterUsuariosDosProfessoresDaTurmaAsync(IEnumerable<ProfessorTitularDisciplinaEol> professores)
        {
            if (professores != null && professores.Any())
            {
                var professoresRf = professores.Select(x => x.ProfessorRf).ToList();
                var usuarios = await mediator.Send(new ObterUsuariosPorCodigosRfQuery(professoresRf));
                if (usuarios != null && usuarios.Any())
                {
                    var retorno = professores
                        .Select(x => new UsuarioNotificarCartaIntencoesObservacaoDto
                        {
                            Nome = $"{x.ProfessorNome} ({x.ProfessorRf})",
                            PodeRemover = false,
                            UsuarioId = usuarios.FirstOrDefault(y => y.CodigoRf == x.ProfessorRf).Id
                        })
                        .ToList();
                    return retorno;
                }
            }

            return default;
        }
    }
}
