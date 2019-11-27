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
import DreDropDown from '~/componentes-sgp/DreDropDown/index';
import UeDropDown from '~/componentes-sgp/UeDropDown/index';

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

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes()}
      onSubmit={valores => onFiltrar(valores)}
      ref={refFormik => setRefForm(refFormik)}
      // validate={valores => validarFiltro(valores)}
      validateOnChange
      validateOnBlur
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Grid cols={2}>
            <Tag>2019</Tag>
            <Tag tipo="informativo1">2019</Tag>
            <Tag tipo="informativo2">2019</Tag>
            <Tag tipo="alerta">2019</Tag>
            <Tag tipo="erro">2019</Tag>
            <Tag tipo="atencao">2019</Tag>
            <Tag tipo="sucesso">2019</Tag>
            <Tag tipo="cancelar">2019</Tag>
          </Grid>
          <Grid cols={2}>
            <Tag inverted>2019</Tag>
            <Tag inverted tipo="informativo1">
              2019
            </Tag>
            <Tag inverted tipo="informativo2">
              2019
            </Tag>
            <Tag inverted tipo="alerta">
              2019
            </Tag>
            <Tag inverted tipo="erro">
              2019
            </Tag>
            <Tag inverted tipo="atencao">
              2019
            </Tag>
            <Tag inverted tipo="sucesso">
              2019
            </Tag>
            <Tag inverted tipo="cancelar">
              2019
            </Tag>
          </Grid>
          {/* <Row className="row mb-2">
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
          </Row> */}
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
