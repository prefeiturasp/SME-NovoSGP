import { Form, Formik } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as Yup from 'yup';
import { ModalConteudoHtml, SelectComponent, Loader } from '~/componentes';
import { setExibirModalCopiarConteudo } from '~/redux/modulos/anual/actions';
import { erros, sucesso } from '~/servicos';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';

const ModalCopiarConteudoPlanoAnual = () => {
  const dispatch = useDispatch();

  const exibirModalCopiarConteudo = useSelector(
    store => store.planoAnual.exibirModalCopiarConteudo
  );

  const listaTurmasParaCopiar = useSelector(
    store => store.planoAnual.listaTurmasParaCopiar
  );

  const planejamentoAnualId = useSelector(
    store => store.planoAnual.planejamentoAnualId
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  const [refForm, setRefForm] = useState({});
  const[listaTurmasParaCopiarAgrupada, setListaTurmasParaCopiarAgrupada] = useState([]);
  const [listaBimestres, setListaBimestres] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [confirmacaoTurmasComPlano, setConfirmacaoTurmasComPlano] = useState(
    ''
  );

  const obterPeriodosEscolaresParaCopia = useCallback(() => {
    ServicoPlanoAnual.obterPeriodosEscolaresParaCopia(planejamentoAnualId)
      .then(resposta => {
        if (resposta && resposta.data && resposta.data.length) {
          const lista = resposta.data.map(item => {
            return {
              valor: item.id,
              bimestre: item.bimestre,
              nome: `${item.bimestre}º Bimestre`,
            };
          });

          if (lista.length > 1) {
            lista.unshift({ nome: 'Todos', valor: '0' });
          }
          setListaBimestres(lista);
        }
      })
      .catch(e => {
        setListaBimestres([]);
        erros(e);
      });
  }, [planejamentoAnualId]);

  useEffect(() => {
    if (planejamentoAnualId && exibirModalCopiarConteudo) {
      obterPeriodosEscolaresParaCopia();
    }
  }, [
    exibirModalCopiarConteudo,
    planejamentoAnualId,
    obterPeriodosEscolaresParaCopia,
  ]);

  const resetarDadosModal = form => {
    setConfirmacaoTurmasComPlano('');
    form.resetForm();
  };  

  const fecharCopiarConteudo = form => {
    dispatch(setExibirModalCopiarConteudo(false));
    resetarDadosModal(form);
  };

  const copiar = async (valores, form) => {
    const ehTodasTurmas = valores.bimestres.includes('0');
    let bimestres = [...valores.bimestres];

    if (ehTodasTurmas) {
      bimestres = listaBimestres
        .filter(item => item.valor !== '0')
        .map(item => item.valor);
    }
    const params = {
      componenteCurricularId: componenteCurricular.codigoComponenteCurricular,
      turmasDestinoIds: valores.turmas,
      planejamentoPeriodosEscolaresIds: bimestres,
    };

    setExibirLoader(true);
    ServicoPlanoAnual.copiarConteudo(params)
      .then(() => {
        sucesso('Cópia do planejamento anual realizada com sucesso.');
        fecharCopiarConteudo(form);
      })
      .catch(e => erros(e))
      .finally(() => {
        setExibirLoader(false);
      });
  };

  const agruparListaTurmas = turmas => {
    let listRet = [];
    turmas.map(item => {
      if(listRet.filter(it => it.codTurma == item.codTurma).length == 0)
        listRet.push(item);
    });
    return listRet;
  };

  useEffect(() =>{
    if (listaTurmasParaCopiar){
      const lstTurma = listaTurmasParaCopiar.map(item => {
        return ({nomeTurma :item.nomeTurma ,codTurma :item.codTurma});
      });    
      setListaTurmasParaCopiarAgrupada(agruparListaTurmas(lstTurma));
    }    
  },[listaTurmasParaCopiar]); 

  const onChangeTurmasSelecionadas = turmas => {};

  const validacoes = Yup.object({
    turmas: Yup.string().required('Selecione ao menos uma turma.'),
    bimestres: Yup.string().required('Selecione ao menos um bimestre.'),
  });

  const onChangeBimestre = (bimestres, form) => {
    const opcaoTodosJaSelecionado = form.values.bimestres.includes('0');
    if (opcaoTodosJaSelecionado) {
      const listaSemOpcaoTodos = bimestres.filter(bi => bi !== '0');
      form.setFieldValue('bimestres', listaSemOpcaoTodos);
    } else if (bimestres.includes('0')) {
      form.setFieldValue('bimestres', ['0']);
    }

    const _form = refForm?.state?.values;
    const turmas = _form.turmas;

    const bimestresSelecionados = listaBimestres.filter( bt => bimestres.includes(bt.valor.toString()));    

    let turmasComPlano = listaTurmasParaCopiar.filter(
      c => turmas.includes(c.codTurma.toString()) &&
           c.codigoComponenteCurricular === componenteCurricular.codigoComponenteCurricular &&
           c.possuiPlano &&
           bimestresSelecionados &&
           bimestresSelecionados.length > 0 &&
           (bimestresSelecionados.filter(b => b.bimestre == c.bimestre).length > 0 ||
            bimestresSelecionados[0].valor === '0')
    );      
    
    if (turmasComPlano && turmasComPlano.length > 0) {
      setConfirmacaoTurmasComPlano(
        `As turmas: ${agruparListaTurmas(turmasComPlano)
          .map(c => c.nomeTurma)
          .join(
            ','
          )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      );
    } else {
      setConfirmacaoTurmasComPlano('');
    }
  };

  return (
    <Formik
      enableReinitialize
      initialValues={{
        turmas: [],
        bimestres: [],
      }}
      validationSchema={validacoes}
      onSubmit={(valores, form) => {
        copiar(valores, form);
      }}
      ref={refFormik => setRefForm(refFormik)}
      validateOnChange
      validateOnBlur
    >
      {form => (
        <Form>
          <ModalConteudoHtml
            key="copiarConteudo"
            visivel={exibirModalCopiarConteudo}
            onConfirmacaoPrincipal={e => {
              form.validateForm().then(() => {
                form.handleSubmit(e);
              });
            }}
            onConfirmacaoSecundaria={() => resetarDadosModal(form)}
            onClose={() => fecharCopiarConteudo(form)}
            labelBotaoPrincipal="Copiar"
            tituloAtencao={confirmacaoTurmasComPlano && 'Atenção'}
            perguntaAtencao={confirmacaoTurmasComPlano}
            labelBotaoSecundario="Cancelar"
            titulo="Copiar Conteúdo"
            closable
            loader={exibirLoader}
            esconderBotoes={exibirLoader}
            desabilitarBotaoPrincipal={false}
          >
            <Loader loading={exibirLoader} ignorarTip>
              <SelectComponent
                label="Copiar para a(s) turma(s)"
                id="turmas"
                name="turmas"
                lista={listaTurmasParaCopiarAgrupada || []}
                valueOption="codTurma"
                valueText="nomeTurma"
                multiple
                placeholder="Selecione uma ou mais turmas"
                onChange={onChangeTurmasSelecionadas}
                form={form}
              />
              <SelectComponent
                label="Copiar para o(s) bimestre(s)"
                id="bimestres"
                name="bimestres"
                lista={listaBimestres}
                valueOption="valor"
                valueText="nome"
                multiple
                placeholder="Selecione um ou mais bimestres"
                onChange={valores => onChangeBimestre(valores, form)}
                form={form}
              />
            </Loader>
          </ModalConteudoHtml>
        </Form>
      )}
    </Formik>
  );
};

export default ModalCopiarConteudoPlanoAnual;
