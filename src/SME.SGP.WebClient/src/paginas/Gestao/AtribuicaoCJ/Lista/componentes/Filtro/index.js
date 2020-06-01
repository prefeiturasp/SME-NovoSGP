import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { Grid, Localizador, SelectComponent } from '~/componentes';

// Componentes SGP
import DreDropDown from '~/componentes-sgp/DreDropDown/';
import UeDropDown from '~/componentes-sgp/UeDropDown/';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

const anoAtual = window.moment().format('YYYY');

function Filtro({ onFiltrar }) {
  const anosLetivos = useSelector(redux => redux.filtro.anosLetivos);

  const [refForm, setRefForm] = useState({});
  const [anoLetivo] = useState(anoAtual);
  const [valoresIniciais] = useState({
    anoLetivo: anoAtual,
    dreId: '',
    ueId: '',
    professorRf: '',
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
          <Linha className="row mb-2">
            <Grid cols={2}>
              <SelectComponent
                name="anoLetivo"
                placeholder="Ano Letivo"
                lista={anosLetivos}
                valueText="desc"
                valueOption="valor"
                form={form}
                onChange={() => {}}
                allowClear={false}
              />
            </Grid>
            <Grid cols={5}>
              <DreDropDown
                url="v1/dres/atribuicoes"
                form={form}
                onChange={() => null}
              />
            </Grid>
            <Grid cols={5}>
              <UeDropDown
                url="v1/dres"
                dreId={form.values.dreId}
                form={form}
                onChange={() => null}
              />
            </Grid>
          </Linha>
          <Linha className="row">
            <Localizador
              dreId={form.values.dreId}
              anoLetivo={anoLetivo}
              form={form}
              onChange={() => null}
            />
          </Linha>
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
