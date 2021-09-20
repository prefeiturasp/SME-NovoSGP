using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Fechamento.GerarPendenciasFechamento
{
    public class GerarPendenciasFechamentoCommandHandler : IRequestHandler<GerarPendenciasFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IConfiguration configuration;

        public GerarPendenciasFechamentoCommandHandler(IMediator mediator, IServicoPendenciaFechamento servicoPendenciaFechamento, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(GerarPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            if (!request.ComponenteSemNota)
            {
                await servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(request.FechamentoTurmaDisciplinaId, request.TurmaCodigo, request.ComponenteCurricularId, request.PeriodoEscolarInicio, request.PeriodoEscolarFim);
                await servicoPendenciaFechamento.ValidarPercentualAlunosAbaixoDaMedia(request.FechamentoTurmaDisciplinaId, request.Justificativa, request.CriadoRF);
                await servicoPendenciaFechamento.ValidarAlteracaoExtemporanea(request.FechamentoTurmaDisciplinaId, request.TurmaCodigo, request.CriadoRF);
            }
            await servicoPendenciaFechamento.ValidarAulasReposicaoPendente(request.FechamentoTurmaDisciplinaId, request.TurmaCodigo, request.TurmaNome, request.ComponenteCurricularId, request.PeriodoEscolarInicio, request.PeriodoEscolarFim);

            if (request.RegistraFrequencia)
                await servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(request.FechamentoTurmaDisciplinaId, request.TurmaCodigo, request.TurmaNome, request.ComponenteCurricularId, request.PeriodoEscolarInicio, request.PeriodoEscolarFim);

            var quantidadePendencias = servicoPendenciaFechamento.ObterQuantidadePendenciasGeradas();
            if (quantidadePendencias > 0)
            {
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;
                await GerarNotificacaoFechamento(request.ComponenteCurricularId, request.TurmaCodigo, request.UsuarioId, request.Bimestre, servicoPendenciaFechamento);
            }

            await mediator.Send(new AtualizarSituacaoFechamentoTurmaDisciplinaCommand(request.FechamentoTurmaDisciplinaId, situacaoFechamento));



            var consolidacaoTurma = new ConsolidacaoTurmaDto(request.TurmaId, request.Bimestre);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid(), null));

            return true;
        }

        private async Task GerarNotificacaoFechamento(long componenteCurricularId, string turmaCodigo, long usuarioLogadoId, int bimestre, IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularId }));
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Componente curricular não encontrado.");
            }

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Não foi possivel localizar a turma de código {turmaCodigo} para notificação das pendências de fechamento");

            var ue = turma.Ue;
            var dre = turma.Ue.Dre;

            var urlFrontEnd = configuration["UrlFrontEnd"];
            if (string.IsNullOrWhiteSpace(urlFrontEnd))
                throw new NegocioException("Url do frontend não encontrada.");

            var pendencias = FormatarPendenciasGeradas(servicoPendenciaFechamento.ObterDescricaoPendenciasGeradas());

            await NotificarUsuarios($"Pendência no fechamento da turma {turma.Nome}",
                    $"O fechamento do {bimestre}º bimestre de {componentes.FirstOrDefault().Nome} da turma {turma.Nome} da {ue.Nome} ({dre.Nome}) gerou {servicoPendenciaFechamento.ObterQuantidadePendenciasGeradas()} pendência(s): " +
                    pendencias +
                    "Para consultar os detalhes da(s) pendência(s) acesse a tela 'Fechamento > Pendências do fechamento'", 
                    usuarioLogadoId, 
                    dre.CodigoDre, 
                    ue.CodigoUe, 
                    turma.CodigoTurma);
        }

        private async Task NotificarUsuarios(string titulo, string mensagem, long usuarioLogadoId, string codigoDre, string codigoUe, string codigoTurma)
        {
            // Notifica Usuario
            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, 
                mensagem, 
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Fechamento,
                new long[] { usuarioLogadoId },
                codigoDre,
                codigoUe,
                codigoTurma));

            // Notifica CP e Diretor
            await mediator.Send(new EnviarNotificacaoCommand(titulo,
                mensagem,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Fechamento,
                new Cargo[] { Cargo.CP, Cargo.Diretor },
                codigoDre,
                codigoUe,
                codigoTurma));
        }

        private string FormatarPendenciasGeradas(IEnumerable<string> descricoesPendencias)
        {
            var pendencias = "<ul>";
            foreach (var descricaoPendencia in descricoesPendencias)
                pendencias += $"<li>{descricaoPendencia}</li>";
            pendencias += "</ul>";

            return pendencias;
        }
    }
}
