using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaDevolucaoPlanoAEECommandHandler : IRequestHandler<GerarPendenciaDevolucaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public GerarPendenciaDevolucaoPlanoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(GerarPendenciaDevolucaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("Não foi possível localizar o PlanoAEE");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma");

            var usuarios = await mediator.Send(new ObterUsuariosIdPorCodigosRfQuery(planoAEE.CriadoRF));

            if (usuarios == null || !usuarios.Any())
                throw new NegocioException("Não foi possível localizar o usuário");

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE devolvido - {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi devolvido. Motivo: {request.Motivo}" +
                $"<br/><a href='{hostAplicacao}aee/plano/editar/{planoAEE.Id}'>Clique aqui</a> para acessar o plano. " +
                $"<br/><br/>A pendência será resolvida automaticamente após a criação de uma nova versão do plano.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(planoAEE.Id, usuarios, titulo, descricao));

            return true;
        }
    }
}
