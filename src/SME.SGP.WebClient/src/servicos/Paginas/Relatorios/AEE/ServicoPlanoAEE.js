import api from '~/servicos/api';
import { store } from '~/redux';
import situacaoPlanoAEE from '~/dtos/situacaoPlanoAEE';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';
import tipoQuestao from '~/dtos/tipoQuestao';
import { setExibirLoaderPlanoAEE } from '~/redux/modulos/planoAEE/actions';
import { erros } from '~/servicos/alertas';

const urlPadrao = 'v1/plano-aee';

class ServicoPlanoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterPlanoPorId = planoId => {
    return api.get(`${urlPadrao}/${planoId}`);
  };

  obterPlanoPorCodigoEstudante = codigoEstudante => {
    return api.get(`${urlPadrao}/estudante/${codigoEstudante}`);
  };

  obterVersaoPlanoPorId = versaoPlanoId => {
    return api.get(`${urlPadrao}/versao/${versaoPlanoId}`);
  };

  obterQuestionario = (questionarioId, planoId, codigoAluno, codigoTurma) => {
    let url = `${urlPadrao}/questionario?questionarioId=${questionarioId}&codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    if (planoId) {
      url = `${url}&planoId=${planoId}`;
    }
    return api.get(url);
  };

  salvarPlano = async () => {
    const { dispatch } = store;

    const state = store.getState();
    const { questionarioDinamico, collapseLocalizarEstudante } = state;
    const { formsQuestionarioDinamico } = questionarioDinamico;

    const { dadosCollapseLocalizarEstudante } = collapseLocalizarEstudante;

    let contadorFormsValidos = 0;

    const validaAntesDoSubmit = refForm => {
      let arrayCampos = [];

      const camposValidar = refForm?.state?.values;
      if (camposValidar && Object.keys(camposValidar)?.length) {
        arrayCampos = Object.keys(camposValidar);
      }

      arrayCampos.forEach(campo => {
        refForm.setFieldTouched(campo, true, true);
      });
      return refForm.validateForm().then(() => {
        if (
          refForm.getFormikContext().isValid ||
          Object.keys(refForm.getFormikContext().errors).length === 0
        ) {
          contadorFormsValidos += 1;
        }
      });
    };

    if (formsQuestionarioDinamico?.length) {
      let todosOsFormsEstaoValidos = false;

      const promises = formsQuestionarioDinamico.map(async item =>
        validaAntesDoSubmit(item.form())
      );

      await Promise.all(promises);

      todosOsFormsEstaoValidos =
        contadorFormsValidos ===
        formsQuestionarioDinamico?.filter(a => a)?.length;
      
      if (todosOsFormsEstaoValidos) {
        let questoesSalvar = formsQuestionarioDinamico.map(item => {
          const form = item.form();
          const campos = form.state.values;
          const questoes = [];
          Object.keys(campos).forEach(key => {
            const questaoAtual = QuestionarioDinamicoFuncoes.obterQuestaoPorId(
              item.dadosQuestionarioAtual,
              key
            );

            let questao = {
              questaoId: key,
              tipoQuestao: questaoAtual.tipoQuestao,
            };

            switch (questao.tipoQuestao) {
              case tipoQuestao.FrequenciaEstudanteAEE:
                questao.resposta = JSON.stringify(campos[key] || '');
                break;
              case tipoQuestao.Upload:
                if (campos[key]?.length) {
                  const arquivosId = campos[key].map(a => a.xhr);
                  questao.resposta = arquivosId;
                } else {
                  questao.resposta = '';
                }
                break;
              case tipoQuestao.ComboMultiplaEscolha:
                if (campos[key]?.length) {
                  questao.resposta = campos[key];
                } else {
                  questao.resposta = '';
                }
                break;
              default:
                questao.resposta = campos[key] || '';
                break;
            }

            if (
              questao.tipoQuestao === tipoQuestao.ComboMultiplaEscolha &&
              questao?.resposta?.length
            ) {
              questao.resposta.forEach(valorSelecionado => {
                if (valorSelecionado) {
                  if (questaoAtual?.resposta?.length) {
                    const temResposta = questaoAtual.resposta.find(
                      a =>
                        String(a?.opcaoRespostaId) === String(valorSelecionado)
                    );

                    if (temResposta) {
                      questoes.push({
                        ...questao,
                        resposta: valorSelecionado,
                        respostaPlanoId: temResposta.id,
                      });
                    } else {
                      questoes.push({
                        ...questao,
                        resposta: valorSelecionado,
                      });
                    }
                  } else {
                    questoes.push({
                      ...questao,
                      resposta: valorSelecionado,
                    });
                  }
                }
              });
            } else {
              if (questaoAtual?.resposta[0]?.id) {
                questao.respostaPlanoId = questaoAtual.resposta[0].id;
              }

              if (
                (questao.tipoQuestao === tipoQuestao.Upload ||
                  questao.tipoQuestao === tipoQuestao.ComboMultiplaEscolha) &&
                !questao.resposta
              ) {
                questao = null;
              }

              if (questao) {
                questoes.push(questao);
              }
            }
          });
          return questoes;
        });

        questoesSalvar = questoesSalvar.filter(q => q !== null);
        const valoresParaSalvar = {
          id: 0,
          turmaId: dadosCollapseLocalizarEstudante.turmaId,
          alunoCodigo: dadosCollapseLocalizarEstudante.codigoAluno,
          situacao: situacaoPlanoAEE.EmAndamento,
          questoes: questoesSalvar[0],
        };

        const resposta = await api
          .post(`${urlPadrao}/salvar`, valoresParaSalvar)
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

        if (resposta?.status === 200) {
          return true;
        }
      }
      // } else {
      //   // dispatch(setExibirModalErrosEncaminhamento(true));
      // }
    }
    return false;
  };
}

export default new ServicoPlanoAEE();
