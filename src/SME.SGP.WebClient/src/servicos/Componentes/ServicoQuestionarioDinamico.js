import { store } from '~/redux';
import {
  setFormsQuestionarioDinamico,
  setQuestionarioDinamicoEmEdicao,
} from '~/redux/modulos/questionarioDinamico/actions';

class ServicoQuestionarioDinamico {
  adicionarFormsQuestionarioDinamico = (
    obterForm,
    questionarioId,
    dadosQuestionarioAtual,
    secaoId
  ) => {
    const { dispatch } = store;
    const state = store.getState();
    const { questionarioDinamico } = state;
    const { formsQuestionarioDinamico } = questionarioDinamico;
    if (!formsQuestionarioDinamico) {
      const param = [];
      param[questionarioId] = {
        form: obterForm,
        dadosQuestionarioAtual,
        secaoId,
      };
      dispatch(setFormsQuestionarioDinamico(param));
    } else if (formsQuestionarioDinamico?.length) {
      const param = formsQuestionarioDinamico;
      param[questionarioId] = {
        form: obterForm,
        dadosQuestionarioAtual,
        secaoId,
      };
      dispatch(setFormsQuestionarioDinamico(param));
    }
  };

  resetarTelaDadosOriginais = () => {
    const { dispatch } = store;
    const state = store.getState();
    const { questionarioDinamico } = state;
    const { formsQuestionarioDinamico } = questionarioDinamico;
    if (formsQuestionarioDinamico?.length) {
      formsQuestionarioDinamico.forEach(item => {
        const form = item.form();
        form.resetForm();
      });
      dispatch(setQuestionarioDinamicoEmEdicao(false));
    }
  };
}

export default new ServicoQuestionarioDinamico();
