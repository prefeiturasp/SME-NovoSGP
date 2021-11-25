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
    public class GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;
        private readonly IUnitOfWork unitOfWork;


        public GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, IConfiguration configuration,
            IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE,
            IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = request.EncaminhamentoAEE;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            if (request.EhCEFAI)
            {
                var usuariosId = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(turma.Ue.Dre.CodigoDre));

                if (!usuariosId.Any())
                    return false;

                foreach(var usuarioId in usuariosId)
                    await EnviarParaCEFAI(usuarioId, turma, encaminhamentoAEE);

                return true;
            }
            else
            {
                return await EnviarParaFuncionarios(turma, encaminhamentoAEE);
            }
        }

        private async Task<bool> EnviarParaFuncionarios(Turma turma, EncaminhamentoAEE encaminhamentoAEE)
        {
            var usuarios = await mediator.Send(new ObterUsuariosIdPorCodigoUeQuery(turma.Ue.CodigoUe));

            if (usuarios == null)
                return false;

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para atribuição de um PAEE. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                $"<br/><br/>Esta pendência será resolvida automaticamente quando o PAEE for atribuído no encaminhamento.";

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    foreach (var usuario in usuarios)
                    {
                        var existePendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(encaminhamentoAEE.Id, usuario));
                        if (existePendencia != null)
                            await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(existePendencia.PendenciaId));

                        var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao,string.Empty,string.Empty,turma.UeId);
                        pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

                        var pendenciaUsuario = new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = usuario };
                        await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);

                        var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
                        await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);

                    }
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }

        private async Task<bool> EnviarParaCEFAI(long usuarioId, Turma turma, EncaminhamentoAEE encaminhamentoAEE)
        {
            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
            var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para atribuição de um PAAI. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                $"<br/><br/>Esta pendência será resolvida automaticamente quando o PAAI for atribuído no encaminhamento.";

            var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao,string.Empty,string.Empty,turma.UeId);
            pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

            var pendenciaUsuario = new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = usuarioId };
            await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);

            var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
            await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);

            return true;
        }
    }
}
