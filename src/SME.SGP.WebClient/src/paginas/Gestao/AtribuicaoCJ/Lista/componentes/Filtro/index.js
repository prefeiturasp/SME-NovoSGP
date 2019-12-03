import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Redux
import { useDispatch } from 'react-redux';

// Componentes
import { Grid, Localizador, Tag } from '~/componentes';

// Componentes SGP
import DreDropDown from '~/componentes-sgp/DreDropDown/';
import UeDropDown from '~/componentes-sgp/UeDropDown/';

// Styles
import { Row } from './styles';

import {
  selecionarDre,
  selecionarUe,
} from '~/redux/modulos/atribuicaoEsporadica/actions';

function Filtro({ onFiltrar }) {
  const dispatch = useDispatch();
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais] = useState({
    anoLetivo: '',
    dreId: '',
    ueId: '',
    professorRf: '',
  });
  const [dreId, setDreId] = useState('');
  const [anoLetivo] = useState('2019');

  const validacoes = () => {
    return Yup.object({});
  };

  const validarFiltro = valores => {
    const formContext = refForm && refForm.getFormikContext();
    if (formContext.isValid && Object.keys(formContext.errors).length === 0) {
      onFiltrar(valores);
    }
  };

  const onChangeDre = valor => {
    setDreId(valor);
    dispatch(selecionarDre(valor));
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
            <Grid cols={6}>
              <DreDropDown form={form} onChange={valor => onChangeDre(valor)} />
            </Grid>
            <Grid cols={6}>
              <UeDropDown
                dreId={dreId}
                form={form}
                onChange={valor => dispatch(selecionarUe(valor))}
              />
            </Grid>
          </Row>
          <Row className="row">
            <Localizador
              dreId={dreId}
              anoLetivo={anoLetivo}
              form={form}
              onChange={valor => valor}
            />
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
