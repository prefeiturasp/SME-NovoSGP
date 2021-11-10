using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaValidacaoPlanoAEECommandHandler : IRequestHandler<GerarPendenciaValidacaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public GerarPendenciaValidacaoPlanoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(GerarPendenciaValidacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("Não foi possível localizar o PlanoAEE");

            var existePendencia = await mediator.Send(new ExistePendenciaPlanoAEEQuery(request.PlanoAEEId));

            if (existePendencia)
                return false;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE para validação - {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi cadastrado. <br/><a href='{hostAplicacao}aee/plano/editar/{planoAEE.Id}'>Clique aqui</a> para acessar o plano e registrar o seu parecer. " +
                $"<br/><br/>A pendência será resolvida automaticamente após este registro.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(planoAEE.Id, null, titulo, descricao, PerfilUsuario.CP));

            return false;
        }
    }
}
