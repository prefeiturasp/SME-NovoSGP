import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { ModalConteudoHtml, RadioGroupButton } from '~/componentes';

const ModalSelecionarAulaFrequenciaPlanoAula = props => {
  const {
    visivel,
    aulasParaSelecionar,
    onClickFecharModal,
    onClickSelecionarAula,
  } = props;

  const opcoesTipoAula = [
    { label: 'Aula do titular', value: '1' },
    { label: ' Aula CJ', value: '2' },
  ];
  const [refForm, setRefForm] = useState({});
  const inicial = {
    tipoAula: '',
  };
  const [valoresIniciais, setValoresIniciais] = useState(inicial);

  const validacoes = Yup.object({
    tipoAula: Yup.string().required('Campo obrigatório'),
  });

  const onClickConfirmar = valores => {
    let aula = null;
    if (valores.tipoAula === '2') {
      aula = aulasParaSelecionar.find(item => item.aulaCJ === true);
    } else {
      aula = aulasParaSelecionar.find(item => item.aulaCJ === false);
    }
    onClickSelecionarAula(aula);
    refForm.resetForm();
    setValoresIniciais(inicial);
  };

  const validaAntesDoSubmit = () => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      refForm.setFieldTouched(campo, true, true);
    });
    refForm.validateForm().then(() => {
      if (
        refForm.getFormikContext().isValid ||
        Object.keys(refForm.getFormikContext().errors).length === 0
      ) {
        refForm.handleSubmit(e => e);
      }
    });
  };

  const onClickCancelar = () => {
    refForm.resetForm();
    setValoresIniciais(inicial);
    onClickFecharModal();
  };

  return (
    <ModalConteudoHtml
      id="selecionar-aula"
      key="selecionar-aula"
      visivel={visivel}
      onConfirmacaoPrincipal={validaAntesDoSubmit}
      onConfirmacaoSecundaria={onClickCancelar}
      onClose={onClickCancelar}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Cancelar"
      titulo="Selecionar aula"
      closable={false}
    >
      <Formik
        enableReinitialize
        initialValues={valoresIniciais}
        onSubmit={valores => onClickConfirmar(valores)}
        validationSchema={validacoes}
        validateOnChange
        validateOnBlur
        ref={refFormik => setRefForm(refFormik)}
      >
        {form => (
          <Form className="col-md-12">
            <div className="row">
              <div className="col-md-12 mb-2">
                <p>
                  Existe mais de uma aula para esta turma nesta data, selecione
                  qual aula você deseja visualizar o diário de bordo.
                </p>
              </div>
              <div className="col-md-12">
                <RadioGroupButton
                  id="tipo-aula"
                  opcoes={opcoesTipoAula}
                  name="tipoAula"
                  form={form}
                />
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </ModalConteudoHtml>
  );
};

ModalSelecionarAulaFrequenciaPlanoAula.propTypes = {
  aulasParaSelecionar: PropTypes.oneOfType([PropTypes.object]),
  visivel: PropTypes.bool,
  onClickFecharModal: PropTypes.func,
  onClickSelecionarAula: PropTypes.func,
};

ModalSelecionarAulaFrequenciaPlanoAula.defaultProps = {
  aulasParaSelecionar: {},
  visivel: false,
  onClickFecharModal: () => {},
  onClickSelecionarAula: () => {},
};

export default ModalSelecionarAulaFrequenciaPlanoAula;
