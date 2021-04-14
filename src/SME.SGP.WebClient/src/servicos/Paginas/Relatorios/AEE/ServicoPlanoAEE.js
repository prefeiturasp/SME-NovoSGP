import * as moment from 'moment';
import QuestionarioDinamicoFuncoes from '~/componentes-sgp/QuestionarioDinamico/Funcoes/QuestionarioDinamicoFuncoes';
import situacaoPlanoAEE from '~/dtos/situacaoPlanoAEE';
import tipoQuestao from '~/dtos/tipoQuestao';
import { store } from '~/redux';
import {
  limparDadosDevolutiva,
  setAtualizarDados,
  setDevolutivaEmEdicao,
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

  obterPlanoPorId = (planoId, turmaCodigo) => {
    let url = `${urlPadrao}/${planoId}`;
    if (turmaCodigo) {
      url = `${url}?turmaCodigo=${turmaCodigo}`;
    }
    return api.get(url);
  };

  obterVersaoPlanoPorId = (versaoPlanoId, questionarioId, turmaCodigo) => {
    let url = `${urlPadrao}/versao/${versaoPlanoId}?questionarioId=${questionarioId}`;

    if (turmaCodigo) {
      url = `${url}&turmaCodigo=${turmaCodigo}`;
    }
    return api.get(url);
  };

  obterPlanoPorCodigoEstudante = codigoEstudante => {
    return api.get(`${urlPadrao}/estudante/${codigoEstudante}`);
  };

  obterQuestionario = (questionarioId, planoId, codigoAluno, codigoTurma) => {
    let url = `${urlPadrao}/questionario?questionarioId=${questionarioId}&codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    if (planoId) {
      url = `${url}&planoId=${planoId}`;
    }
    return api.get(url);
  };

  salvarPlano = async retornarPlanoId => {
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
          situacao: planoAEEDados?.situacao,
          questoes: questoesSalvar[0],
        };

        dispatch(setExibirLoaderPlanoAEE(true));
        const resposta = await api
          .post(`${urlPadrao}/salvar`, valoresParaSalvar)
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderPlanoAEE(false)));

        if (resposta?.status === 200) {
          if (retornarPlanoId) {
            return resposta?.data?.planoId;
          }
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
    const {
      planoAEEDados,
      questionarioDinamicoEmEdicao,
    } = questionarioDinamico;

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
    if (!questionarioDinamicoEmEdicao && key !== '3') {
      const salvou = await this.escolherAcaoDevolutivas(
        planoAEEDados?.situacao
      );
      if (salvou) {
        dispatch(setAtualizarDados(true));
      }

      dispatch(limparDadosDevolutiva());
      dispatch(setDevolutivaEmEdicao(false));
    }
  };

  obterDevolutiva = planoAeeId => {
    return api.get(`${urlPadrao}/${planoAeeId}/devolutiva`);
  };

  encerrarPlano = planoAeeId => {
    return api.post(`${urlPadrao}/encerrar-plano?planoAeeId=${planoAeeId}`);
  };

  salvarDevolutivaCP = () => {
    const { planoAEE } = store.getState();
    const { planoAEEDados, parecerCoordenacao } = planoAEE;
    return api.post(`${urlPadrao}/${planoAEEDados.id}/devolutiva/cp`, {
      parecer: parecerCoordenacao,
    });
  };

  salvarDevolutivaPAAI = () => {
    const { planoAEE } = store.getState();
    const { planoAEEDados, parecerPAAI } = planoAEE;
    return api.post(`${urlPadrao}/${planoAEEDados.id}/devolutiva/paai`, {
      parecer: parecerPAAI,
    });
  };

  atribuirResponsavel = () => {
    const { planoAEE } = store.getState();
    const { planoAEEDados, dadosAtribuicaoResponsavel } = planoAEE;
    return api.post(`${urlPadrao}/atribuir-responsavel`, {
      planoAEEId: planoAEEDados.id,
      responsavelRF: dadosAtribuicaoResponsavel.codigoRF,
    });
  };

  escolherAcaoDevolutivas = async () => {
    const { dispatch, getState } = store;
    const { planoAEE } = getState();
    const {
      dadosDevolutiva,
      planoAEEDados,
      dadosAtribuicaoResponsavel,
      devolutivaEmEdicao,
    } = planoAEE;
    if (devolutivaEmEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
        if (
          (planoAEEDados.situacao === situacaoPlanoAEE.DevolutivaCP ||
            planoAEEDados.situacao === situacaoPlanoAEE.AtribuicaoPAAI) &&
          !dadosAtribuicaoResponsavel?.codigoRF
        ) {
          await this.salvarDevolutivaCP();
          dispatch(setAtualizarDados(true));
          sucesso('Devolutiva realizada com sucesso');
          return true;
        }
        if (planoAEEDados.situacao === situacaoPlanoAEE.AtribuicaoPAAI) {
          await this.atribuirResponsavel();
          sucesso('Atribuição do responsável realizada com sucesso');
          return true;
        }
        if (
          planoAEEDados?.situacao === situacaoPlanoAEE.DevolutivaPAAI &&
          dadosDevolutiva?.podeEditarParecerPAAI
        ) {
          await this.salvarDevolutivaPAAI();
          sucesso('Encerramento do plano realizado com sucesso');
          return true;
        }
      }
    }
    return false;
  };

  obterDadosObservacoes = id => {
    // TODO - Validar!
    return api.get(`${urlPadrao}/${id}/observacoes`);
  };

  obterNofiticarUsuarios = ({ turmaId, observacaoId = '' }) => {
    // TODO - Validar!
    return api.get(
      `${urlPadrao}/notificacoes/usuarios?turmaId=${turmaId}&observacaoId=${observacaoId}`
    );
  };

  salvarEditarObservacao = (id, dados) => {
    // TODO - Validar!
    if (id) {
      const url = `${urlPadrao}/observacoes/${id}`;
      return api.put(url, dados);
    }

    const url = `${urlPadrao}/${id}/observacoes`;
    return api.post(url, dados);
  };

  excluirObservacao = id => {
    // TODO - Validar!
    return api.delete(`${urlPadrao}/observacoes/${id}`);
  };
}

export default new ServicoPlanoAEE();
