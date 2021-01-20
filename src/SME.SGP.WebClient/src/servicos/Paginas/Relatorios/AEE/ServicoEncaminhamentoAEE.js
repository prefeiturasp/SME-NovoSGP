import { RotasDto } from '~/dtos';
import tipoQuestao from '~/dtos/tipoQuestao';
import { store } from '~/redux';
import {
  setDadosModalAviso,
  setExibirLoaderEncaminhamentoAEE,
  setExibirModalAviso,
  setFormsSecoesEncaminhamentoAEE,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

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

  obterDadosEstudante = (codigoAluno, anoLetivo) => {
    const url = `v1/estudante/${codigoAluno}/anosLetivos/${anoLetivo}`;
    return api.get(url);
  };

  obterSecoesPorEtapaDeEncaminhamentoAEE = etapa => {
    const url = `${urlPadrao}/secoes?etapa=${etapa}`;
    return api.get(url);
  };

  obterQuestionario = (questionarioId, encaminhamentoId) => {
    let url = `${urlPadrao}/questionario?questionarioId=${questionarioId}`;
    if (encaminhamentoId) {
      url = `${url}&encaminhamentoId=${encaminhamentoId}`;
    }
    return api.get(url);
  };

  obterEncaminhamentoPorId = encaminhamentoId => {
    return api.get(`${urlPadrao}/${encaminhamentoId}`);
  };

  addFormsSecoesEncaminhamentoAEE = (
    obterForm,
    questionarioId,
    dadosQuestionarioAtual,
    tiposQuestaoPorIdQuestao
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
        tiposQuestaoPorIdQuestao,
      };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    } else if (formsSecoesEncaminhamentoAEE?.length) {
      const param = formsSecoesEncaminhamentoAEE;
      param[questionarioId] = {
        form: obterForm,
        dadosQuestionarioAtual,
        tiposQuestaoPorIdQuestao,
      };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    }
  };

  salvarEncaminhamento = async encaminhamentoId => {
    const { dispatch } = store;

    const state = store.getState();
    const { encaminhamentoAEE } = state;
    const {
      formsSecoesEncaminhamentoAEE,
      dadosSecaoLocalizarEstudante,
    } = encaminhamentoAEE;

    // TODO REFAZER ASYNC
    // let contadorFormsValidos = 0;

    // const validaAntesDoSubmit = refForm => {
    //   let arrayCampos = [];

    //   if (refForm?.fields && Object.keys(refForm?.fields)?.length) {
    //     arrayCampos = Object.keys(refForm.fields);
    //   }

    //   arrayCampos.forEach(campo => {
    //     refForm.setFieldTouched(campo, true, true);
    //   });
    //   refForm.validateForm().then(() => {
    //     if (
    //       refForm.getFormikContext().isValid ||
    //       Object.keys(refForm.getFormikContext().errors).length === 0
    //     ) {
    //       contadorFormsValidos += 1;
    //     }
    //   });
    // };

    if (formsSecoesEncaminhamentoAEE?.length) {
      // formsSecoesEncaminhamentoAEE.forEach(item => {
      //   const form = item.form();
      //   validaAntesDoSubmit(form);
      // });

      const valoresParaSalvar = {
        id: encaminhamentoId || 0,
        turmaId: dadosSecaoLocalizarEstudante.turmaId,
        alunoCodigo: dadosSecaoLocalizarEstudante.codigoAluno,
      };
      valoresParaSalvar.secoes = formsSecoesEncaminhamentoAEE.map(
        (item, secaoId) => {
          const form = item.form();
          const campos = form.state.values;
          const questoes = [];

          Object.keys(campos).forEach(key => {
            const questao = {
              questaoId: key,
              tipoQuestao: item?.tiposQuestaoPorIdQuestao[key],
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
              default:
                questao.resposta = campos[key] || '';
                break;
            }

            if (
              questao.tipoQuestao === tipoQuestao.Upload &&
              questao?.resposta?.length
            ) {
              questao.resposta.forEach(b => {
                questoes.push({ ...questao, resposta: b });
              });
            } else {
              questoes.push(questao);
            }
          });
          return {
            questoes,
            secaoId,
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
          sucesso('Registro salvo com sucesso');
          history.push(RotasDto.RELATORIO_AEE_ENCAMINHAMENTO);
        }
      }
    }
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

  removerArquivo = arquivoCodigo => {
    return api.delete(`${urlPadrao}/arquivo?arquivoCodigo=${arquivoCodigo}`);
  };
}

export default new ServicoEncaminhamentoAEE();
