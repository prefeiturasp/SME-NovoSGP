import { store } from '~/redux';
import { setFormsQuestionarioDinamico } from '~/redux/modulos/questionarioDinamico/actions';

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
}

export default new ServicoQuestionarioDinamico();
