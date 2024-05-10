import React, { useState, useCallback, memo } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Redux
import { useDispatch } from 'react-redux';

// Componentes
import { Grid, Localizador } from '~/componentes';
import DreDropDown from '~/componentes-sgp/DreDropDown';
import UeDropDown from '~/componentes-sgp/UeDropDown';
import AnoLetivoTag from '../../../componentes/AnoLetivoTag';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

// Utils
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

import { selecionarUe } from '~/redux/modulos/atribuicaoEsporadica/actions';

const Filtro = memo(({ onFiltrar }) => {
  const dispatch = useDispatch();
  const [refForm, setRefForm] = useState({});
  const [dreId, setDreId] = useState('');

  const anoAtual = window.moment().format('YYYY');

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
    if (validaSeObjetoEhNuloOuVazio(valores)) return;
    const formContext = refForm && refForm.getFormikContext();
    if (formContext.isValid && Object.keys(formContext.errors).length === 0) {
      onFiltrar(valores);
    }
  };

  const onChangeDre = useCallback(valor => {
    setDreId(valor);
  }, []);

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes()}
      onSubmit={valores => onFiltrar(valores)}
      ref={refFormik => setRefForm(refFormik)}
      validate={valores => validarFiltro(valores)}
      validateOnChange
      // validateOnBlur
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Linha className="row mb-2">
            <Grid cols={2}>
              <AnoLetivoTag form={form} />
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
          </Linha>
          <Linha className="row">
            <Localizador
              dreId={dreId}
              anoLetivo={anoAtual}
              form={form}
              onChange={valor => valor}
            />
          </Linha>
        </Form>
      )}
    </Formik>
  );
});

Filtro.propTypes = {
  onFiltrar: PropTypes.func,
};

Filtro.defaultProps = {
  onFiltrar: () => null,
};

export default Filtro;
