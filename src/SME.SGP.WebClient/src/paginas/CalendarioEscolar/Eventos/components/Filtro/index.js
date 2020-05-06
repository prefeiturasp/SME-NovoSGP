/* eslint-disable react/no-this-in-sfc */
import React, { useState, useCallback } from 'react';
import PropTypes from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { CampoData, CampoTexto, momentSchema } from '~/componentes';
import DropDownTipoCalendario from './components/DropDownTipoCalendario';
import DropDownTipoEvento from './components/DropDownTipoEvento';

// Componentes SGP
import DreDropDown from '~/componentes-sgp/DreDropDown/';
import UeDropDown from '~/componentes-sgp/UeDropDown/';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function Filtro({ onFiltrar }) {
  const usuario = useSelector(x => x.usuario);
  const [refForm, setRefForm] = useState({});

  const [valoresIniciais] = useState({
    tipoCalendarioId: undefined,
    dreId: undefined,
    ueId: undefined,
    ehTodasDres: false,
    ehTodasUes: false,
    dataInicio: '',
    dataFim: '',
  });

  const [validacoes] = useState(
    Yup.object({
      dataInicio: momentSchema.test(
        'validaInicio',
        'Data obrigatória',
        function validar() {
          const { dataInicio } = this.parent;
          const { dataFim } = this.parent;
          if (!dataInicio && dataFim) {
            return false;
          }
          return true;
        }
      ),
      dataFim: momentSchema.test(
        'validaFim',
        'Data obrigatória',
        function validar() {
          const { dataInicio } = this.parent;
          const { dataFim } = this.parent;
          if (dataInicio && !dataFim) {
            return false;
          }
          return true;
        }
      ),
    })
  );

  const validarFiltro = useCallback(
    async valores => {
      const formContext = refForm && refForm.getFormikContext();
      if (formContext.isValid && Object.keys(formContext.errors).length === 0) {
        onFiltrar(valores);
      }
    },
    [onFiltrar, refForm]
  );

  const onChangeTipoCalendario = useCallback(
    valor => {
      const formContext = refForm && refForm.getFormikContext();
      if (valorNuloOuVazio(valor)) {
        formContext.setValues({
          ...valoresIniciais,
          dreId: formContext.values.dreId,
          ueId: formContext.values.ueId,
        });
      }
    },
    [refForm, valoresIniciais]
  );

  return (
    <Formik
      ref={refFormik => setRefForm(refFormik)}
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes}
      validate={valores => validarFiltro(valores)}
      onSubmit={() => true}
      validateOnChange
      validateOnBlur
      validateOnMount
    >
      {form => (
        <Form className="col-md-12 mb-4">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
              <DropDownTipoCalendario
                onChange={onChangeTipoCalendario}
                form={form}
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
              <DreDropDown
                form={form}
                opcaoTodas
                onChange={() => null}
                desabilitado={
                  usuario.possuiPerfilDre || !usuario.possuiPerfilSmeOuDre
                }
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
              <UeDropDown
                form={form}
                opcaoTodas
                onChange={() => null}
                dreId={form.values.dreId}
                desabilitado={false}
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
              <CampoTexto
                placeholder="Digite o nome do evento"
                onChange={() => null}
                value={form.values.nomeEvento}
                name="nomeEvento"
                desabilitado={false}
                form={form}
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
              <DropDownTipoEvento
                form={form}
                onChange={() => null}
                selecionouCalendario={!!form.values.tipoCalendarioId}
              />
            </div>
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2 pr-2">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataInicio"
                onChange={() => null}
                placeholder="Data início"
                form={form}
                desabilitado={false}
              />
            </div>
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2 pl-2">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataFim"
                onChange={() => null}
                placeholder="Data fim"
                form={form}
                desabilitado={false}
              />
            </div>
          </div>
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
