using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.EncaminhamentoAEE.Pendencias.GerarPendenciaProfessorEncaminhamentoAEEDevolvido
{
    public class GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommandHandler : IRequestHandler<GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommandHandler(IMediator mediator, IConfiguration configuration,
            IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));

        }

        public async Task<bool> Handle(GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = request.EncaminhamentoAEE;

            var UsuarioIdProfessorCriadorDoEncaminhamentoAEE = (await mediator.Send(new ObterUsuariosPorCodigosRfQuery(new string[] { encaminhamentoAEE.CriadoRF }))).FirstOrDefault(b => b.CodigoRf != "0").Id;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId), cancellationToken);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para análise. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                $"<br/><br/>Esta pendência será resolvida automaticamente quando o parecer do AEE for registrado no sistema";

            var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao, turma.Id);

            pendencia.Situacao = SituacaoPendencia.Pendente;
            encaminhamentoAEE.ResponsavelId = UsuarioIdProfessorCriadorDoEncaminhamentoAEE;

            pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

            var pendenciaUsuario = new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = UsuarioIdProfessorCriadorDoEncaminhamentoAEE };
            await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);

            var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
            await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);

            return true;
        }
    }
}
