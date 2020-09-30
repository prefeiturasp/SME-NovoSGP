import { Form, Formik } from 'formik';
import React, { useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as Yup from 'yup';
import { ModalConteudoHtml, SelectComponent } from '~/componentes';
import { setExibirModalCopiarConteudo } from '~/redux/modulos/anual/actions';

const ModalCopiarConteudoPlanoAnual = () => {
  const dispatch = useDispatch();

  const exibirModalCopiarConteudo = useSelector(
    store => store.planoAnual.exibirModalCopiarConteudo
  );

  const listaTurmasParaCopiar = useSelector(
    store => store.planoAnual.listaTurmasParaCopiar
  );

  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [bimestresSelecionados, setBimestresSelecionados] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [confirmacaoTurmasComPlano, setConfirmacaoTurmasComPlano] = useState(
    ''
  );
  const refForm = useRef();

  const listaBimestresMock = [
    { valor: 1, nome: '1 Bimestre' },
    { valor: 2, nome: '2 Bimestre' },
    { valor: 3, nome: '3 Bimestre' },
  ];

  const fecharCopiarConteudo = () => {
    setTurmasSelecionadas([]);
    setBimestresSelecionados([]);
    dispatch(setExibirModalCopiarConteudo(false));
    setConfirmacaoTurmasComPlano('');
    refForm.current.handleReset();
  };

  const copiar = async () => {
    // const plano = {
    //   // ...planoAnual,
    //   // bimestres: planoAnual.bimestres.filter(c =>
    //   //   bimestresSelecionados.includes(c.bimestre.toString())
    //   // ),
    // };
    // setExibirLoader(true);
    // servicoPlanoAnual
    //   .copiarConteudo({
    //     planoAnual: plano,
    //     idsTurmasDestino: turmasSelecionadas,
    //     bimestresDestino: bimestresSelecionados,
    //   })
    //   .then(() => {
    //     sucesso('Planejamento copiado com sucesso.');
    //     fecharCopiarConteudo();
    //   })
    //   .catch(e => {
    //     erros(e);
    //     // TODO TESTAR VEER SE QUANDO DA ERRO O ALERTA FICA ENCIMA DO MODAL
    //   })
    //   .finally(() => {
    //     setExibirLoader(false);
    //   });
  };

  const onChangeTurmasSelecionadas = turmas => {
    setTurmasSelecionadas(turmas);
    const turmasComPlano = listaTurmasParaCopiar.filter(
      c => turmas.includes(c.codTurma.toString()) && c.possuiPlano
    );
    // TODO Validar se vai continuar assim a consistência!
    if (turmasComPlano && turmasComPlano.length > 0) {
      setConfirmacaoTurmasComPlano(
        `As turmas: ${turmasComPlano
          .map(c => c.nomeTurma)
          .join(
            ','
          )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      );
    } else {
      setConfirmacaoTurmasComPlano('');
    }
  };

  const validacoes = Yup.object({
    turmas: Yup.string().required('Selecione ao menos uma turma.'),
    bimestres: Yup.string().required('Selecione ao menos um bimestre.'),
  });

  return (
    <Formik
      enableReinitialize
      initialValues={{
        turmas: [],
        bimestres: [],
      }}
      validationSchema={validacoes}
      onSubmit={() => copiar()}
      validateOnChange
      validateOnBlur
      ref={refForm}
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
            onConfirmacaoSecundaria={fecharCopiarConteudo}
            onClose={fecharCopiarConteudo}
            labelBotaoPrincipal="Copiar"
            tituloAtencao={confirmacaoTurmasComPlano && 'Atenção'}
            perguntaAtencao={confirmacaoTurmasComPlano}
            labelBotaoSecundario="Cancelar"
            titulo="Copiar Conteúdo"
            closable={false}
            loader={exibirLoader}
            desabilitarBotaoPrincipal={false}
          >
            <div>
              <SelectComponent
                label="Copiar para a(s) turma(s)"
                id="turmas"
                name="turmas"
                lista={listaTurmasParaCopiar || []}
                valueOption="codTurma"
                valueText="nomeTurma"
                valueSelect={turmasSelecionadas}
                multiple
                placeholder="Selecione uma ou mais turmas"
                onChange={onChangeTurmasSelecionadas}
                form={form}
              />
              <SelectComponent
                label="Copiar para o(s) bimestre(s)"
                id="bimestres"
                name="bimestres"
                lista={listaBimestresMock}
                valueOption="valor"
                valueText="nome"
                valueSelect={bimestresSelecionados}
                multiple
                placeholder="Selecione um ou mais bimestres"
                onChange={setBimestresSelecionados}
                form={form}
              />
            </div>
          </ModalConteudoHtml>
        </Form>
      )}
    </Formik>
  );
};

export default ModalCopiarConteudoPlanoAnual;
