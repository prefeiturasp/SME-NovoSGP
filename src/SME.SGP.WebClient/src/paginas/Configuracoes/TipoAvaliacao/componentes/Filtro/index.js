import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Redux
import { useDispatch } from 'react-redux';

// Componentes
import { Grid, CampoTexto, SelectComponent } from '~/componentes';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

function Filtro({ onFiltrar }) {
  const dispatch = useDispatch();
  const [refForm, setRefForm] = useState({});
  const [dreId, setDreId] = useState('');
  const [valoresIniciais] = useState({
    nome: '',
    descricao: '',
    situacao: '',
  });
  const listaSituacao = [
    {
      desc: 'Ativo',
      valor: true,
    },
    {
      desc: 'Inativo',
      valor: false,
    },
  ];

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
            <Grid cols={4}>
              <CampoTexto
                form={form}
                name="nome"
                id="nome"
                maxlength={100}
                placeholder="Digite o tipo de avaliação"
                type="input"
                // ref={campoNomeTipoEventoRef}
                // onChange={aoDigitarDescricao}
                desabilitado={false}
              />
            </Grid>
            <Grid cols={6}>
              <CampoTexto
                form={form}
                name="descricao"
                id="descricao"
                maxlength={100}
                placeholder="Digite a descrição da avaliação"
                type="input"
                // ref={campoNomeTipoEventoRef}
                // onChange={aoDigitarDescricao}
                desabilitado={false}
              />
            </Grid>
            <Grid cols={2}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="situacao"
                //  onChange={aoTrocarAnoLetivo}
                lista={listaSituacao}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                //    valueSelect={anoLetivoSelecionado && `${anoLetivoSelecionado}`}
                placeholder="Situação"
                // disabled={campoAnoLetivoDesabilitado}
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
