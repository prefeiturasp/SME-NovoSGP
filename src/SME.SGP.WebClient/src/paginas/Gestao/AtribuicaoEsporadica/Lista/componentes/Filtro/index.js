import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { Grid, Localizador } from '~/componentes';

// Styles
import { CaixaAno, CaixaTextoAno, Row } from './styles';

function Filtro({ onFiltrar }) {
  const [anoLetivo, setAnoLetivo] = useState('2019');
  const [valoresIniciais, setValoresIniciais] = useState({
    anoLetivo: '2019',
    dreId: null,
    ueId: null,
    rf: null,
  });

  const validacoes = () => {
    return Yup.object({
      anoLetivo: Yup.string().required(),
    });
  };

  const onFiltrarFormulario = valores => {
    console.log(valores);
    onFiltrar(valores);
  };

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes()}
      onSubmit={valores => onFiltrarFormulario(valores)}
      validateOnChange
      validateOnBlur
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Row className="row mb-2">
            <Grid cols={2}>
              <CaixaAno>
                <CaixaTextoAno>{anoLetivo}</CaixaTextoAno>
              </CaixaAno>
            </Grid>
          </Row>
          <Row className="row">
            <Grid cols={12}>
              <div className="row">
                <Localizador onChange={valor => console.log(valor)} />
              </div>
            </Grid>
          </Row>
        </Form>
      )}
    </Formik>
  );
}

export default Filtro;
