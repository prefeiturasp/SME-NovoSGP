import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Redux
import { useDispatch } from 'react-redux';

// Componentes
import { Grid, Localizador } from '~/componentes';
import DreDropDown from '../../../componentes/DreDropDown';
import UeDropDown from '../../../componentes/UeDropDown';
import AnoLetivoDropDown from '../../../componentes/AnoLetivoDropDown';

// Styles
import { Row } from './styles';

import {
  selecionarDre,
  selecionarUe,
  selecionarAnoLetivo,
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
  const [anoLetivo, setAnoLetivo] = useState('');

  const validacoes = () => {
    return Yup.object({
      anoLetivo: Yup.string().required(),
    });
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

  const onChangeAnoLetivo = valor => {
    setAnoLetivo(valor);
    dispatch(selecionarAnoLetivo(valor));
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
              <AnoLetivoDropDown
                form={form}
                name="anoLetivo"
                onChange={valor => onChangeAnoLetivo(valor)}
              />
            </Grid>
            <Grid cols={5}>
              <DreDropDown form={form} onChange={valor => onChangeDre(valor)} />
            </Grid>
            <Grid cols={5}>
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
