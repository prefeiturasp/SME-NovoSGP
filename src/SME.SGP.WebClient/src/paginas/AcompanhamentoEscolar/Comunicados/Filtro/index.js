import React, {
  useState,
  useEffect,
  useMemo,
  useLayoutEffect,
  useCallback,
} from 'react';
import PropTypes from 'prop-types';

import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import * as moment from 'moment';

import {
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
  momentSchema,
} from '~/componentes';

import AbrangenciaServico from '~/servicos/Abrangencia';

import { Linha } from '~/componentes/EstilosGlobais';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import { erros } from '~/servicos/alertas';
import FiltroHelper from '~/paginas/AcompanhamentoEscolar/Comunicados/Helper/helper.js';

function Filtro({ onFiltrar }) {
  const todos = [{ id: 'todas', nome: 'Todas' }];
  const todosTurmasModalidade = [{ id: '-99', nome: 'Todas' }];
  const semestresLista = [
    { id: '1', nome: '1º Semestre' },
    { id: '2', nome: '2º Semestre' },
  ];

  const [refForm, setRefForm] = useState({});
  const [gruposLista, setGruposLista] = useState([]);
  const [anosLetivos, setAnosLetivos] = useState([]);
  const [modalidades, setModalidades] = useState(todosTurmasModalidade);
  const [dres, setDres] = useState(todos);
  const [ues, setUes] = useState(todos);
  const [semestres] = useState(semestresLista);
  const [turmas, setTurmas] = useState(todosTurmasModalidade);

  const [modalidadeSelecionada, setModalidadeSelecionada] = useState('-99');

  const dreDesabilitada = useMemo(() => {
    return dres.length <= 1;
  }, [dres]);

  const ueDesabilitada = useMemo(() => {
    return ues.length <= 1;
  }, [ues]);

  const modalidadeDesabilitada = useMemo(() => {
    return modalidades.length <= 1;
  }, [modalidades]);

  const semestreDesabilitado = useMemo(() => {
    return refForm.state?.values?.modalidade !== '3';
  }, [refForm?.state?.values?.modalidade]);

  const turmasDesabilitada = useMemo(() => {
    return turmas.length <= 1;
  }, [turmas]);

  const gruposDesabilitados = useMemo(() => {
    return (
      modalidadeSelecionada &&
      modalidadeSelecionada !== '' &&
      modalidadeSelecionada !== '-99'
    );
  });

  const [valoresIniciais] = useState({
    gruposId: '',
    dataEnvio: '',
    dataExpiracao: '',
    titulo: '',
    anoLetivo: `${moment().year()}`,
    CodigoDre: 'todas',
    CodigoUe: 'todas',
    modalidade: '-99',
    semestre: '',
    turmas: ['-99'],
  });

  const [validacoes] = useState(
    Yup.object({
      dataExpiracao: momentSchema.test(
        'validaDataMaiorQueEnvio',
        'Data de expiração deve ser maior que a data de envio',
        function validar() {
          const { dataEnvio } = this.parent;
          const { dataExpiracao } = this.parent;

          if (
            dataEnvio &&
            dataExpiracao &&
            window.moment(dataExpiracao) < window.moment(dataEnvio)
          ) {
            return false;
          }

          return true;
        }
      ),
    })
  );

  const ObterDres = useCallback(async () => {
    const dados = await FiltroHelper.ObterDres();

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('CodigoDre', String(dados[0].id));
      ObterUes(dados[0].id);
    }

    setDres(dados);
  }, [setDres, refForm]);

  const ObterAnoLetivo = useCallback(async () => {
    const dados = await FiltroHelper.ObterAnoLetivo();

    if (!dados || dados.length === 0) return;

    setAnosLetivos(dados);
    validarFiltro();
    await ObterDres();
  }, [setAnosLetivos, ObterDres]);

  useEffect(() => {
    if (!refForm?.setFieldValue) return;

    obterListaGrupos();
    ObterAnoLetivo();
  }, [ObterAnoLetivo, refForm]);

  const ObterUes = async dre => {
    const dados = await FiltroHelper.ObterUes(dre);

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('CodigoUe', dados[0].id);
      ObterModalidades(dados[0].id);
      validarFiltro();
    }

    setUes(dados);
  };

  async function ObterModalidades(ue) {
    const dados = await FiltroHelper.ObterModalidades(ue);

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) refForm.setFieldValue('modalidade', dados[0].id);

    setModalidades(dados);
  }

  const ObterTurmas = async (anoLetivo, codigoUe, modalidade, semestre) => {
    const dados = await FiltroHelper.ObterTurmas(
      anoLetivo,
      codigoUe,
      modalidade,
      semestre
    );

    if (!dados || dados.length === 0) return;

    setTurmas(dados);
  };

  const ObterGruposIdPorModalidade = async modalidade => {
    const dados = await FiltroHelper.ObterGruposIdPorModalidade(modalidade);

    if (!dados || dados.length === 0) return;

    refForm.setFieldValue('gruposId', dados);
  };

  const onChangeAnoLetivo = async ano => {
    refForm.setFieldValue('CodigoDre', 'todas');
    onChangeDre('todas');

    if (ano == 0 || !ano || ano == '') {
      setDres(todos);
      return;
    }

    ObterDres();
  };

  const onChangeDre = async dre => {
    refForm.setFieldValue('CodigoUe', 'todas');
    onChangeUe('todas');

    if (dre == 'todas') {
      setUes(todos);
      validarFiltro();
      return;
    }

    validarFiltro();
    ObterUes(dre);
  };

  const onChangeUe = async ue => {
    refForm.setFieldValue('modalidade', '-99');
    refForm.setFieldValue('turmas', ['-99']);

    if (ue == 'todas') {
      setModalidades(todosTurmasModalidade);
      setTurmas(todosTurmasModalidade);
      return;
    }

    onChangeModalidade('-99');
    ObterModalidades(ue);
  };

  const onChangeModalidade = async modalidade => {
    refForm.setFieldValue('semestre', '');
    refForm.setFieldValue('turmas', ['-99']);
    refForm.setFieldValue('gruposId', '');

    setTurmas(todosTurmasModalidade);

    if (
      !refForm.state.values.CodigoUe ||
      refForm.state.values.CodigoUe === 'todas'
    ) {
      setModalidadeSelecionada('');
      return;
    }

    if (!modalidade || modalidade === '' || modalidade === '-99') {
      setModalidadeSelecionada('');
      return;
    }

    ObterGruposIdPorModalidade(modalidade);
    setModalidadeSelecionada(modalidade);

    if (modalidade !== '3')
      ObterTurmas(
        refForm.state.values.anoLetivo,
        refForm.state.values.CodigoUe,
        modalidade,
        0
      );
  };

  const onSemestreChange = async semestre => {
    refForm.setFieldValue('turmas', []);

    if (!semestre || semestre == 0) {
      setTurmas(todosTurmasModalidade);
      refForm.setFieldValue('turmas', ['-99']);
      return;
    }

    if (refForm.state.values.modalidade === '3')
      ObterTurmas(
        refForm.state.values.anoLetivo,
        refForm.state.values.CodigoUe,
        refForm.state.values.modalidade,
        semestre
      );
  };

  const onTurmaChange = async turmas => {
    if (turmas.length <= 0) {
      refForm.setFieldValue('turmas', ['-99']);
      validarFiltro();
      return;
    }

    var ultimoTodos = turmas[turmas.length - 1] === '-99';

    var turmasFiltradas = ultimoTodos
      ? turmas.filter(x => x === '-99')
      : turmas.filter(x => x !== '-99');

    refForm.setFieldValue('turmas', turmasFiltradas);

    validarFiltro();
  };

  const onSubmitFiltro = valores => {
    const valoresSubmit = {
      ...valores,
      modalidade: valores.modalidade === '-99' ? '' : valores.modalidade,
      turmas: valores.turmas[0] === '-99' ? [] : valores.turmas,
    };

    onFiltrar(valoresSubmit);
  };

  async function obterListaGrupos() {
    const lista = await ServicoComunicados.listarGrupos();
    setGruposLista(lista);
  }

  const validarFiltro = () => {
    const arrayCampos = Object.keys(valoresIniciais);

    arrayCampos.forEach(campo => {
      if (!valoresIniciais[campo].length) delete refForm.state.values[campo];
      else refForm.setFieldTouched(campo, true, true);
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
            <Grid cols={2}>
              <Label form={form} text="Ano Letivo" control="anoLetivo" />
              <SelectComponent
                form={form}
                id="anoLetivo"
                name="anoLetivo"
                placeholder="Selecione um ano letivo"
                valueOption="id"
                valueText="nome"
                value={form.values.anoLetivo}
                lista={anosLetivos}
                allowClear={false}
                onChange={x => {
                  validarFiltro();
                  onChangeAnoLetivo(x);
                }}
              />
            </Grid>
            <Grid cols={5}>
              <Label control="CodigoDre" text="Dre" />
              <SelectComponent
                form={form}
                id="CodigoDre"
                name="CodigoDre"
                placeholder="Selecione uma Dre"
                valueOption="id"
                disabled={dreDesabilitada}
                valueText="nome"
                value={form.values.CodigoDre}
                lista={dres}
                allowClear={false}
                onChange={x => {
                  validarFiltro();
                  onChangeDre(x);
                }}
              />
            </Grid>
            <Grid cols={5}>
              <Label control="CodigoUe" text="Unidade Escolar" />
              <SelectComponent
                form={form}
                id="CodigoUe"
                name="CodigoUe"
                placeholder="Selecione uma Ue"
                disabled={ueDesabilitada}
                valueOption="id"
                valueText="nome"
                value={form.values.CodigoUe}
                lista={ues}
                allowClear={false}
                onChange={x => {
                  validarFiltro();
                  onChangeUe(x);
                }}
              />
            </Grid>
          </Linha>
          <Linha className="row mb-2">
            <Grid cols={4}>
              <Label control="modalidade" text="Modalidade" />
              <SelectComponent
                form={form}
                id="modalidade"
                name="modalidade"
                placeholder="Selecione uma modalidade"
                valueOption="id"
                valueText="nome"
                value={form.values.modalidade}
                lista={modalidades}
                allowClear
                disabled={modalidadeDesabilitada}
                onChange={x => {
                  validarFiltro();
                  onChangeModalidade(x);
                }}
              />
            </Grid>
            <Grid cols={2}>
              <Label control="semestre" text="Semestre" />
              <SelectComponent
                form={form}
                id="semestre"
                name="semestre"
                placeholder="Selecione um semestre"
                valueOption="id"
                valueText="nome"
                value={form.values.semestre}
                lista={semestres}
                allowClear
                disabled={semestreDesabilitado}
                onChange={x => {
                  validarFiltro();
                  onSemestreChange(x);
                }}
              />
            </Grid>
            <Grid cols={6}>
              <Label control="turmas" text="Turmas" />
              <SelectComponent
                form={form}
                id="turmas"
                name="turmas"
                placeholder="Selecione uma ou mais turmas"
                valueOption="id"
                valueText="nome"
                value={form.values.turmas}
                disabled={turmasDesabilitada}
                lista={turmas}
                multiple
                onChange={x => {
                  validarFiltro();
                  onTurmaChange(x);
                }}
              />
            </Grid>
          </Linha>
          <Linha className="row mb-2">
            <Grid cols={4}>
              <Label control="gruposId" text="Grupo" />
              <SelectComponent
                form={form}
                id="gruposId"
                name="gruposId"
                placeholder="Selecione um grupo"
                value={form.values.gruposId}
                multiple
                disabled={gruposDesabilitados}
                lista={gruposLista}
                valueOption="id"
                valueText="nome"
                onChange={() => validarFiltro()}
              />
            </Grid>
            <Grid cols={4}>
              <Label control="dataEnvio" text="Data de envio" />
              <CampoData
                form={form}
                name="dataEnvio"
                placeholder="Selecione a data de envio"
                formatoData="DD/MM/YYYY"
                onChange={() => validarFiltro()}
              />
            </Grid>
            <Grid cols={4}>
              <Label control="dataExpiracao" text="Data de expiração" />
              <CampoData
                form={form}
                name="dataExpiracao"
                placeholder="Selecione a data de expiração"
                formatoData="DD/MM/YYYY"
                onChange={() => validarFiltro()}
              />
            </Grid>
          </Linha>
          <Linha className="row">
            <Grid cols={12}>
              <Label control="titulo" text="Título" />
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
