using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncerramentoPlanoAEECommandHandler : IRequestHandler<GerarPendenciaCPEncerramentoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public GerarPendenciaCPEncerramentoPlanoAEECommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(GerarPendenciaCPEncerramentoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("Não foi possível localizar o PlanoAEE");

            if (planoAEE.Situacao == SituacaoPlanoAEE.ParecerCP)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));

                var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
                var hostAplicacao = configuration["UrlFrontEnd"];
                var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

                var titulo = $"Plano AEE a encerrar - {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) - {ueDre}";
                var descricao = $"Foi solicitado o encerramento do Plano AEE {estudanteOuCrianca} {planoAEE.AlunoNome} ({planoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre}. <br/><a href='{hostAplicacao}aee/plano/editar/{planoAEE.Id}'>Clique aqui</a> para acessar o plano e registrar a devolutiva. " +
                    $"<br/><br/>A pendência será resolvida automaticamente após este registro.";

                await mediator.Send(new GerarPendenciaPlanoAEECommand(planoAEE.Id, null, titulo, descricao, PerfilUsuario.CP));
            }
            return false;
        }
    }
}
