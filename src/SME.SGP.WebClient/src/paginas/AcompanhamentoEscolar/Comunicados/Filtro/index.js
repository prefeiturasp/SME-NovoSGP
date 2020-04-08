import React, { useState } from 'react';
import PropTypes from 'prop-types';

import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import {
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
} from '~/componentes';

import { Linha } from '~/componentes/EstilosGlobais';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  const gruposLista = [
    { Id: 1, Nome: 'EJA' },
    { Id: 2, Nome: 'Médio' },
    { Id: 3, Nome: 'Fundamental' },
    { Id: 4, Nome: 'EMEBS' },
    { Id: 5, Nome: 'CEI' },
    { Id: 6, Nome: 'EMEI' },
  ];

  const [valoresIniciais] = useState({
    grupoId: [],
    dataEnvioInicio: '',
    dataEnvioFim: '',
    dataExpiracaoInicio: '',
    dataExpiracaoFim: '',
    titulo: '',
  });

  const validacoes = () => {
    return Yup.object({});
  };

  const validarFiltro = valores => {
    const formContext = refForm && refForm.getFormikContext();
    if (formContext.isValid) {
      onFiltrar(valores);
    }
  };

  const [filtro, setFiltro] = useState({});

  const validaDataInicio = dataExpiracaoInicio => {
    const filtroAtual = filtro;

    filtroAtual.dataExpiracaoInicio =
      dataExpiracaoInicio && dataExpiracaoInicio.toDate();
    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataExpiracaoInicio && filtroAtual.dataExpiracaoFim)
      validarFiltro();
  };

  const validaDataFim = dataExpiracaoFim => {
    const filtroAtual = filtro;
    filtroAtual.dataExpiracaoFim =
      dataExpiracaoFim && dataExpiracaoFim.toDate();

    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataExpiracaoInicio && filtroAtual.dataExpiracaoFim)
      validarFiltro();
  };

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes}
      onSubmit={valores => onFiltrar(valores)}
      ref={refFormik => setRefForm(refFormik)}
      validateOnChange
      validateOnBlur
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <Linha className="row mb-2">
            <Grid cols={4}>
              <Label control="grupoId" text="Grupo" />
              <SelectComponent
                form={form}
                name="grupoId"
                placeholder="Selecione um grupo"
                value={form.values.gruposId}
                multiple
                lista={gruposLista}
                valueOption="Id"
                valueText="Nome"
              />
            </Grid>
            <Grid cols={2}>
              <Label control="dataEnvioInicio" text="Data de envio" />
              <CampoData
                form={form}
                name="dataEnvioInicio"
                placeholder="Data início"
                formatoData="DD/MM/YYYY"
                onChange={data => validaDataInicio(data)}
              />
            </Grid>
            <Grid cols={2}>
              <Label
                control="dataEnvioFim"
                text="Data de envio"
                className="text-white"
              />
              <CampoData
                form={form}
                name="dataEnvioFim"
                placeholder="Data fim"
                formatoData="DD/MM/YYYY"
                onChange={data => validaDataFim(data)}
              />
            </Grid>
            <Grid cols={2}>
              <Label control="dataExpiracaoInicio" text="Data de expiração" />
              <CampoData
                form={form}
                name="dataExpiracaoInicio"
                placeholder="Data início"
                formatoData="DD/MM/YYYY"
                onChange={data => validaDataInicio(data)}
              />
            </Grid>
            <Grid cols={2}>
              <Label
                control="dataExpiracaoFim"
                text="Data de expiração"
                className="text-white"
              />
              <CampoData
                form={form}
                name="dataExpiracaoFim"
                placeholder="Data fim"
                formatoData="DD/MM/YYYY"
                onChange={data => validaDataFim(data)}
              />
            </Grid>
          </Linha>
          <Linha className="row">
            <Grid cols={12}>
              <Label control="titulo" text="Tíutulo" />
              <CampoTexto
                form={form}
                name="titulo"
                placeholder="Procure pelo título do comunicado"
                value={form.values.titulo}
              />
            </Grid>
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
