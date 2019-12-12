import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { Grid, CampoTexto } from '~/componentes';
import MesesDropDown from '../../../componentes/MesesDropDown';

// Styles
import { Row } from './styles';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais] = useState({
    mes: '',
    titulo: '',
  });

  const validacoes = () => {
    return Yup.object({});
  };

  const validarFiltro = valores => {
    const formContext = refForm && refForm.getFormikContext();
    if (formContext.isValid && Object.keys(formContext.errors).length === 0) {
      onFiltrar(valores);
    }
  };

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes()}
      onSubmit={valores => onFiltrar(valores)}
      ref={refFormik => setRefForm(refFormik)}
      validate={valores => validarFiltro(valores)}
      validateOnChange
      validateOnBlur
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Row className="row mb-2">
            <Grid cols={2}>
              <MesesDropDown form={form} />
            </Grid>
            <Grid cols={10}>
              <CampoTexto
                name="titulo"
                id="titulo"
                placeholder="Digite o título do registro"
                form={form}
                iconeBusca
              />
            </Grid>
          </Row>
        </Form>
      )}
    </Formik>
  );
}

Filtro.propTypes = {
  onFiltrar: PropTypes.func,
};

Filtro.defaultProps = {
  onFiltrar: () => null,
};

export default Filtro;
