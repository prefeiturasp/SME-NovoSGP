import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';

// Formulario
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes
import { Grid, SelectComponent, Loader } from '~/componentes';
import PeriodosDropDown from './componentes/PeriodosDropDown';
import { DreDropDown, UeDropDown } from '~/componentes-sgp';

// Styles
import { Linha } from '~/componentes/EstilosGlobais';

// Services
import api from '~/servicos/api';
import AtribuicaoCJServico from '~/servicos/Paginas/AtribuicaoCJ';

// Funções
import FiltroHelper from '~/componentes-sgp/filtro/helper';

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [cicloId, setCicloId] = useState(undefined);
  const [ano, setAno] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [periodo, setPeriodo] = useState(undefined);

  const [valoresIniciais] = useState({
    dreId,
    ueId,
    cicloId,
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

  const aoTrocarDreId = id => {
    if (!id) refForm.setFieldValue('ueId', undefined);
    setDreId(id);
  };

  const [listaCiclos, setListaCiclos] = useState([]);
  const [carregandoCiclos, setCarregandoCiclos] = useState(false);

  const buscarListaCiclos = async () => {
    setCarregandoCiclos(true);

    const params = {
      anoSelecionado: window.moment().format('YYYY'),
      modalidade: 5,
      anos: ['3', '4', '5', '6', '7', '8', '9'],
    };

    const ciclos = await api.post('v1/ciclos/filtro', params).finally(() => {
      setCarregandoCiclos(false);
    });

    const { data } = ciclos;

    if (data) {
      data.sort(FiltroHelper.ordenarLista('descricao'));
      data.unshift({ descricao: 'Todos', id: '0' });
      setListaCiclos(data);
    }
  };

  useEffect(() => {
    buscarListaCiclos();
  }, []);

  const [listaAnos, setListaAnos] = useState([]);
  const [carregandoAnos, setCarregandoAnos] = useState(false);

  const aoTrocarCiclo = id => {
    setCarregandoAnos(true);

    if (id) {
      switch (id) {
        case '1':
          // Alfabetização
          setListaAnos([{ desc: '3', valor: '3' }]);
          break;
        case '2':
          // Interdisciplinar
          setListaAnos([
            { desc: '4', valor: '4' },
            { desc: '5', valor: '5' },
            { desc: '6', valor: '6' },
          ]);
          break;
        case '3':
          // Autoral
          setListaAnos([
            { desc: '7', valor: '7' },
            { desc: '8', valor: '8' },
            { desc: '9', valor: '9' },
          ]);
          break;
        case '0':
          setListaAnos([{ desc: 'Todos', valor: '0' }]);
          break;
        default:
          setListaAnos([]);
      }
    } else {
      setListaAnos([]);
    }

    setCicloId(id);
    setCarregandoAnos(false);
  };

  const [listaTodasTurmas, setListaTodasTurmas] = useState([]);

  const buscarTurmas = useCallback(async () => {
    const turmas = await AtribuicaoCJServico.buscarTurmas(ueId, 5);
    const { data } = turmas;

    if (data) setListaTodasTurmas(data);
  }, [ueId]);

  const [listaTurmas, setListaTurmas] = useState([]);

  useEffect(() => {
    if (ueId) {
      if (ueId === '0') {
        setListaTurmas([{ codigo: '0', nome: 'Todas' }]);
      } else {
        buscarTurmas();
      }
    }
  }, [buscarTurmas, ueId]);

  useEffect(() => {
    if (ano) {
      const turmas = [];
      listaTodasTurmas.forEach(turma => {
        const valor = turma.nome.split('');
        if (
          typeof parseInt(valor[0], 10) === 'number' &&
          valor[0] === ano &&
          turmas.indexOf(turma) === -1
        ) {
          turmas.push(turma);
        }
      });
      setListaTurmas(turmas.sort(FiltroHelper.ordenarLista('nome')));
    } else {
      setListaTurmas([]);
    }
  }, [ano, listaTodasTurmas]);

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
              <DreDropDown
                form={form}
                onChange={dre => aoTrocarDreId(dre)}
                opcaoTodas
              />
            </Grid>
            <Grid cols={6}>
              <UeDropDown
                form={form}
                dreId={form.values.dreId}
                onChange={ue => setUeId(ue)}
                opcaoTodas
              />
            </Grid>
          </Linha>
          <Linha className="row mb-2">
            <Grid cols={3}>
              <Loader loading={carregandoCiclos} tip="">
                <SelectComponent
                  className="fonte-14"
                  form={form}
                  name="cicloId"
                  lista={listaCiclos}
                  onChange={ciclo => aoTrocarCiclo(ciclo)}
                  containerVinculoId="containerFiltro"
                  valueOption="id"
                  valueText="descricao"
                  placeholder="Selec. o ciclo"
                  disabled={!listaCiclos.length}
                />
              </Loader>
            </Grid>
            <Grid cols={2}>
              <Loader loading={carregandoAnos} tip="">
                <SelectComponent
                  className="fonte-14"
                  form={form}
                  name="ano"
                  lista={listaAnos}
                  onChange={valor => setAno(valor)}
                  containerVinculoId="containerFiltro"
                  valueOption="valor"
                  valueText="desc"
                  placeholder="Selec. o ano"
                  disabled={!listaAnos.length}
                />
              </Loader>
            </Grid>
            <Grid cols={2}>
              <SelectComponent
                className="fonte-14"
                form={form}
                name="turmaId"
                lista={listaTurmas}
                onChange={valor => setTurmaId(valor)}
                containerVinculoId="containerFiltro"
                valueOption="codigo"
                valueText="nome"
                placeholder="Selec. a turma"
                disabled={!listaTurmas.length}
              />
            </Grid>
            <Grid cols={5}>
              <PeriodosDropDown
                valor={periodo}
                form={form}
                onChangePeriodo={valor => setPeriodo(valor)}
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
