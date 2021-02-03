import { store } from '~/redux';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';

class QuestionarioDinamicoFuncoes {
  onChangeCamposComOpcaoResposta = (
    questaoAtual,
    form,
    valorAtualSelecionado,
    onChange
  ) => {
    const valoreAnteriorSelecionado = form.values[questaoAtual.id] || '';

    const opcaoAtual = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valorAtualSelecionado || '')
    );

    const opcaoAnterior = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valoreAnteriorSelecionado || '')
    );

    const questaoComplementarIdAtual = opcaoAtual?.questaoComplementar?.id;
    const questaoComplementarIdAnterior =
      opcaoAnterior?.questaoComplementar?.id;

    if (questaoComplementarIdAtual !== questaoComplementarIdAnterior) {
      if (questaoComplementarIdAtual) {
        form.setFieldValue(
          questaoComplementarIdAtual,
          form.values[questaoComplementarIdAnterior]
        );
        form.values[questaoComplementarIdAtual] =
          form.values[questaoComplementarIdAnterior];
      }
      delete form.values[questaoComplementarIdAnterior];
      form.unregisterField(questaoComplementarIdAnterior);
    }
    onChange();
  };

  limparDadosOriginaisQuestionarioDinamico = () => {
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

export default new QuestionarioDinamicoFuncoes();
