import * as moment from 'moment';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';
import situacaoPlanoAEE from '~/dtos/situacaoPlanoAEE';
import tipoQuestao from '~/dtos/tipoQuestao';
import { store } from '~/redux';
import {
  setAtualizarDados,
  setExibirLoaderPlanoAEE,
  setExibirModalErrosPlano,
} from '~/redux/modulos/planoAEE/actions';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';

const urlPadrao = 'v1/plano-aee';

class ServicoPlanoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  existePlanoAEEEstudante = async codigoEstudante => {
    const resultado = await api
      .get(`${urlPadrao}/estudante/${codigoEstudante}/existe`)
      .catch(e => erros(e));

    if (resultado?.data) {
      return true;
    }
    return false;
  };

  obterPlanoPorId = planoId => {
    return api.get(`${urlPadrao}/${planoId}`);
  };

  obterVersaoPlanoPorId = versaoPlanoId => {
    return api.get(`${urlPadrao}/versao/${versaoPlanoId}`);
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
    const {
      questionarioDinamico,
      collapseLocalizarEstudante,
      planoAEE,
    } = state;
    const { formsQuestionarioDinamico } = questionarioDinamico;

    const { dadosCollapseLocalizarEstudante } = collapseLocalizarEstudante;
    const { planoAEEDados } = planoAEE;

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

    const formPlanoAEE = [formsQuestionarioDinamico?.[0]];

    if (formPlanoAEE?.length) {
      let todosOsFormsEstaoValidos = false;

      const promises = formPlanoAEE.map(async item =>
        validaAntesDoSubmit(item.form())
      );

      await Promise.all(promises);

      todosOsFormsEstaoValidos =
        contadorFormsValidos === formPlanoAEE?.filter(a => a)?.length;

      if (todosOsFormsEstaoValidos) {
        let questoesSalvar = formPlanoAEE.map(item => {
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
              case tipoQuestao.Periodo:
                if (campos[key]?.periodoInicio && campos[key]?.periodoFim) {
                  questao.resposta = JSON.stringify([
                    campos[key].periodoInicio
                      ? moment(campos[key].periodoInicio).format('DD/MM/YYYY')
                      : '',
                    campos[key].periodoFim
                      ? moment(campos[key].periodoFim).format('DD/MM/YYYY')
                      : '',
                  ]);
                } else {
                  questao.resposta = '';
                }
                break;
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
          id: planoAEEDados?.id ? planoAEEDados?.id : 0,
          turmaId: dadosCollapseLocalizarEstudante.turmaId,
          turmaCodigo: dadosCollapseLocalizarEstudante.codigoTurma,
          alunoCodigo: dadosCollapseLocalizarEstudante.codigoAluno,
          situacao: situacaoPlanoAEE.EmAndamento,
          questoes: questoesSalvar[0],
        };

        dispatch(setExibirLoaderPlanoAEE(true));
        const resposta = await api
          .post(`${urlPadrao}/salvar`, valoresParaSalvar)
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

        if (resposta?.status === 200) {
          return true;
        }
      } else {
        dispatch(setExibirModalErrosPlano(true));
      }
    }
    return false;
  };

  obterVersoes = (planoAEEId, reestruturacaoId) => {
    return api.get(
      `${urlPadrao}/${planoAEEId}/versoes/reestruturacao/${reestruturacaoId}`
    );
  };

  obterReestruturacoes = planoAEEId => {
    return api.get(`${urlPadrao}/${planoAEEId}/reestruturacoes`);
  };

  salvarReestruturacoes = params => {
    return api.post(
      `${urlPadrao}/${params.planoAEEId}/reestruturacoes`,
      params
    );
  };

  cliqueTabPlanoAEE = async (key, temId) => {
    const { dispatch } = store;

    const state = store.getState();
    const { questionarioDinamico } = state;
    const { questionarioDinamicoEmEdicao } = questionarioDinamico;

    if (questionarioDinamicoEmEdicao && key !== '1') {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );

      if (confirmou) {
        const salvou = await this.salvarPlano();
        if (salvou) {
          dispatch(setQuestionarioDinamicoEmEdicao(false));
          dispatch(setAtualizarDados(true));
          const mensagem = temId
            ? 'Registro alterado com sucesso'
            : 'Registro salvo com sucesso';
          sucesso(mensagem);
          return;
        }
      }
      QuestionarioDinamicoFuncoes.limparDadosOriginaisQuestionarioDinamico();
    }
  };
}

export default new ServicoPlanoAEE();
