import { store } from '~/redux';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';

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

        if (opcaoResposta?.questaoComplementar) {
          const temCampo = camposEmTela.find(
            c => String(c) === String(opcaoResposta?.questaoComplementar?.id)
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
        form.values[opcaoRespostaComValorDigitado?.questaoComplementar?.id];
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

      if (opcaoAtual?.questaoComplementar?.obrigatorio) {
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

      if (opcaoResposta?.questaoComplementar?.obrigatorio) {
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
      opcaoRespostaAdicionar?.questaoComplementar?.id;

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
      opcaoRespostaRemover?.questaoComplementar?.id;

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
        if (a?.questaoComplementar) {
          delete form.values[a.questaoComplementar.id];
          form.unregisterField(a.questaoComplementar.id);
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
}

export default new QuestionarioDinamicoFuncoes();
