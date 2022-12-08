using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterNotificacaoPorIdUseCase : IObterNotificacaoPorIdUseCase
{
    private readonly IMediator mediator;

    public ObterNotificacaoPorIdUseCase(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<NotificacaoDetalheDto> Executar(long param)
    {
        var notificacao = await mediator.Send(new ObterNotificacaoPorIdQuery(param));

        if (notificacao == null)
            throw new NegocioException($"Notificação de Id: '{param}' não localizada.");

        if (notificacao.Status != NotificacaoStatus.Lida && notificacao.MarcarComoLidaAoObterDetalhe())
        {
            await mediator.Send(new SalvarNotificacaoCommand(notificacao));
            await mediator.Send(new NotificarLeituraNotificacaoCommand(notificacao));
        }

        var retorno = await MapearEntidadeParaDetalheDto(notificacao);

        return retorno;
    }
    
    private async Task<NotificacaoDetalheDto> MapearEntidadeParaDetalheDto(Notificacao retorno)
    {
        var codigoRelatorio = string.Empty;
        var tipoRelatorio = 0;
        var relatorioExiste = true;

        if (NotificacaoTipo.Relatorio == retorno.Tipo)
            codigoRelatorio = ObterCodigoArquivo(retorno.Mensagem);

        if (codigoRelatorio.Any())
            tipoRelatorio = await mediator.Send(new ObterTipoRelatorioPorCodigoQuery(codigoRelatorio));

        if (!string.IsNullOrEmpty(codigoRelatorio) && tipoRelatorio != (int)TipoRelatorio.Itinerancias)
            relatorioExiste = await VerificarSeArquivoExiste(codigoRelatorio);

        return new NotificacaoDetalheDto
        {
            AlteradoEm = retorno.AlteradoEm.ToString(),
            AlteradoPor = retorno.AlteradoPor,
            CriadoEm = retorno.CriadoEm.ToString("dd/MM/yyyy"),
            CriadoPor = retorno.CriadoPor,
            Id = retorno.Id,
            Mensagem = relatorioExiste ? await ObterMensagem(retorno) : "O arquivo não está mais disponível, solicite a geração do relatório novamente.",
            Situacao = retorno.Status.ToString(),
            Tipo = retorno.Tipo.GetAttribute<DisplayAttribute>().Name,
            Titulo = retorno.Titulo,
            MostrarBotaoRemover = retorno.PodeRemover,
            MostrarBotoesDeAprovacao = retorno.DeveAprovar,
            MostrarBotaoMarcarComoLido = retorno.DeveMarcarComoLido,
            CategoriaId = (int)retorno.Categoria,
            TipoId = (int)retorno.Tipo,
            StatusId = (int)retorno.Status,
            Codigo = retorno.Codigo,
            Observacao = retorno.WorkflowAprovacaoNivel == null ? string.Empty : retorno.WorkflowAprovacaoNivel.Observacao
        };
    }

    private async Task<string> ObterMensagem(Notificacao notificacao)
    {
        switch (notificacao.Categoria)
        {
            case NotificacaoCategoria.Workflow_Aprovacao:
                return await ObterMensagemWorkflowAprovacao(notificacao);
            case NotificacaoCategoria.Alerta:
            case NotificacaoCategoria.Aviso:
            default:
                return notificacao.Mensagem;
        }
    }

    private async Task<string> ObterMensagemWorkflowAprovacao(Notificacao notificacao)
    {
        var workflowAprovacao = await mediator.Send(new ObterWorkflowAprovacaoPorNotificacaoIdQuery(notificacao.Id));

        switch (workflowAprovacao.Tipo)
        {
            case WorkflowAprovacaoTipo.AlteracaoParecerConclusivo:
                return await MontarMensagemParecerConclusivo(workflowAprovacao.Id, notificacao);
            case WorkflowAprovacaoTipo.Basica:
            case WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional:
            case WorkflowAprovacaoTipo.ReposicaoAula:
            case WorkflowAprovacaoTipo.Evento_Data_Passada:
            case WorkflowAprovacaoTipo.Fechamento_Reabertura:
            case WorkflowAprovacaoTipo.AlteracaoNotaFechamento:                
            case WorkflowAprovacaoTipo.RegistroItinerancia:                
            case WorkflowAprovacaoTipo.AlteracaoNotaConselho:
            default:
               return notificacao.Mensagem;
        }
    }

    private async Task<string> MontarMensagemParecerConclusivo(long workflowId, Notificacao notificacao)
    {
        var pareceresConclusivos =
            (await mediator.Send(new ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery(workflowId))).ToList();

        if (!pareceresConclusivos.Any())
            return string.Empty;
        
        var alunosCodigos = pareceresConclusivos.Select(wf => long.Parse(wf.AlunoCodigo)).ToArray();
        var alunos = (await mediator.Send(new ObterAlunosEolPorCodigosQuery(alunosCodigos))).ToList();
        
        var usuariosIds = pareceresConclusivos.Select(wf => wf.UsuarioSolicitanteId).Distinct().ToArray();
        var usuarios = (await mediator.Send(new ObterUsuarioPorIdsSemPerfilQuery(usuariosIds))).ToList();

        var msg = new StringBuilder();
        var agrupamentoPorTurma = pareceresConclusivos.GroupBy(wf => wf.TurmaId);
        
        foreach (var grupoTurma in agrupamentoPorTurma)
        {
            var cabecalho = string.Empty; // TODO montar o cabeçalho
            msg.AppendLine(cabecalho);
            
            msg.AppendLine("<table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            msg.AppendLine("<tbody>");
            msg.AppendLine("<tr>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Estudante</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Parecer anterior</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Novo Parecer</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Usuário que alterou</strong></td>");
            msg.AppendLine("<td style='padding: 3px;'><strong>Data da alteração</strong></td>");
            msg.AppendLine("</tr>");  
            
            foreach (var aprovacao in grupoTurma.ToList())
            {
                var aluno = alunos.Find(aluno => aluno.CodigoAluno.ToString() == aprovacao.AlunoCodigo);
                var usuario = usuarios.Find(usuario => usuario.Id == aprovacao.UsuarioSolicitanteId);

                msg.AppendLine($@"<tr>
                    <td style='padding: 3px;'>{aluno.NumeroAlunoChamada} - {aluno.NomeAluno} ({aluno.CodigoAluno})</td>
                    <td style='padding: 3px;'>{aprovacao.NomeParecerAnterior}</td>
                    <td style='padding: 3px;'>{aprovacao.NomeParecerNovo}</td>
                    <td style='padding: 3px;'>{usuario.Nome} ({usuario.CodigoRf})</td>
                    <td style='padding: 3px;'>{aprovacao.CriadoEm:dd/MM/yyy HH:mm}</td>
                </tr>");
            }
            
            var rodape = string.Empty; // TODO montar o rodapé
            msg.AppendLine(rodape);
        }

        return msg.ToString();
    }
    
    private static string ObterCodigoArquivo(string mensagem)
    {
        const string pattern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
        Regex rg = new(pattern);
        var codigo = rg.Match(mensagem);
        return codigo.ToString();
    } 
    
    private async Task<bool> VerificarSeArquivoExiste(string codigoArquivo)
    {
        var guidRelatorio = new Guid(codigoArquivo);
        return await mediator.Send(new VerificarExistenciaRelatorioPorCodigoQuery(guidRelatorio));
    }        
}