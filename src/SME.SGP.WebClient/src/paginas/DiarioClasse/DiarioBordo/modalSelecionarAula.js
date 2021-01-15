import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { ModalConteudoHtml, SelectComponent } from '~/componentes';

const ModalSelecionarAula = props => {
  const {
    visivel,
    aulasParaSelecionar,
    onClickFecharModal,
    onClickSelecionarAula,
  } = props;

  const opcoesAulas = aulasParaSelecionar.sort((a, b) => a.aulaCJ ? 1:-1)
    .map(item => {
    const lbl = (item.aulaCJ ? 'Aula CJ - ' : 'Aula normal - ') + item.criadoPor + ` (${item.professorRf})`;
    const aula = {label: lbl, value: item.aulaId};
    return aula;
  });
  const [refForm, setRefForm] = useState({});
  const inicial = {
    aula: '',
  };
  const [valoresIniciais, setValoresIniciais] = useState(inicial);

  const validacoes = Yup.object({
    aula: Yup.string().required('Campo obrigatório'),
  });

  const onClickConfirmar = valores => {
    let aula = null;
    aula = aulasParaSelecionar.find(item => String(item.aulaId) === valores.aula);
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
              <SelectComponent
                id="aula"
                lista={opcoesAulas}
                placeholder="Selecione uma aula"
                valueText="label"
                valueOption="value"
                name="aula"
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

ModalSelecionarAula.propTypes = {
  aulasParaSelecionar: PropTypes.oneOfType([PropTypes.object]),
  visivel: PropTypes.bool,
  onClickFecharModal: PropTypes.func,
  onClickSelecionarAula: PropTypes.func,
};

ModalSelecionarAula.defaultProps = {
  aulasParaSelecionar: {},
  visivel: false,
  onClickFecharModal: () => {},
  onClickSelecionarAula: () => {},
};

export default ModalSelecionarAula;
