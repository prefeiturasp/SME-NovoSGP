using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System.Linq;
using SME.SGP.Infra.Enumerados;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva>();
            if (filtro.ConsolidacoesFrequenciaMensalInsuficientes.PossuiRegistros())
            {
                var consolidacaoFrequenciaMensalInsuficiente = filtro.ConsolidacoesFrequenciaMensalInsuficientes.FirstOrDefault();
                var nomeUe = $"{((TipoEscola)consolidacaoFrequenciaMensalInsuficiente.TipoEscola).ShortName()} {consolidacaoFrequenciaMensalInsuficiente.Ue}";
                var nomeMes = ((Mes)consolidacaoFrequenciaMensalInsuficiente.Mes).ObterNome();

                var tituloNotificacao = $"Crianças/Estudantes com baixa frequência - {nomeUe}";
                var cabecalhoMensagemNotificacao = $"Na lista abaixo encontram-se as crianças/estudantes que não atingiram o percentual mínimo de frequência no mês de {nomeMes} na {nomeUe} ({consolidacaoFrequenciaMensalInsuficiente.Dre}).";
                var mensagem = await ObterCorpoMensagemNotificacao(cabecalhoMensagemNotificacao, filtro.ConsolidacoesFrequenciaMensalInsuficientes);

                foreach (var usuarioNotificacao in filtro.ResponsaveisNotificacao)
                    await mediator.Send(new NotificarUsuarioCommand(tituloNotificacao,
                                                                    mensagem,
                                                                    usuarioNotificacao.CodigoRF,
                                                                    NotificacaoCategoria.Aviso,
                                                                    NotificacaoTipo.BuscaAtiva));
            }

            return true;
        }

        private async Task<string> ObterCorpoMensagemNotificacao(string cabecalho, IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto> consolidacoesFrequenciaInsuficientes)
        {
            var texto = cabecalho;
            foreach(var turma in consolidacoesFrequenciaInsuficientes.OrderBy(cc => cc.Modalidade.ObterNomeCurto())
                                                                    .ThenBy(cc => cc.Turma)
                                                                    .GroupBy(cc => $"{cc.Modalidade.ObterNomeCurto()} - {cc.Turma}"))
            {
                texto += $"<p><strong>{turma.Key}</strong></p>";
                texto += @$"<table border='1' style='border-collapse: collapse;'>
                              <thead>
                                <tr>
                                  <th>Criança/Estudante</th>
                                  <th>% de Frequência</th>
                                  <th>Registros de busca ativa</th>
                                  </tr>
                              </thead>
                              <tbody>";

                foreach (var consolidacao in turma)
                {
                    var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(consolidacao.TurmaCodigo, consolidacao.AlunoCodigo, true));
                    texto += $@"  <tr>
                                    <td>{$"{aluno.NumeroAlunoChamada} - {aluno.NomeValido()} ({consolidacao.AlunoCodigo})"}</td>
                                    <td style='text-align: left;'>{consolidacao.Frequencia}%</td>
                                    <td style='text-align: left;'>{await mediator.Send(new ObterQdadeRegistrosAcaoAlunoMesQuery(consolidacao.AlunoCodigo, consolidacao.Mes, consolidacao.AnoLetivo))}</td>
                                  </tr>";
                }
                texto += @$"  </tbody>
                            </table>";
            } 

            return texto;
        }
    }
}
