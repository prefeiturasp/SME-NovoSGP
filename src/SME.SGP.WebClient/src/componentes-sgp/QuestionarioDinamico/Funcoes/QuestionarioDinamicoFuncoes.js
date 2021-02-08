import { store } from '~/redux';
import {
  setFormsQuestionarioDinamico,
  setQuestionarioDinamicoEmEdicao,
} from '~/redux/modulos/questionarioDinamico/actions';

class QuestionarioDinamicoFuncoes {
  onChangeCamposComOpcaoResposta = (
    questaoAtual,
    form,
    valorAtualSelecionado
  ) => {
    const valoreAnteriorSelecionado = form.values[questaoAtual.id] || '';

    const opcaoAtual = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valorAtualSelecionado || '')
    );

    const opcaoAnterior = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valoreAnteriorSelecionado || '')
    );

    // TODO - Ajustar para quando tiver mais de um questão complementar!
    const questaoComplementarIdAtual =
      opcaoAtual?.questoesComplementares?.[0]?.id;

    const questaoComplementarIdAnterior =
      opcaoAnterior?.questoesComplementares?.[0]?.id;

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

  obterOpcaoRespostaPorId = (opcoesResposta, idComparacao) => {
    if (opcoesResposta?.length) {
      const opcaoResposta = opcoesResposta.find(
        item => String(item.id) === String(idComparacao)
      );
      return opcaoResposta;
    }
    return null;
  };

  obterValorCampoComplementarComboMultiplaEscolha = (
    form,
    valoresAnterioresSelecionado,
    questaoAtual
  ) => {
    const camposEmTela = Object.keys(form.values);

    let valorDigitadoCampoComplementar = '';

    const idOpcaoRespostaComValorDigitado = valoresAnterioresSelecionado.find(
      idCampo => {
        const opcaoResposta = this.obterOpcaoRespostaPorId(
          questaoAtual?.opcaoResposta,
          idCampo
        );

        if (opcaoResposta?.questoesComplementares[0]) {
          const temCampo = camposEmTela.find(
            c =>
              String(c) === String(opcaoResposta?.questoesComplementares[0]?.id)
          );
          return !!temCampo;
        }

        return null;
      }
    );

    if (idOpcaoRespostaComValorDigitado) {
      const opcaoRespostaComValorDigitado = this.obterOpcaoRespostaPorId(
        questaoAtual?.opcaoResposta,
        idOpcaoRespostaComValorDigitado
      );
      valorDigitadoCampoComplementar =
        form.values[
          opcaoRespostaComValorDigitado?.questoesComplementares[0]?.id
        ];
    }

    return valorDigitadoCampoComplementar;
  };

  obterIdOpcaoRespostaComComplementarObrigatoria = (
    valorAtualSelecionado,
    questaoAtual
  ) => {
    return valorAtualSelecionado.find(valor => {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        item => String(item.id) === String(valor)
      );

      if (opcaoAtual?.questoesComplementares[0]?.obrigatorio) {
        return true;
      }
      return false;
    });
  };

  obterIdOpcaoRespostaComComplementarNaoObrigatoria = (
    valorAtualSelecionado,
    questaoAtual
  ) => {
    return valorAtualSelecionado.find(valor => {
      const opcaoResposta = this.obterOpcaoRespostaPorId(
        questaoAtual?.opcaoResposta,
        valor
      );

      if (opcaoResposta?.questoesComplementares[0]?.obrigatorio) {
        return false;
      }
      return true;
    });
  };

  removerAddCampoComplementarComboMultiplaEscolha = (
    questaoAtual,
    form,
    idOpcaoComplementarAdicionar,
    idOpcaoComplementarRemover,
    valorDigitadoCampoComplementar
  ) => {
    const opcaoRespostaAdicionar = this.obterOpcaoRespostaPorId(
      questaoAtual?.opcaoResposta,
      idOpcaoComplementarAdicionar
    );

    const questaoComplementarAdicionarId =
      opcaoRespostaAdicionar?.questoesComplementares[0]?.id;

    if (questaoComplementarAdicionarId) {
      form.setFieldValue(
        questaoComplementarAdicionarId,
        valorDigitadoCampoComplementar
      );
      form.values[
        questaoComplementarAdicionarId
      ] = valorDigitadoCampoComplementar;
    }

    const opcaoRespostaRemover = this.obterOpcaoRespostaPorId(
      questaoAtual?.opcaoResposta,
      idOpcaoComplementarRemover
    );

    const questaoComplementarRemoverId =
      opcaoRespostaRemover?.questoesComplementares[0]?.id;

    if (questaoComplementarRemoverId) {
      delete form.values[questaoComplementarRemoverId];
      form.unregisterField(questaoComplementarRemoverId);
    }
  };

  onChangeCampoComboMultiplaEscolha = (
    questaoAtual,
    form,
    valoresAtuaisSelecionados
  ) => {
    if (
      !valoresAtuaisSelecionados?.length &&
      questaoAtual?.opcaoResposta?.length
    ) {
      questaoAtual.opcaoResposta.forEach(a => {
        if (a?.questoesComplementares[0]) {
          delete form.values[a.questoesComplementares[0].id];
          form.unregisterField(a.questoesComplementares[0].id);
        }
      });

      return;
    }

    const valoresAnterioresSelecionado = form.values[questaoAtual.id]?.length
      ? form.values[questaoAtual.id]
      : [];

    if (valoresAnterioresSelecionado?.length) {
      const valorDigitadoCampoComplementar = this.obterValorCampoComplementarComboMultiplaEscolha(
        form,
        valoresAnterioresSelecionado,
        questaoAtual
      );

      const idOpcaoRespostaAnteriorComplementarObrigatoria = this.obterIdOpcaoRespostaComComplementarObrigatoria(
        valoresAnterioresSelecionado,
        questaoAtual
      );

      const idOpcaoRespostaAtualComplementarObrigatoria = this.obterIdOpcaoRespostaComComplementarObrigatoria(
        valoresAtuaisSelecionados,
        questaoAtual
      );

      const idOpcaoRespostaAnteriorComplementarNaoObrigatoria = this.obterIdOpcaoRespostaComComplementarNaoObrigatoria(
        valoresAnterioresSelecionado,
        questaoAtual
      );

      const idOpcaoRespostaAtualComplementarNaoObrigatoria = this.obterIdOpcaoRespostaComComplementarNaoObrigatoria(
        valoresAtuaisSelecionados,
        questaoAtual
      );

      if (
        idOpcaoRespostaAnteriorComplementarObrigatoria &&
        idOpcaoRespostaAtualComplementarObrigatoria
      ) {
        this.removerAddCampoComplementarComboMultiplaEscolha(
          questaoAtual,
          form,
          idOpcaoRespostaAnteriorComplementarObrigatoria,
          idOpcaoRespostaAnteriorComplementarNaoObrigatoria ||
            idOpcaoRespostaAtualComplementarNaoObrigatoria,
          valorDigitadoCampoComplementar
        );
      } else if (
        idOpcaoRespostaAnteriorComplementarObrigatoria &&
        !idOpcaoRespostaAtualComplementarObrigatoria
      ) {
        this.removerAddCampoComplementarComboMultiplaEscolha(
          questaoAtual,
          form,
          idOpcaoRespostaAtualComplementarNaoObrigatoria,
          idOpcaoRespostaAnteriorComplementarObrigatoria,
          valorDigitadoCampoComplementar
        );
      } else if (
        !idOpcaoRespostaAnteriorComplementarObrigatoria &&
        idOpcaoRespostaAtualComplementarObrigatoria
      ) {
        this.removerAddCampoComplementarComboMultiplaEscolha(
          questaoAtual,
          form,
          idOpcaoRespostaAtualComplementarObrigatoria,
          idOpcaoRespostaAnteriorComplementarNaoObrigatoria,
          valorDigitadoCampoComplementar
        );
      }
    }
  };

  obterQuestaoPorId = (dados, idPesquisa) => {
    let questaoAtual = '';

    const obterQuestao = item => {
      if (!questaoAtual) {
        if (String(item.id) === String(idPesquisa)) {
          questaoAtual = item;
        } else if (item?.opcaoResposta?.length) {
          item.opcaoResposta.forEach(opcaoResposta => {
            if (opcaoResposta.questoesComplementares[0]) {
              obterQuestao(opcaoResposta.questoesComplementares[0]);
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

export default new QuestionarioDinamicoFuncoes();
