using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesNotificacaoQueryHandler : IRequestHandler<ObterCartaIntencoesNotificacaoQuery, IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterCartaIntencoesNotificacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Handle(ObterCartaIntencoesNotificacaoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var professorRf = string.Empty;
            var professorNome = string.Empty;

            if (request.CartaIntencoesObservacaoId > 0)
            {
                var CartaIntencoes = await mediator.Send(new ObterCartaIntentocesPorIdQuery(request.CartaIntencoesObservacaoId));

                if (CartaIntencoes is null)
                    throw new NegocioException("A carta de intenções informada não foi encontrada.");

                professorRf = CartaIntencoes.CriadoRF;
                professorNome = CartaIntencoes.CriadoPor;
            }
            else if (request.ObservacaoId.HasValue)
            {
                var CartaIntencoesObs = await mediator.Send(new ObterCartaIntencoesObservacaoPorObservacaoIdQuery(request.ObservacaoId.Value));

                if (CartaIntencoesObs is null)
                    throw new NegocioException("A carta de intenções observação informada não foi encontrada.");

                professorRf = CartaIntencoesObs.Auditoria.CriadoRF;
                professorNome = CartaIntencoesObs.Auditoria.CriadoPor;
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (!professorRf.Equals(usuarioLogado.CodigoRf))
                return await mediator.Send(new ObterUsuariosNotificarCartaIntencoesObservacaoQuery(ObterProfessorTitular(professorRf, professorNome)));

            else
                return default;
        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(string codigoRf, string nome)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = codigoRf, ProfessorNome = nome } };
        }
    }
}
