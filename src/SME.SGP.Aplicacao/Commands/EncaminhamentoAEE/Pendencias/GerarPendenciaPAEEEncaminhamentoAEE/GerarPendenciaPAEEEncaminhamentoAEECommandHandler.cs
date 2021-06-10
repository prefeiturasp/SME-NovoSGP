using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPAEEEncaminhamentoAEECommandHandler : IRequestHandler<GerarPendenciaPAEEEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;


        public GerarPendenciaPAEEEncaminhamentoAEECommandHandler(IMediator mediator, IConfiguration configuration, 
            IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(GerarPendenciaPAEEEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = request.EncaminhamentoAEE;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.InfantilPreEscola ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para análise. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                $"<br/><br/>Esta pendência será resolvida automaticamente quando o parecer do AEE for registrado no sistema";

            var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao);
            pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

            var pendenciaUsuario = new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = encaminhamentoAEE.ResponsavelId.GetValueOrDefault() };
            await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);

            var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
            await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);

            return true;
        }
    }
}
