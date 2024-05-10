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
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';
import { erros } from '~/servicos/alertas';

// Funções
import FiltroHelper from '~/componentes-sgp/filtro/helper';

function objetoExistaNaLista(objeto, lista) {
  return lista.some(
    elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
  );
}

function Filtro({ onFiltrar }) {
  const [refForm, setRefForm] = useState({});

  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [cicloId, setCicloId] = useState(undefined);
  const [ano, setAno] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [periodo, setPeriodo] = useState(undefined);
  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);

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
    if (!id) {
      setUeId(undefined);
      refForm.setFieldValue('ueId', undefined);
    }
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

  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaTodasTurmas, setListaTodasTurmas] = useState([]);

  const [anoDesabilitado, setAnoDesabilitado] = useState(false);

  const aoTrocarAno = useCallback(
    valorAno => {
      if (valorAno) {
        const turmas = [];
        listaTodasTurmas.forEach(turma => {
          const valor = turma.nome.split('');
          if (
            typeof parseInt(valor[0], 10) === 'number' &&
            valor[0].toString() === valorAno.toString() &&
            !objetoExistaNaLista(turma, turmas)
          ) {
            turmas.push(turma);
          }
        });

        turmas.sort(FiltroHelper.ordenarLista('nome'));
        if (!turmas.length || turmas.length > 1)
          turmas.unshift({ nome: 'Todas', codigo: '0' });

        setListaTurmas(turmas);
      } else {
        setListaTurmas([]);
      }
      setTurmaId(undefined);
      refForm.setFieldValue('turmaId', undefined);

      setAno(valorAno);
    },
    [listaTodasTurmas, refForm]
  );

  useEffect(() => {
    if (listaTurmas) {
      if (listaTurmas.length === 1) {
        setTurmaId(listaTurmas[0].codigo);
        refForm.setFieldValue('turmaId', listaTurmas[0].codigo);
        setAnoDesabilitado(true);
      } else {
        setAnoDesabilitado(false);
      }
    }
  }, [listaTurmas, refForm]);

  const aoTrocarCiclo = id => {
    setCarregandoAnos(true);

    if (id) {
      switch (id) {
        case '1':
          // Alfabetização
          setListaAnos([
            { desc: 'Todos', valor: '0' },
            { desc: '3', valor: '3' },
          ]);
          break;
        case '2':
          // Interdisciplinar
          setListaAnos([
            { desc: 'Todos', valor: '0' },
            { desc: '4', valor: '4' },
            { desc: '5', valor: '5' },
            { desc: '6', valor: '6' },
          ]);
          break;
        case '3':
          // Autoral
          setListaAnos([
            { desc: 'Todos', valor: '0' },
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
      aoTrocarAno(undefined);
    }

    setAno(undefined);
    refForm.setFieldValue('ano', undefined);

    setTurmaId(undefined);
    refForm.setFieldValue('turmaId', undefined);

    setCicloId(id);
    setCarregandoAnos(false);
  };

  const buscarTurmas = useCallback(async () => {
    const turmas = await AtribuicaoCJServico.buscarTurmas(ueId, 5, anoLetivo);
    const { data } = turmas;

    if (data) setListaTodasTurmas(data);
  }, [ueId, anoLetivo]);

  useEffect(() => {
    if (!dreId && refForm && Object.entries(refForm).length) {
      refForm.setFieldValue('cicloId', undefined);
      setCicloId(undefined);
      refForm.setFieldValue('ano', undefined);
      setAno(undefined);
      setListaTurmas([]);
      refForm.setFieldValue('turmaId', undefined);
      setTurmaId(undefined);
      refForm.setFieldValue('periodo', undefined);
      setPeriodo(undefined);
    }
  }, [dreId, refForm]);

  useEffect(() => {
    if (ueId) {
      if (cicloId && ano && ueId === '0') {
        setListaTurmas([{ codigo: '0', nome: 'Todas' }]);
      } else {
        buscarTurmas();
      }
    } else if (refForm && Object.entries(refForm).length) {
      refForm.setFieldValue('cicloId', undefined);
      setCicloId(undefined);
      refForm.setFieldValue('ano', undefined);
      setAno(undefined);
      setListaTurmas([]);
      refForm.setFieldValue('turmaId', undefined);
      setTurmaId(undefined);
      refForm.setFieldValue('periodo', undefined);
      setPeriodo(undefined);
    }
  }, [ano, buscarTurmas, cicloId, refForm, ueId]);

  const aoTrocarUeId = valorUe => {
    setCicloId(undefined);
    refForm.setFieldValue('cicloId', undefined);
    aoTrocarCiclo(undefined);
    setUeId(valorUe);
  };

  const onChangeAnoLetivo = async valor => {
    setAnoLetivo(valor);
  };

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnosLetivos(true);
    const anosLetivo = await ResumosGraficosPAPServico.ListarAnosLetivos().catch(
      e => {
        erros(e);
        setCarregandoAnosLetivos(false);
      }
    );

    const valorAnos = anosLetivo?.data.map(item => ({
      desc: item.ano,
      valor: item.ano,
    }));

    const anosFiltrado = anosLetivo?.data.filter(
      item => item?.ehSugestao === true
    );
    const anoSugestao = anosFiltrado ? anosFiltrado[0]?.ano : '';

    setAnoLetivo(anoSugestao);
    setListaAnosLetivo(valorAnos);
    setCarregandoAnos(false);
    setCarregandoAnosLetivos(false);
  }, []);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  useEffect(() => {
    if (anoLetivo) {
      refForm.setFieldValue('anoLetivo', anoLetivo);
    }
  }, [refForm, anoLetivo]);

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
              <Loader loading={carregandoAnosLetivos} tip="">
                <SelectComponent
                  form={form}
                  name="anoLetivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano Letivo"
                />
              </Loader>
            </Grid>
            <Grid cols={5}>
              <DreDropDown
                form={form}
                onChange={dre => aoTrocarDreId(dre)}
                opcaoTodas
              />
            </Grid>
            <Grid cols={5}>
              <UeDropDown
                form={form}
                dreId={form.values.dreId}
                onChange={ue => aoTrocarUeId(ue)}
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
                  disabled={!ueId || !listaCiclos.length}
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
                  onChange={valor => aoTrocarAno(valor)}
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
                disabled={!ano || anoDesabilitado}
              />
            </Grid>
            <Grid cols={5}>
              <PeriodosDropDown
                valor={periodo}
                form={form}
                onChangePeriodo={valor => setPeriodo(valor)}
                desabilitado={!dreId || !ueId || !cicloId}
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
