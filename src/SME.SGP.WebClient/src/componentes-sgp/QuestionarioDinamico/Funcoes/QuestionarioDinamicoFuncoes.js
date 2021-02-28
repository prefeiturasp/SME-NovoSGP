import { groupBy } from 'lodash';
import { store } from '~/redux';
import { setListaSecoesEmEdicao } from '~/redux/modulos/encaminhamentoAEE/actions';
import {
  setFormsQuestionarioDinamico,
  setQuestionarioDinamicoEmEdicao,
  setResetarTabela,
} from '~/redux/modulos/questionarioDinamico/actions';

class QuestionarioDinamicoFuncoes {
  agruparCamposDuplicados = (data, campo) => {
    if (data?.length) {
      const groups = groupBy(data, campo);
      const results = Object.entries(groups).map(([key, values]) => {
        return { questaoNome: key, questoesDuplicadas: values };
      });

      return results.filter(r => r.questoesDuplicadas.length > 1);
    }
    return [];
  };

  adicionarCampoNovo = (form, idCampoNovo, valor) => {
    const camposEmTela = Object.keys(form.values);

    const campoEstaEmTela = camposEmTela.find(c => c === String(idCampoNovo));

    if (!campoEstaEmTela) {
      form.setFieldValue(idCampoNovo, valor);
      form.values[idCampoNovo] = valor;
    }
  };

  removerCampo = (form, idCampoNovo) => {
    const camposEmTela = Object.keys(form.values);

    const campoEstaEmTela = camposEmTela.find(c => c === String(idCampoNovo));

    if (campoEstaEmTela) {
      delete form.values[idCampoNovo];
      form.unregisterField(idCampoNovo);
    }
  };

  obterTodosCamposComplementares = (valorAtualSelecionado, questaoAtual) => {
    const campos = [];
    valorAtualSelecionado.forEach(a => {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        c => String(c.id) === String(a || '')
      );

      if (opcaoAtual?.questoesComplementares?.length) {
        opcaoAtual.questoesComplementares.forEach(questao => {
          const temCampo = campos.find(q => q.id === questao.id);

          if (!temCampo) {
            campos.push(questao);
          }
        });
      }
    });

