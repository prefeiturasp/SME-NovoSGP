import React, {
  useState,
  useEffect,
  useMemo,
  useCallback,
} from 'react';

import PropTypes from 'prop-types';

import { Form, Formik } from 'formik';

import { Linha } from '~/componentes/EstilosGlobais';

import { Grid, SelectComponent, Label } from '~/componentes';

function FiltroDashbord() {
  const onSubmitFiltro = valores => {
    console.log('valores -->', valores);
    return true;
  };

  return (
    <Formik
      enableReinitialize
      // initialValues={valoresIniciais}
      // validationSchema={validacoes}
      onSubmit={valores => onSubmitFiltro(valores)}
      ref={refFormik => setRefForm(refFormik)}
      validateOnBlur
      validateOnChange
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Linha className="row mb-2">
            <Grid cols={6}>
              <Label
                control="CodigoDre"
                text="Diretoria Regional de Educação (DRE)"
              />
              <SelectComponent
                form={form}
                id="CodigoDre"
                name="CodigoDre"
                placeholder="Selecione uma Dre"
                valueOption="id"
                // disabled={dreDesabilitada}
                valueText="nome"
                // value={form.values.CodigoDre}
                // lista={dres}
                allowClear={false}
                onChange={x => {
                  // validarFiltro();
                  // onChangeDre(x);
                }}
              />
            </Grid>
            <Grid cols={6}>
              <Label control="CodigoUe" text="Unidade Escolar (UE)" />
              <SelectComponent
                form={form}
                id="CodigoUe"
                name="CodigoUe"
                placeholder="Selecione uma Ue"
                // disabled={ueDesabilitada}
                valueOption="id"
                valueText="nome"
                // value={form.values.CodigoUe}
                // lista={ues}
                allowClear={false}
                onChange={x => {
                  // validarFiltro();
                  // onChangeUe(x);
                }}
              />
            </Grid>
          </Linha>
        </Form>
      )}
    </Formik>
  );
}

export default FiltroDashbord;
