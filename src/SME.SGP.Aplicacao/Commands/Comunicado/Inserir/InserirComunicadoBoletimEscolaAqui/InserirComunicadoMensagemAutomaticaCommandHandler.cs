using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoMensagemAutomaticaCommandHandler : IRequestHandler<InserirComunicadoMensagemAutomaticaCommand, bool>
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public InserirComunicadoMensagemAutomaticaCommandHandler(IRepositorioComunicado repositorioComunicado, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirComunicadoMensagemAutomaticaCommand request, CancellationToken cancellationToken)
        {
            var comunicado = new Comunicado();
            var aluno = await ObterNomeAluno(request.Aluno, request.AnoLetivo);
            MapearParaEntidade(request, comunicado, aluno.Nome, request.TipoRelatorio, request.NomeRelatorio);
            try
            {
                unitOfWork.IniciarTransacao();
                var id = await repositorioComunicado.SalvarAsync(comunicado);
                if (comunicado.Modalidades.NaoEhNulo())
                    await mediator.Send(new InserirComunicadoModalidadeCommand(comunicado));
                if (comunicado.Turmas.NaoEhNulo() && comunicado.Turmas.Any())
                    await InserirComunicadoTurma(comunicado);
                if (comunicado.Alunos.NaoEhNulo() && comunicado.Alunos.Any())
                    await InserirComunicadoAluno(comunicado);

                await mediator.Send(new CriarNotificacaoEscolaAquiCommand(comunicado));
                unitOfWork.PersistirTransacao();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                return await Task.FromResult(false);
            }
        }
        private async Task InserirComunicadoTurma(Comunicado comunicado)
        {
            comunicado.AtualizarIdTurmas();
            await mediator.Send(new InserirComunicadoTurmaCommand(comunicado.Turmas));
        }
        private async Task InserirComunicadoAluno(Comunicado comunicado)
        {
            comunicado.AtualizarIdAlunos();
            await mediator.Send(new InserirComunicadoAlunoCommand(comunicado.Alunos));
        }
        public async Task<AlunoReduzidoDto> ObterNomeAluno(string codigoAluno, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(codigoAluno, anoLetivo));
        }
        private void MapearParaEntidade(InserirComunicadoMensagemAutomaticaCommand request, Comunicado comunicado, string nomeAluno, TipoRelatorio tipoRelatorio, string nomeRelatorio)
        {
            var mensagem = FormatarMensagem(request.UrlRedirecionamentoBase, request.AnoLetivo, request.CodigoArquivo, nomeAluno, tipoRelatorio, request.Semestre);
            comunicado.DataEnvio = request.DataEnvio;
            comunicado.DataExpiracao = request.DataExpiracao;
            comunicado.Descricao = mensagem;
            comunicado.Titulo = $"{nomeRelatorio} disponível para download";
            comunicado.AnoLetivo = request.AnoLetivo;
            comunicado.AlunoEspecificado = request.AlunoEspecificado;
            comunicado.Modalidades = new int[] { request.Modalidade };
            comunicado.TipoComunicado = TipoComunicado.MENSAGEM_AUTOMATICA;
            comunicado.Semestre = request.Semestre;
            comunicado.AdicionarTurma(request.Turma);
            comunicado.AdicionarAluno(request.Aluno);
        }
        private string FormatarMensagem(string urlNotificacao, int anoLetivo, Guid CodigoArquivo, string nomeAluno, TipoRelatorio tipoRelatorio, int semestre)
        {
            switch (tipoRelatorio)
            {
                case TipoRelatorio.BoletimDetalhadoApp:
                    return MensagemRelatorioBoletim(urlNotificacao, anoLetivo, CodigoArquivo, nomeAluno);
                case TipoRelatorio.RaaEscolaAqui:
                    return MensagemRelatorioRaa(urlNotificacao, CodigoArquivo, nomeAluno, semestre);
                default:
                    return string.Empty;
            }
        }
        private static string MensagemRelatorioBoletim(string urlNotificacao, int anoLetivo, Guid CodigoArquivo,
            string nomeAluno)
        {
            return $@"<h3><strong>Boletim {anoLetivo} dispon&iacute;vel para download</strong></h3>
                    <p>O boletim do ano de {anoLetivo} do estudante {nomeAluno.ToUpper()} est&aacute; dispon&iacute;vel, clique no bot&atilde;o abaixo para fazer o download do arquivo.</p>
                    <p>OBSERVA&Ccedil;&Atilde;O: O Download deve ser realizado em at&eacute; 24 horas, ap&oacute;s&nbsp; este prazo o arquivo ser&aacute; 
                    exclu&iacute;do e caso necessite voc&ecirc; dever solicitar um novo PDF de boletim.</p>
                    <p><strong><a href='{urlNotificacao}/api/v1/downloads/sgp/pdf/Boletim.pdf/{CodigoArquivo.ToString()}' target='_blank'>Boletim {anoLetivo}</a></strong></p>";
        }
        private static string MensagemRelatorioRaa(string urlNotificacao, Guid CodigoArquivo, string nomeAluno, int semestre)
        {
            return $@"<p>O Relatório de Acompanhamento da Aprendizagem(RAA) do {semestre}° semestre do criança {nomeAluno.ToUpper()} est&aacute; dispon&iacute;vel, clique no bot&atilde;o abaixo para fazer o download do arquivo.</p>
                    <p>OBSERVA&Ccedil;&Atilde;O: O Download deve ser realizado em at&eacute; 24 horas, ap&oacute;s&nbsp; este prazo o arquivo ser&aacute; 
                    exclu&iacute;do e caso necessite voc&ecirc; dever&aacute solicitar novamente.</p>
                    <p><strong><a href='{urlNotificacao}/api/v1/downloads/sgp/html/RAA/{CodigoArquivo.ToString()}' target='_blank'>RAA {semestre}° Semestre</a></strong></p>";
        }

    }
}

