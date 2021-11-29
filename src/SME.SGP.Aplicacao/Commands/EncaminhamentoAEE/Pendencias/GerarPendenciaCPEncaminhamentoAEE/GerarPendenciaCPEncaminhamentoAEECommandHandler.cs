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
    public class GerarPendenciaCPEncaminhamentoAEECommandHandler : IRequestHandler<GerarPendenciaCPEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public GerarPendenciaCPEncaminhamentoAEECommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration,
            IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(GerarPendenciaCPEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(request.EncaminhamentoAEEId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("Não foi possível localizar o EncaminhamentoAEE");



            if (encaminhamentoAEE.Situacao == SituacaoAEE.Encaminhado)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

                var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
                var hostAplicacao = configuration["UrlFrontEnd"];
                var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

                var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
                var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para análise da coordenação. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                    $"<br/><br/>Esta pendência será resolvida automaticamente quando o parecer da coordenação for registrado no sistema.";

                using (var transacao = unitOfWork.IniciarTransacao())
                {
                    try
                    {
                        var pendenciaExiste = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(encaminhamentoAEE.Id));
                        var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao, null, null, turma.UeId);
                        if (pendenciaExiste == null)
                        {
                            pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

                            var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
                            await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);

                            await mediator.Send(new SalvarPendenciaPerfilCommand(pendencia.Id, new List<PerfilUsuario> { PerfilUsuario.CP }));
                        }

                        unitOfWork.PersistirTransacao();

                        if (pendencia.Id > 0)
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendencia.Id, turma.UeId), Guid.NewGuid()));

                        return true;
                    }
                    catch (Exception e)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            return false;
        }
    }
}
