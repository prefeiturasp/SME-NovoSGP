using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System.Linq;
using SME.SGP.Infra.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<List<ConsolidacaoFreqAlunoMensalInsuficienteDto>>();
            if (filtro.PossuiRegistros())
            {
                var registro = filtro.FirstOrDefault();
                var nomeUe = $"{((TipoEscola)registro.TipoEscola).ShortName()} {registro.Ue}";
                var nomeMes = ((Mes)registro.Mes).ObterNome();

                var tituloNotificacao = $"Crianças/Estudantes com baixa frequência - {nomeUe}";
                var cabecalhoTexto = $"Na lista abaixo encontram-se as crianças/estudantes que não atingiram o percentual mínimo de frequência no mês de {nomeMes} na {nomeUe} ({registro.Dre}).";
                await PreencherInsuficienciaFrequenciaTurma(cabecalhoTexto, filtro);
            }

            return true;
        }

        private async Task<string> PreencherInsuficienciaFrequenciaTurma(string cabecalho, IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto> consolidacoesFrequenciaInsuficientes)
        {
            var texto = cabecalho;
            foreach(var turma in consolidacoesFrequenciaInsuficientes.OrderBy(cc => cc.Modalidade.ObterNomeCurto())
                                                                    .ThenBy(cc => cc.Turma)
                                                                    .GroupBy(cc => $"{cc.Modalidade.ObterNomeCurto()} - {cc.Turma}"))
            {
                texto += $"<p><strong>{turma}</strong></p>";
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
                                    <td></td>
                                    <td>{consolidacao.Frequencia}%</td>
                                    <td>{await mediator.Send(new ObterQdadeRegistrosAcaoAlunoMesQuery(consolidacao.AlunoCodigo, consolidacao.Mes, consolidacao.AnoLetivo))}</td>
                                  </tr>";
                }
                texto += @$"  </tbody>
                            </table>";
            } 

            return texto;
        }
    }
}
