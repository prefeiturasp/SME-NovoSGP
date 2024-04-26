using MediatR;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarMapeamentoDoEstudanteDoBimestreUseCase : AbstractUseCase, IAtualizarMapeamentoDoEstudanteDoBimestreUseCase
    {
        public AtualizarMapeamentoDoEstudanteDoBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            long.TryParse(param.Mensagem.ToString(), out long idMapeamentoEstudante);
            var mapeamentoEstudante = await mediator.Send(new ObterMapeamentoEstudantePorIdQuery(idMapeamentoEstudante));
            if (mapeamentoEstudante.NaoEhNulo())
            {
                var respostasAtualizadas = await mediator.Send(new ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery(mapeamentoEstudante.Secoes
                                                                                                                                        .FirstOrDefault()
                                                                                                                                        .SecaoMapeamentoEstudante
                                                                                                                                        .QuestionarioId,
                                                                                                                                mapeamentoEstudante.TurmaId,
                                                                                                                                mapeamentoEstudante.AlunoCodigo,
                                                                                                                                mapeamentoEstudante.Bimestre));

                var secao = mapeamentoEstudante.Secoes.FirstOrDefault();
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.ACOMPANHADO_SRM_CEFAI,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.PARTICIPA_PAP,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.PARTICIPA_MAIS_EDUCACAO,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.PROJETO_FORTALECIMENTO_APRENDIZAGENS,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.HIPOTESE_ESCRITA,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.AVALIACOES_EXTERNAS_PROVA_SP,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.FREQUENCIA,
                                                  respostasAtualizadas);
                await AtualizarRespostaMapeamento(secao.Questoes, NomesComponentesMapeamentoEstudante.QDADE_REGISTROS_BUSCA_ATIVA,
                                                  respostasAtualizadas);

                return true;
            }
            return false;
        }

        private async Task AtualizarRespostaMapeamento(List<QuestaoMapeamentoEstudante> questoes, string nomeComponente, IEnumerable<RespostaQuestaoMapeamentoEstudanteDto> respostasAtualizadas)
        {
            var questaoAtual = questoes.FirstOrDefault(q => q.Questao.NomeComponente.Equals(nomeComponente));
            var respostaAtual = questaoAtual.Respostas[0];
            var respostaAtualizada = respostasAtualizadas.FirstOrDefault(r => r.QuestaoId.Equals(questaoAtual.QuestaoId));
            await mediator.Send(new AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommand(respostaAtual,
                                                                                          respostaAtualizada.ToMapeamentoEstudanteSecaoQuestaoDto(questaoAtual.Questao.Tipo,
                                                                                                                                                  respostaAtual.Id)));
        }
    }
}