    return campos;
  };

  adicionarRemoverCamposDuplicados = (
    form,
    camposDuplicados,
    valoresCamposComplemetares
  ) => {
    camposDuplicados.forEach(c => {
      // Na base vai ter somente 2 campos com mesmo nome para essa rotina 1 obrigatório e outro não!
      const campoRemover = c.questoesDuplicadas?.find(co => !co.obrigatorio);
      const campoRenderizar = c.questoesDuplicadas?.find(co => co.obrigatorio);

      const valorCampoRemovido = valoresCamposComplemetares.find(
        valorCampo =>
          valorCampo?.id === campoRemover?.id ||
          valorCampo?.nome === campoRemover?.nome
      );

      if (campoRemover) {
        this.removerCampo(form, campoRemover.id);
      }

      if (campoRenderizar) {
        this.adicionarCampoNovo(
          form,
          campoRenderizar?.id,
          valorCampoRemovido?.valor
        );
      }
    });
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
      dispatch(setResetarTabela(true));
    }
  };

  obterCamposNaoDuplicados = (campos, camposDuplicados) => {
    return campos.filter(ca => {
      const estaDuplicado = camposDuplicados.find(cd =>
        cd.questoesDuplicadas.find(v => v.id === ca.id)
      );

      if (!estaDuplicado) {
        return true;
      }
      return false;
    });
  };

  adicionarCamposNaoDuplicados = (
    form,
    campos,
    camposDuplicados,
    valoresCamposComplemetares
  ) => {
    const camposNaoDuplicados = this.obterCamposNaoDuplicados(
      campos,
      camposDuplicados
    );

    if (camposNaoDuplicados?.length) {
      camposNaoDuplicados.forEach(a => {
        const valorCampoRemovido = valoresCamposComplemetares.find(
          valorCampo => valorCampo?.id === a?.id
        );
        this.adicionarCampoNovo(form, a.id, valorCampoRemovido?.valor);
      });
    }
  };

  obterValoresCamposComplemetares = (
    form,
    questaoAtual,
    valoreAnteriorSelecionado
  ) => {
    const valores = [];
    const camposEmTela = Object.keys(form.values);

    valoreAnteriorSelecionado.forEach(v => {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        c => String(c.id) === String(v || '')
      );

      if (opcaoAtual?.questoesComplementares?.length) {
        opcaoAtual.questoesComplementares.forEach(questao => {
          const temCampoEmTela = camposEmTela.find(
            idCampo => idCampo === String(questao.id)
          );

          if (temCampoEmTela) {
            const jaEstaNaLista = valores.find(l => l.id === questao.id);
            if (!jaEstaNaLista) {
              valores.push({
                id: questao.id,
                valor: form.values[questao.id],
                nome: questao.nome.trim(),
              });
            }
          }
        });
      }
    });

    return valores;
  };

  removerTodosCamposComplementares = (
    valoreAnteriorSelecionado,
    questaoAtual,
    form
  ) => {
    valoreAnteriorSelecionado.forEach(id => {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        c => String(c.id) === String(id || '')
      );

      if (opcaoAtual?.questoesComplementares?.length) {
        opcaoAtual.questoesComplementares.forEach(questao => {
          this.removerCampo(form, questao.id);
        });
      }
    });
  };

  onChangeCampoCheckboxOuComboMultiplaEscolha = (
    questaoAtual,
    form,
    valorAtualSelecionado
  ) => {
    const valoreAnteriorSelecionado = form.values[questaoAtual.id] || [];

    const valoresCamposComplemetares = this.obterValoresCamposComplemetares(
      form,
      questaoAtual,
      valoreAnteriorSelecionado
    );

    if (valoreAnteriorSelecionado?.length) {
      this.removerTodosCamposComplementares(
        valoreAnteriorSelecionado,
        questaoAtual,
        form
      );
    }

    const todosCamposComplementares = this.obterTodosCamposComplementares(
      valorAtualSelecionado,
      questaoAtual
    );

    const camposSemEspaco = todosCamposComplementares.map(m => {
      return { ...m, nome: m.nome.trim() };
    });

    const camposDuplicados = this.agruparCamposDuplicados(
      camposSemEspaco,
      'nome'
    );

    if (camposDuplicados?.length) {
      this.adicionarRemoverCamposDuplicados(
        form,
        camposDuplicados,
        valoresCamposComplemetares
      );
    }

    if (camposSemEspaco?.length && camposDuplicados?.length) {
      this.adicionarCamposNaoDuplicados(
        form,
        camposSemEspaco,
        camposDuplicados
      );
    } else {
      camposSemEspaco.forEach(a => {
        const valorCampoRemovido = valoresCamposComplemetares.find(
          valorCampo => valorCampo?.id === a?.id || valorCampo?.nome === a?.nome
        );

        this.adicionarCampoNovo(form, a.id, valorCampoRemovido?.valor);
      });
    }
  };

  onChangeCamposComOpcaoResposta = (
    questaoAtual,
    form,
    valorAtualSelecionado
  ) => {
    const valorAnteriorSelecionado = form.values[questaoAtual.id] || '';

    const valoresCamposComplemetares = this.obterValoresCamposComplemetares(
      form,
      questaoAtual,
      [valorAnteriorSelecionado]
    );

    const opcaoAtual = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valorAtualSelecionado || '')
    );

    const opcaoAnterior = questaoAtual?.opcaoResposta.find(
      c => String(c.id) === String(valorAnteriorSelecionado || '')
    );

    const idsQuestoesComplementaresAnterior = opcaoAnterior?.questoesComplementares.map(
      q => q.id
    );

    const idsQuestoesComplementaresAtual = opcaoAtual?.questoesComplementares.map(
      q => q.id
    );

    const idsQuestoesExclusao = idsQuestoesComplementaresAnterior?.filter(
      idComplentar => {
        if (!idsQuestoesComplementaresAtual?.includes(idComplentar)) {
          return true;
        }
        return false;
      }
    );

    if (idsQuestoesExclusao?.length) {
      idsQuestoesExclusao.forEach(id => {
        delete form.values[id];
        form.unregisterField(id);
      });
    }

    const idsQuestoesAdicionar = idsQuestoesComplementaresAtual?.filter(
      idComplentar => {
        if (!idsQuestoesComplementaresAnterior?.includes(idComplentar)) {
          return true;
        }
        return false;
      }
    );

    if (idsQuestoesAdicionar?.length) {
      idsQuestoesAdicionar.forEach(id => {
        const qAtual = opcaoAtual.questoesComplementares.find(q => q.id === id);

        const valorCampoRemovido = valoresCamposComplemetares.find(
          valorCampo =>
            valorCampo?.id === id || valorCampo?.nome === qAtual?.nome
        );

        form.setFieldValue(id, valorCampoRemovido?.valor || '');
        form.values[id] = valorCampoRemovido?.valor || '';
      });
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

  obterQuestaoPorId = (dados, idPesquisa) => {
    let questaoAtual = '';

    const obterQuestao = item => {
      if (!questaoAtual) {
        if (String(item.id) === String(idPesquisa)) {
          questaoAtual = item;
        } else if (item?.opcaoResposta?.length) {
          item.opcaoResposta.forEach(opcaoResposta => {
            if (opcaoResposta?.questoesComplementares?.length) {
              opcaoResposta.questoesComplementares.forEach(questao => {
                obterQuestao(questao);
              });
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
    // TODO Usado o questionarioId para setar o indice do arra.
    // Caso trocar para push no array de form, validar se vai duplicar os forms!
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

  obterListaDiasSemana = () => {
    return [
      {
        valor: 'Domingo',
        desc: 'Domingo',
        ordem: 1,
      },
      {
        valor: 'Segunda',
        desc: 'Segunda',
        ordem: 2,
      },
      {
        valor: 'Terça',
        desc: 'Terça',
        ordem: 3,
      },
      {
        valor: 'Quarta',
        desc: 'Quarta',
        ordem: 4,
      },
      {
        valor: 'Quinta',
        desc: 'Quinta',
        ordem: 5,
      },
      {
        valor: 'Sexta',
        desc: 'Sexta',
        ordem: 6,
      },
      {
        valor: 'Sábado',
        desc: 'Sábado',
        ordem: 7,
      },
    ];
  };

  ordenarDiasDaSemana = diasDaSemanaAtual => {
    const listaOrdenada = [];

    const diasDaSemana = this.obterListaDiasSemana();

    diasDaSemana.forEach(dia => {
      const temEssesDias = diasDaSemanaAtual.filter(
        item => item.diaSemana === dia.valor
      );
      if (temEssesDias?.length) {
        temEssesDias.forEach(d => {
          listaOrdenada.push(d);
        });
      }
    });
    return listaOrdenada;
  };
}

export default new QuestionarioDinamicoFuncoes();
