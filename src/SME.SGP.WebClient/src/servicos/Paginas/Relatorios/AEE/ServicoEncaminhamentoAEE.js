import * as Yup from 'yup';
import tipoQuestao from '~/dtos/tipoQuestao';
import { store } from '~/redux';
import {
  setDadosModalAviso,
  setEncaminhamentoAEEEmEdicao,
  setExibirLoaderEncaminhamentoAEE,
  setExibirModalAviso,
  setExibirModalErrosEncaminhamento,
  setFormsSecoesEncaminhamentoAEE,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

const urlPadrao = 'v1/encaminhamento-aee';

class ServicoEncaminhamentoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterAvisoModal = async () => {
    const { dispatch } = store;

    const state = store.getState();
    const { encaminhamentoAEE } = state;

    const { dadosModalAviso } = encaminhamentoAEE;

    if (!dadosModalAviso) {
      const retorno = await api.get(`${urlPadrao}/instrucoes-modal`);
      if (retorno?.data) {
        dispatch(setDadosModalAviso(retorno.data));
        dispatch(setExibirModalAviso(true));
      } else {
        dispatch(setDadosModalAviso());
        dispatch(setExibirModalAviso(false));
      }
    } else {
      dispatch(setExibirModalAviso(true));
    }
  };

  obterSecoesPorEtapaDeEncaminhamentoAEE = (etapa, encaminhamentoAeeId) => {
    let url = `${urlPadrao}/secoes?etapa=${etapa}`;
    if (encaminhamentoAeeId) {
      url += `&encaminhamentoAeeId=${encaminhamentoAeeId}`;
    }
    return api.get(url);
  };

  obterQuestionario = (
    questionarioId,
    encaminhamentoId,
    codigoAluno,
    codigoTurma
  ) => {
    let url = `${urlPadrao}/questionario?questionarioId=${questionarioId}&codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    if (encaminhamentoId) {
      url = `${url}&encaminhamentoId=${encaminhamentoId}`;
    }
    return api.get(url);
  };

  obterEncaminhamentoPorId = encaminhamentoId => {
    return api.get(`${urlPadrao}/${encaminhamentoId}`);
  };

  resetarTelaDadosOriginais = () => {
    const { dispatch } = store;
    const state = store.getState();
    const { encaminhamentoAEE } = state;
    const { formsSecoesEncaminhamentoAEE } = encaminhamentoAEE;
    if (formsSecoesEncaminhamentoAEE?.length) {
      formsSecoesEncaminhamentoAEE.forEach(item => {
        const form = item.form();
        form.resetForm();
      });
      dispatch(setEncaminhamentoAEEEmEdicao(false));
    }
  };

  addFormsSecoesEncaminhamentoAEE = (
    obterForm,
    questionarioId,
    dadosQuestionarioAtual,
    secaoId
  ) => {
    const { dispatch } = store;
    const state = store.getState();
    const { encaminhamentoAEE } = state;
    const { formsSecoesEncaminhamentoAEE } = encaminhamentoAEE;
    if (!formsSecoesEncaminhamentoAEE) {
      const param = [];
      param[questionarioId] = {
        form: obterForm,
        dadosQuestionarioAtual,
        secaoId,
      };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    } else if (formsSecoesEncaminhamentoAEE?.length) {
      const param = formsSecoesEncaminhamentoAEE;
      param[questionarioId] = {
        form: obterForm,
        dadosQuestionarioAtual,
        secaoId,
      };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    }
  };

  // TODO
  secaoEstaConcluida = secaoId => {
    const state = store.getState();
    const { encaminhamentoAEE } = state;
    const { formsSecoesEncaminhamentoAEE } = encaminhamentoAEE;

    if (formsSecoesEncaminhamentoAEE?.length) {
      const form = formsSecoesEncaminhamentoAEE.find(
        d => d.secaoId === secaoId
      );
      if (form && form()) {
        return form().getFormikContext().isValid;
      }
    }

    return false;
  };

  obterQuestaoPorId = (dados, idPesquisa) => {
    let questaoAtual = '';

    const obterQuestao = item => {
      if (!questaoAtual) {
        if (String(item.id) === String(idPesquisa)) {
          questaoAtual = item;
        } else if (item?.opcaoResposta?.length) {
          item.opcaoResposta.forEach(opcaoResposta => {
            if (opcaoResposta.questaoComplementar) {
              obterQuestao(opcaoResposta.questaoComplementar);
            }
          });
        }
      }
    };

    dados.forEach(item => {
      obterQuestao(item);
    });

    return questaoAtual;
  };

  obterValidationSchema = (dadosQuestionarioAtual, form) => {
    if (dadosQuestionarioAtual?.length && form?.state?.values) {
      const camposComValidacao = {};

      let arrayCampos = [];

      const camposValidar = form?.state?.values;
      if (camposValidar && Object.keys(camposValidar)?.length) {
        arrayCampos = Object.keys(camposValidar);
      }

      const montaValidacoes = questaoAtual => {
        if (questaoAtual?.opcaoResposta?.length) {
          questaoAtual.opcaoResposta.forEach(opcaoAtual => {
            if (opcaoAtual?.questaoComplementar) {
              montaValidacoes(opcaoAtual.questaoComplementar);
            }
          });
        }

        if (
          questaoAtual.obrigatorio &&
          arrayCampos.find(questaoId => questaoId === String(questaoAtual.id))
        ) {
          camposComValidacao[questaoAtual.id] = Yup.string()
            .nullable()
            .required('Campo obrigatório');
        }
      };

      if (arrayCampos?.length) {
        dadosQuestionarioAtual.forEach(questaoAtual => {
          montaValidacoes(questaoAtual);
        });

        return Yup.object(camposComValidacao);
      }
    }
    return {};
  };

  salvarEncaminhamento = async (
    encaminhamentoId,
    situacao,
    enviarEncaminhamento
  ) => {
    const { dispatch } = store;

    const state = store.getState();
    const { encaminhamentoAEE, collapseLocalizarEstudante } = state;
    const { formsSecoesEncaminhamentoAEE } = encaminhamentoAEE;

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

    if (formsSecoesEncaminhamentoAEE?.length) {
      let todosOsFormsEstaoValidos = !enviarEncaminhamento;

      if (enviarEncaminhamento) {
        const promises = formsSecoesEncaminhamentoAEE.map(async item =>
          validaAntesDoSubmit(item.form())
        );

        await Promise.all(promises);

        todosOsFormsEstaoValidos =
          contadorFormsValidos ===
          formsSecoesEncaminhamentoAEE?.filter(a => a)?.length;
      }

      if (todosOsFormsEstaoValidos) {
        const valoresParaSalvar = {
          id: encaminhamentoId || 0,
          turmaId: dadosCollapseLocalizarEstudante.turmaId,
          alunoCodigo: dadosCollapseLocalizarEstudante.codigoAluno,
          situacao,
        };
        valoresParaSalvar.secoes = formsSecoesEncaminhamentoAEE.map(
          (item, secaoId) => {
            const form = item.form();
            const campos = form.state.values;
            const questoes = [];

            Object.keys(campos).forEach(key => {
              const questaoAtual = this.obterQuestaoPorId(
                item.dadosQuestionarioAtual,
                key
              );

              let questao = {
                questaoId: key,
                tipoQuestao: questaoAtual.tipoQuestao,
              };

              switch (questao.tipoQuestao) {
                case tipoQuestao.AtendimentoClinico:
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
                questao.tipoQuestao === tipoQuestao.Upload &&
                questao?.resposta?.length
              ) {
                questao.resposta.forEach(codigo => {
                  if (codigo) {
                    if (questaoAtual?.resposta?.length) {
                      const arquivoResposta = questaoAtual.resposta.find(
                        a => a?.arquivo?.codigo === codigo
                      );

                      if (arquivoResposta) {
                        questoes.push({
                          ...questao,
                          resposta: codigo,
                          respostaEncaminhamentoId: arquivoResposta.id,
                        });
                      } else {
                        questoes.push({
                          ...questao,
                          resposta: codigo,
                        });
                      }
                    } else {
                      questoes.push({
                        ...questao,
                        resposta: codigo,
                      });
                    }
                  }
                });
              } else if (
                questao.tipoQuestao === tipoQuestao.ComboMultiplaEscolha &&
                questao?.resposta?.length
              ) {
                questao.resposta.forEach(valorSelecionado => {
                  if (valorSelecionado) {
                    if (questaoAtual?.resposta?.length) {
                      const temResposta = questaoAtual.resposta.find(
                        a =>
                          String(a?.opcaoRespostaId) ===
                          String(valorSelecionado)
                      );

                      if (temResposta) {
                        questoes.push({
                          ...questao,
                          resposta: valorSelecionado,
                          respostaEncaminhamentoId: temResposta.id,
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
                  questao.respostaEncaminhamentoId =
                    questaoAtual.resposta[0].id;
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
            return {
              questoes,
              secaoId,
              concluido:
                Object.keys(form.getFormikContext().errors)?.length === 0,
            };
          }
        );

        valoresParaSalvar.secoes = valoresParaSalvar.secoes
          .filter(a => a)
          .filter(b => b.questoes?.length);

        if (valoresParaSalvar?.secoes?.length) {
          dispatch(setExibirLoaderEncaminhamentoAEE(true));

          const resposta = await api
            .post(`${urlPadrao}/salvar`, valoresParaSalvar)
            .catch(e => erros(e))
            .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));

          if (resposta?.status === 200) {
            return true;
          }
        }
      } else {
        dispatch(setExibirModalErrosEncaminhamento(true));
      }
    }
    return false;
  };

  excluirEncaminhamento = encaminhamentoId => {
    const url = `${urlPadrao}/${encaminhamentoId}`;
    return api.delete(url);
  };

  obterInformacoesEscolaresDoAluno = (codigoAluno, codigoTurma) => {
    const url = `v1/estudante/informacoes-escolares?codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    return api.get(url);
  };

  obterAusenciaMotivoPorAlunoTurmaBimestreAno = (
    codigoAluno,
    bimestre,
    codigoTurma,
    anoLetivo
  ) => {
    const url = `v1/calendarios/frequencias/ausencias-motivos?codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}&bimestre=${bimestre}&anoLetivo=${anoLetivo}`;
    return api.get(url);
  };

  podeCadastrarEncaminhamentoEstudante = codigoEstudante => {
    return api.get(`${urlPadrao}/estudante/${codigoEstudante}/pode-cadastrar`);
  };

  removerArquivo = arquivoCodigo => {
    return api.delete(`${urlPadrao}/arquivo?arquivoCodigo=${arquivoCodigo}`);
  };

  encerramentoEncaminhamentoAEE = (encaminhamentoId, motivoEncerramento) => {
    const parametro = { encaminhamentoId, motivoEncerramento };
    return api.post(`${urlPadrao}/encerrar`, parametro);
  };

  enviarParaAnaliseEncaminhamento = encaminhamentoId => {
    return api.post(`${urlPadrao}/enviar-analise/${encaminhamentoId}`);
  };
}

export default new ServicoEncaminhamentoAEE();
