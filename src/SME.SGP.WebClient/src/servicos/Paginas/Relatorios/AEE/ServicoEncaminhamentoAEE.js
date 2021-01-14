import { RotasDto } from '~/dtos';
import { store } from '~/redux';
import {
  setDadosModalAviso,
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

  addFormsSecoesEncaminhamentoAEE = (
    obterForm,
    questionarioId,
    dadosQuestionarioAtual
  ) => {
    const { dispatch } = store;
    const state = store.getState();
    const { encaminhamentoAEE } = state;
    const { formsSecoesEncaminhamentoAEE } = encaminhamentoAEE;
    if (!formsSecoesEncaminhamentoAEE) {
      const param = [];
      param[questionarioId] = { form: obterForm, dadosQuestionarioAtual };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    } else if (formsSecoesEncaminhamentoAEE?.length) {
      const param = formsSecoesEncaminhamentoAEE;
      param[questionarioId] = { form: obterForm, dadosQuestionarioAtual };
      dispatch(setFormsSecoesEncaminhamentoAEE(param));
    }
  };

  salvarEncaminhamento = async encaminhamentoId => {
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
          const questoes = Object.keys(campos).map(key => {
            const questao = item.dadosQuestionarioAtual.find(
              q => String(q.id) === String(key)
            );
            return {
              questaoId: key,
              resposta: String(campos[key]),
              tipoQuestao: questao?.tipoQuestao,
            };
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
        const resposta = await api
          .post(`${urlPadrao}/salvar`, valoresParaSalvar)
          .catch(e => erros(e));

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
}

export default new ServicoEncaminhamentoAEE();
