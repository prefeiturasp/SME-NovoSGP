import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Grid, CampoTexto, Localizador } from '~/componentes';
import MesesDropDown from '../../../componentes/MesesDropDown';

// Componentes SGP
import { DreDropDown, UeDropDown } from '~/componentes-sgp';

// Styles
import { Row } from './styles';

function Filtro({ onFiltrar }) {
  const { anoLetivo } = useSelector(store => store.usuario.turmaSelecionada);
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais] = useState({
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
            <Grid cols={6}>
              <DreDropDown
                url="v1/dres/atribuicoes"
                form={form}
                onChange={() => null}
              />
            </Grid>
            <Grid cols={6}>
              <UeDropDown
                dreId={form.values.dreId}
                form={form}
                url="v1/dres"
                onChange={() => null}
              />
            </Grid>
          </Row>
          <Row className="row mb-2">
            <Localizador
              dreId={form.values.dreId}
              anoLetivo={anoLetivo}
              form={form}
              onChange={() => null}
            />
          </Row>
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
                allowClear
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
