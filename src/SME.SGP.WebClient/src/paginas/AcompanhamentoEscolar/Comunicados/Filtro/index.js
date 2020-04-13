import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

import { Form, Formik } from 'formik';
import * as Yup from 'yup';

import {
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
  momentSchema,
} from '~/componentes';

import { Linha } from '~/componentes/EstilosGlobais';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  const [gruposLista, setGruposLista] = useState([]);

  useEffect(() => {
    async function obterListaGrupos() {
      const lista = await ServicoComunicados.listarGrupos();
      setGruposLista(lista);
    }
    obterListaGrupos();
  }, []);

  const [valoresIniciais] = useState({
    grupoId: [],
    dataEnvioInicio: '',
    dataEnvioFim: '',
    dataExpiracaoInicio: '',
    dataExpiracaoFim: '',
    titulo: '',
  });

  const [validacoes] = useState(
    Yup.object({
      dataEnvioInicio: momentSchema.test(
        'validaDataEnvio',
        'Data início não deve ser maior que a data fim',
        function validar() {
          const { dataEnvioInicio } = this.parent;
          const { dataEnvioFim } = this.parent;

          if (
            dataEnvioInicio &&
            dataEnvioFim &&
            window.moment(dataEnvioInicio) > window.moment(dataEnvioFim)
          ) {
            return false;
          }

          return true;
        }
      ),
      dataExpiracaoInicio: momentSchema
        .test(
          'validaDataMaiorQueEnvio',
          'Data de expiração deve ser maior que a data de envio',
          function validar() {
            const { dataEnvioFim } = this.parent;
            const { dataExpiracaoInicio } = this.parent;

            if (
              dataEnvioFim &&
              dataExpiracaoInicio &&
              window.moment(dataExpiracaoInicio) < window.moment(dataEnvioFim)
            ) {
              return false;
            }

            return true;
          }
        )
        .test(
          'validaDataExpiracao',
          'Data início não deve ser maior que a data fim',
          function validar() {
            const { dataExpiracaoInicio } = this.parent;
            const { dataExpiracaoFim } = this.parent;

            if (
              dataExpiracaoInicio &&
              dataExpiracaoFim &&
              window.moment(dataExpiracaoInicio) >
                window.moment(dataExpiracaoFim)
            ) {
              return false;
            }

            return true;
          }
        ),
    })
  );

  const onSubmitFiltro = valores => {
    onFiltrar(valores);
  };

  const validarFiltro = () => {
    const arrayCampos = Object.keys(valoresIniciais);

    arrayCampos.forEach(campo => {
      refForm.setFieldTouched(campo, true, true);
    });

    refForm.validateForm().then(() => {
      if (
        refForm &&
        refForm.state &&
        refForm.state.errors &&
        Object.entries(refForm.state.errors).length === 0
      ) {
        onSubmitFiltro(refForm.state.values);
      }
    });
  };

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes}
      onSubmit={valores => onSubmitFiltro(valores)}
      ref={refFormik => setRefForm(refFormik)}
      validateOnBlur
      validateOnChange
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
                valueOption="id"
                valueText="nome"
                onChange={() => validarFiltro()}
              />
            </Grid>
            <Grid cols={2}>
              <Label control="dataEnvioInicio" text="Data de envio" />
              <CampoData
                form={form}
                name="dataEnvioInicio"
                placeholder="Data início"
                formatoData="DD/MM/YYYY"
                onChange={() => validarFiltro()}
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
                onChange={() => validarFiltro()}
              />
            </Grid>
            <Grid cols={2}>
              <Label control="dataExpiracaoInicio" text="Data de expiração" />
              <CampoData
                form={form}
                name="dataExpiracaoInicio"
                placeholder="Data início"
                formatoData="DD/MM/YYYY"
                onChange={() => validarFiltro()}
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
                onChange={() => validarFiltro()}
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
                onChange={() => validarFiltro()}
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
