import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { useSelector } from 'react-redux';
import { Grid, SelectComponent } from '~/componentes';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

// Services
import ServicoFiltro from '~/servicos/Componentes/ServicoFiltro';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  const usuario = useSelector(state => state.usuario);
  const filtro = useSelector(state => state.filtro);

  const [listaDres, setListaDres] = useState([]);

  // const buscarListaDres = async () => {
  //   setListaDres(
  //     await ServicoFiltro.listarDres(
  //       consideraHistorico,
  //       modalidadeSelecionada,
  //       periodoSelecionado,
  //       anoLetivoSelecionado
  //     )
  //   );
  // };

  // const []

  useEffect(() => {
    console.log(usuario);
    console.log(filtro);
    // buscarListaDres();
  }, []);

  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [ciclo, setCiclo] = useState(undefined);
  const [ano, setAno] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [periodo, setPeriodo] = useState(undefined);

  const [valoresIniciais] = useState({
    dreId,
    ueId,
    ciclo,
    ano,
    turmaId,
    periodo,
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
            <Grid cols={6}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="dreId"
                lista={listaDres}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selecione a DRE"
              />
            </Grid>
            <Grid cols={6}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="dreId"
                lista={[]}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selecione a UE"
              />
            </Grid>
          </Linha>
          <Linha className="row mb-2">
            <Grid cols={3}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="ciclo"
                lista={[]}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selec. o Ciclo"
              />
            </Grid>
            <Grid cols={2}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="ano"
                lista={[]}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selec. o ano"
              />
            </Grid>
            <Grid cols={2}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="turmaId"
                lista={[]}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selec. a turma"
              />
            </Grid>
            <Grid cols={5}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="periodo"
                lista={[]}
                containerVinculoId="containerFiltro"
                valueOption="valor"
                valueText="desc"
                placeholder="Selecione o período"
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
