import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
// import { useSelector } from 'react-redux';
import { Grid, SelectComponent, Loader } from '~/componentes';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

// Services
import AbrangenciaServico from '~/servicos/Abrangencia';
// import ServicoFiltro from '~/servicos/Componentes/ServicoFiltro';
import FiltroHelper from '~/componentes-sgp/filtro/helper';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  // const usuario = useSelector(state => state.usuario);
  // const { turmaSelecionada } = usuario;
  // const filtro = useSelector(state => state.filtro);

  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [ciclo, setCiclo] = useState(undefined);
  const [ano, setAno] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [periodo, setPeriodo] = useState(undefined);

  const [valoresIniciais, setValoresIniciais] = useState({
    dreId,
    ueId,
    ciclo,
    ano,
    turmaId,
    periodo,
  });

  const [carregandoDres, setCarregandoDres] = useState(false);
  const [listaDres, setListaDres] = useState([]);

  const buscarListaDres = async () => {
    const dres = [];
    setCarregandoDres(true);

    AbrangenciaServico.buscarDres()
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(dre => {
            dres.push({
              desc: dre.nome,
              valor: dre.codigo,
            });
          });
        }
      })
      .finally(() => {
        setCarregandoDres(false);
      });

    dres.sort(FiltroHelper.ordenarLista('desc'));
    dres.unshift({ valor: 0, desc: 'Todas' });
    setListaDres(dres);
  };

  useEffect(() => {
    buscarListaDres();
  }, []);

  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaUes, setListaUes] = useState([]);

  const buscarListaUes = useCallback(async () => {
    const ues = [];
    setCarregandoUes(true);

    AbrangenciaServico.buscarUes(dreId)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(ue => {
            ues.push({
              desc: ue.nome,
              valor: ue.codigo,
            });
          });
        }
      })
      .finally(() => {
        setCarregandoUes(false);
      });

    setListaUes(ues.sort(FiltroHelper.ordenarLista('desc')));
  }, [dreId]);

  useEffect(() => {
    buscarListaUes();
    setValoresIniciais({ dreId });
  }, [buscarListaUes, dreId]);

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
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  className="fonte-14"
                  form={form}
                  name="dreId"
                  lista={listaDres}
                  onChange={dre => setDreId(dre)}
                  containerVinculoId="containerFiltro"
                  valueOption="valor"
                  valueText="desc"
                  placeholder="Selecione a DRE"
                />
              </Loader>
            </Grid>
            <Grid cols={6}>
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  className="fonte-14"
                  form={form}
                  name="ueId"
                  lista={listaUes}
                  onChange={ue => setUeId(ue)}
                  containerVinculoId="containerFiltro"
                  valueOption="valor"
                  valueText="desc"
                  placeholder="Selecione a UE"
                />
              </Loader>
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
