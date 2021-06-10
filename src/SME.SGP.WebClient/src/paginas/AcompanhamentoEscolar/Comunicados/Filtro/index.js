import React, { useState, useEffect, useMemo, useCallback } from 'react';
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
  Loader,
  SelectAutocomplete,
  momentSchema,
} from '~/componentes';

import { Linha } from '~/componentes/EstilosGlobais';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import ServicoComunicadoEvento from '~/servicos/Paginas/AcompanhamentoEscolar/ComunicadoEvento/ServicoComunicadoEvento';
import { erros, ServicoCalendarios } from '~/servicos';
import FiltroHelper from '~/paginas/AcompanhamentoEscolar/Comunicados/Helper/helper.js';

const MODALIDADE_EJA_ID = '3';
const TODAS_MODALIDADES_ID = '-99';
const TODAS_TURMAS_ID = '-99';
const TODAS_UES_ID = '-99';
const TODAS_DRE_ID = '-99';
const TIPOS_ESCOLA_BLOQUEAR = [
  10,
  11,
  12,
  13,
  14,
  15,
  18,
  19,
  22,
  23,
  25,
  26,
  27,
  29,
];

function Filtro({ onFiltrar }) {
  const todos = [{ id: TODAS_MODALIDADES_ID, nome: 'Todas' }];
  const todosTurmasModalidade = [{ id: TODAS_TURMAS_ID, nome: 'Todas' }];
  const semestresLista = [
    { id: '1', nome: '1º Semestre' },
    { id: '2', nome: '2º Semestre' },
  ];

  const [refForm, setRefForm] = useState({});
  const [gruposLista, setGruposLista] = useState([]);
  const [anosLetivos, setAnosLetivos] = useState([]);
  const [modalidades, setModalidades] = useState(todosTurmasModalidade);
  const [dres, setDres] = useState([]);
  const [ues, setUes] = useState(todos);
  const [ueSelecionada, setUeSelecionada] = useState(TODAS_UES_ID);
  const [semestres] = useState(semestresLista);
  const [turmas, setTurmas] = useState(todosTurmasModalidade);

  const [modalidadeSelecionada, setModalidadeSelecionada] = useState(
    TODAS_MODALIDADES_ID
  );
  const [anosModalidade, setAnosModalidade] = useState([]);
  const [gruposSelecionados, setGruposSelecionados] = useState([]);
  const [timeoutCampoPesquisa, setTimeoutCampoPesquisa] = useState();
  const [
    bloquearCamposCalendarioEventos,
    setBloquearCamposCalendarioEventos,
  ] = useState(false);

  const dreDesabilitada = useMemo(() => {
    return dres.length <= 1;
  }, [dres]);

  const ueDesabilitada = useMemo(() => {
    return ues.length <= 1;
  }, [ues]);

  const modalidadeDesabilitada = useMemo(() => {
    return modalidades.length <= 1 || gruposSelecionados?.length != 0;
  }, [refForm, modalidades, ues, dres, gruposSelecionados]);

  const semestreDesabilitado = useMemo(() => {
    return modalidadeSelecionada !== MODALIDADE_EJA_ID;
  }, [modalidadeSelecionada]);

  const turmasDesabilitada = useMemo(() => {
    return turmas.length <= 1;
  }, [turmas]);

  const gruposDesabilitados = useMemo(() => {
    return (
      modalidadeSelecionada &&
      modalidadeSelecionada !== '' &&
      modalidadeSelecionada !== TODAS_MODALIDADES_ID &&
      modalidadeSelecionada !== 'Todas' &&
      gruposSelecionados?.length > 0
    );
  }, [modalidadeSelecionada]);

  const [valoresIniciais] = useState({
    gruposId: '',
    dataEnvio: '',
    dataExpiracao: '',
    titulo: '',
    anoLetivo: `${moment().year()}`,
    CodigoDre: TODAS_DRE_ID,
    CodigoUe: TODAS_UES_ID,
    modalidade: TODAS_MODALIDADES_ID,
    semestre: 'Todos',
    turmas: [TODAS_TURMAS_ID],
    tipoCalendarioId: '',
    eventoId: '',
    ano: 'Todos',
  });

  const [validacoes] = useState(
    Yup.object({
      dataExpiracao: momentSchema
        .test(
          'validaDataAnoMaiorQueAnoAtual',
          'Data de expiração não pode ser maior que ano atual',
          function validar() {
            const { dataExpiracao } = this.parent;
            return !(
              moment(dataExpiracao).format('YYYY') >
              moment(new Date()).format('YYYY')
            );
          }
        )
        .test(
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

  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [listaCalendario, setListaCalendario] = useState([]);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');

  const [carregandoEventos, setCarregandoEventos] = useState(false);
  const [listaEvento, setListaEvento] = useState([]);
  const [valorEvento, setValorEvento] = useState('');
  const [eventoSelecionado, setEventoSelecionado] = useState('');
  const [pesquisaEvento, setPesquisaEvento] = useState('');

  const limparDadosEvento = form => {
    setValorEvento('');
    setEventoSelecionado();
    if (form?.setFieldValue) {
      form.setFieldValue('eventoId', '');
    }
  };

  const limparDadosTipoCalendario = form => {
    setValorEvento('');
    setEventoSelecionado();
    if (form?.setFieldValue) {
      form.setFieldValue('eventoId', '');
    }
  };

  const selecionaEvento = (nome, form) => {
    const evento = listaEvento?.find(t => {
      return t.nome === nome;
    });

    if (evento?.id) {
      setValorEvento(evento.nome);
      setEventoSelecionado(evento);
    } else {
      limparDadosEvento(form);
    }
  };

  const selecionaTipoCalendario = (descricao, form) => {
    const tipo = listaCalendario?.find(t => {
      return t.descricao === descricao;
    });

    if (tipo?.id) {
      setValorTipoCalendario(descricao);
      setTipoCalendarioSelecionado(tipo.id);
    } else {
      limparDadosTipoCalendario(form);
    }

    selecionaEvento('', form);
  };

  const anosModalidadeDesabilita = useMemo(() => {
    return (
      anosModalidade?.length <= 1 ||
      modalidadeSelecionada === TODAS_MODALIDADES_ID ||
      modalidadeSelecionada === MODALIDADE_EJA_ID
    );
  }, [anosModalidade, modalidadeSelecionada]);

  const modalidadeTurmaCalendarioRelation = {
    '1': '3',
    '3': '2',
    '5': '1',
    '6': '1',
  };

  const hasAnoLetivoClause = t =>
    refForm?.state?.values?.anoLetivo ?? false
      ? t.anoLetivo == refForm.state.values.anoLetivo
      : true;

  const hasModalidadeSelecionadaClause = t =>
    modalidadeSelecionada && modalidadeSelecionada != TODAS_MODALIDADES_ID
      ? modalidadeTurmaCalendarioRelation[modalidadeSelecionada] &&
        modalidadeTurmaCalendarioRelation[modalidadeSelecionada] == t.modalidade
      : true;

  const hasActiveSituation = t => t.situacao;

  const filterAllowedCalendarTypes = data => {
    return data
      .filter(hasAnoLetivoClause)
      .filter(hasModalidadeSelecionadaClause)
      .filter(hasActiveSituation);
  };

  const loadTiposCalendarioEffect = () => {
    let isSubscribed = true;

    (async () => {
      setCarregandoTipos(true);

      const {
        data,
      } = await ServicoCalendarios.obterTiposCalendarioAutoComplete(
        pesquisaTipoCalendario
      );

      if (isSubscribed) {
        const allowedList = filterAllowedCalendarTypes(data);
        setListaCalendario(allowedList);
        selecionaTipoCalendario(
          allowedList.length > 0 ? allowedList[0].descricao : '',
          refForm
        );
        setCarregandoTipos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  };

  const changeListaCalendarioEffect = () => {
    if (refForm && refForm.state) validarFiltro();
  };

  const changeValorEventoEffect = () => {
    if (refForm && refForm.state) validarFiltro();
  };

  const loadEventosEffect = () => {
    let isSubscribed = true;

    (async () => {
      setCarregandoEventos(true);

      const _form = refForm?.state?.values;
      const filter = {
        tipoCalendario: +(tipoCalendarioSelecionado ?? null),
        anoLetivo: +(_form?.anoLetivo ?? null),
        modalidade: +(_form?.modalidade ?? null),
        codigoDre:
          _form?.CodigoDre && _form?.CodigoDre != TODAS_DRE_ID
            ? null
            : _form?.CodigoDre,
        codigoUe:
          _form?.CodigoUe && _form?.CodigoUe != TODAS_UES_ID
            ? null
            : _form?.codigoUe,
      };

      Object.keys(filter).forEach(key => {
        if (filter[key] == null || filter[key] == TODAS_UES_ID)
          delete filter[key];
      });

      const retorno = await ServicoComunicadoEvento.listarPor(filter)
        .catch(e => erros(e))
        .finally(() => setCarregandoEventos(false));

      if (isSubscribed) {
        if (retorno?.data.length) {
          retorno.data.forEach(
            item => (item.nome = `${item.id} - ${item.nome}`)
          );
          setListaEvento(retorno.data);
        } else {
          setListaEvento([]);
        }
        setCarregandoEventos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  };

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
      validarFiltro();
    }

    setUes(dados);
  };

  async function ObterModalidades(ue) {
    const anoForm = refForm?.state?.values?.anoLetivo
      ? refForm.state.values.anoLetivo
      : moment().year();
    const dados = await FiltroHelper.obterModalidadesAnoLetivo(ue, anoForm);

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
    refForm.setFieldValue('turmas', ['Todas']);
  };

  const ObterGruposIdPorModalidade = async modalidade => {
    const dados = await FiltroHelper.ObterGruposIdPorModalidade(modalidade);
    if (!dados || dados.length === 0) return;
    refForm.setFieldValue('gruposId', dados);
  };

  const chainTodosAnos = (dados, modalidade) => {
    if (
      dados.length == 1 ||
      modalidade == TODAS_MODALIDADES_ID ||
      modalidade == MODALIDADE_EJA_ID
    ) {
      refForm.setFieldValue('ano', 'Todos');
    }
  };

  const chainLimpaAnos = (dados, modalidade) => {
    if (modalidade != TODAS_MODALIDADES_ID && modalidade != MODALIDADE_EJA_ID) {
      refForm.setFieldValue('ano', '');
    }
  };

  const ObterAnosPorModalidade = async (
    modalidade,
    codigoUe = TODAS_UES_ID
  ) => {
    if (!modalidade || modalidade === '') return;

    const dados = await FiltroHelper.obterAnosPorModalidade(
      modalidade,
      codigoUe
    );

    if (!dados) {
      setAnosModalidade([]);
      return;
    }

    setAnosModalidade(dados);
    chainTodosAnos(dados, modalidade);
    chainLimpaAnos(dados, modalidade);
  };

  const onChangeAnoLetivo = async ano => {
    refForm.setFieldValue('CodigoDre', TODAS_DRE_ID);
    refForm.setFieldValue('tipoCalendarioId', '');
    onChangeDre(TODAS_DRE_ID);

    if (ano == 0 || !ano || ano == '') {
      setDres(todos);
      return;
    }

    ObterDres();
    loadTiposCalendarioEffect();
  };

  const onChangeDre = async dre => {
    setUeSelecionada(TODAS_UES_ID);
    refForm.setFieldValue('CodigoUe', ueSelecionada);
    onChangeUe(ueSelecionada);

    if (dre == TODAS_DRE_ID) {
      setUes(todos);
      validarFiltro();
      return;
    }

    validarFiltro();
    ObterUes(dre);
    loadTiposCalendarioEffect();
  };

  const onChangeUe = async ue => {
    refForm.setFieldValue('modalidade', TODAS_MODALIDADES_ID);
    refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
    setUeSelecionada(ue);

    if (ue == TODAS_UES_ID) {
      setModalidades(todosTurmasModalidade);
      setTurmas(todosTurmasModalidade);
    }
  };

  const validarBloquearCamposCalendarioEventos = ue => {
    const ueEscolhida = ues?.find(item => item?.id?.toString() === ue);
    const ueEncontrada = TIPOS_ESCOLA_BLOQUEAR.find(
      id => id === ueEscolhida?.tipoEscola
    );

    return !!ueEncontrada;
  };

  useEffect(() => {
    if (ueSelecionada) {
      const bloquear = validarBloquearCamposCalendarioEventos(ueSelecionada);
      if (bloquear) {
        setBloquearCamposCalendarioEventos(true);
        return;
      }

      setBloquearCamposCalendarioEventos(false);
      loadTiposCalendarioEffect();
    } else {
      setBloquearCamposCalendarioEventos(false);
      loadTiposCalendarioEffect();
    }
  }, [ueSelecionada]);

  const onChangeModalidade = async modalidade => {
    refForm.setFieldValue('semestre', '');
    refForm.setFieldValue('ano', []);
    refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
    refForm.setFieldValue('gruposId', '');
    setGruposSelecionados([]);

    setTurmas(todosTurmasModalidade);

    if (
      !modalidade ||
      modalidade === '' ||
      modalidade === TODAS_MODALIDADES_ID
    ) {
      setModalidadeSelecionada(TODAS_MODALIDADES_ID);
      return;
    }

    setModalidadeSelecionada(modalidade);
    loadTiposCalendarioEffect();

    await ObterGruposIdPorModalidade(modalidade);
    await ObterAnosPorModalidade(modalidade, ueSelecionada);

    if (modalidade !== MODALIDADE_EJA_ID) {
      ObterTurmas(refForm.state.values.anoLetivo, ueSelecionada, modalidade, 0);
    }
  };

  const onSemestreChange = async semestre => {
    refForm.setFieldValue('turmas', []);
    refForm.setFieldValue('anos', []);

    if (!semestre || semestre == 0) {
      setTurmas(todosTurmasModalidade);
      refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
      return;
    }

    if (refForm.state.values.modalidade === MODALIDADE_EJA_ID)
      ObterTurmas(
        refForm.state.values.anoLetivo,
        ueSelecionada,
        refForm.state.values.modalidade,
        semestre
      );
  };

  const onAnoModalidadeChange = async ano => {
    refForm.setFieldValue('turmas', []);

    if (!ano || ano == -99) {
      setTurmas(todosTurmasModalidade);
      refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
    }
  };

  const onTurmaChange = async turmas => {
    if (turmas.length == 0) {
      refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
      validarFiltro();
      return;
    }

    const ultimoTodos = turmas[turmas.length - 1] === TODAS_TURMAS_ID;

    const turmasFiltradas = ultimoTodos
      ? turmas.filter(x => x === TODAS_TURMAS_ID)
      : turmas.filter(x => x !== TODAS_TURMAS_ID);

    refForm.setFieldValue('turmas', turmasFiltradas);

    validarFiltro();
  };

  const onGrupoChange = grupos => {
    refForm.setFieldValue('modalidade', TODAS_MODALIDADES_ID);
    refForm.setFieldValue('ano', []);
    setModalidadeSelecionada(TODAS_MODALIDADES_ID);
    setGruposSelecionados(grupos);
  };

  const onSubmitFiltro = valores => {
    if (dres?.length && ues?.length) {
      const valoresSubmit = {
        ...valores,
        // modalidade: valores.modalidade === TODAS_MODALIDADES_ID ? '' : valores.modalidade,
        modalidade: null,
        turmas: valores.turmas[0] === TODAS_TURMAS_ID ? [] : valores.turmas,
        tipoCalendarioId: tipoCalendarioSelecionado ?? null,
        eventoId: eventoSelecionado?.id ?? null,
        semestre: valores.semestre == 'Todos' ? null : valores.semestre,
        ano: valores.ano == 'Todos' ? null : valores.ano,
        CodigoUe: valores.CodigoUe == TODAS_UES_ID ? 'todas' : valores.CodigoUe,
        CodigoDre:
          valores.CodigoDre == TODAS_DRE_ID ? 'todas' : valores.CodigoDre,
        gruposId: gruposSelecionados,
        dataEnvio: valores?.dataEnvio?.set({ hour: 0, minute: 0, second: 0 }),
        dataExpiracao: valores?.dataExpiracao?.set({
          hour: 23,
          minute: 59,
          second: 59,
        }),
      };

      onFiltrar(valoresSubmit);
    }
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

  const validarFitlroDebounced = () => {
    if (timeoutCampoPesquisa) {
      clearTimeout(timeoutCampoPesquisa);
    }
    const timeout = setTimeout(() => {
      validarFiltro();
    }, 500);

    setTimeoutCampoPesquisa(timeout);
  };

  useEffect(loadTiposCalendarioEffect, [
    pesquisaTipoCalendario,
    modalidadeSelecionada,
    refForm,
  ]);

  useEffect(loadEventosEffect, [
    pesquisaEvento,
    tipoCalendarioSelecionado,
    valorTipoCalendario,
    modalidadeSelecionada,
    refForm,
  ]);

  useEffect(() => {
    if (dres?.length && ues?.length) {
      validarFitlroDebounced();
    }
  }, [dres, ues]);

  useEffect(changeListaCalendarioEffect, [listaCalendario]);
  useEffect(changeValorEventoEffect, [valorEvento]);
  useEffect(() => {
    ObterModalidades(ueSelecionada ?? TODAS_UES_ID);
  }, [anosLetivos, dres, ues, ueSelecionada]);

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
              <Label control="CodigoUe" text="Unidade Escolar (UE)" />
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
            <Grid cols={2}>
              <Label control="ano" text="Ano" />
              <SelectComponent
                form={form}
                id="ano"
                name="ano"
                placeholder="Selecione ano"
                valueOption="ano"
                valueText="ano"
                value={form.values.ano}
                lista={anosModalidade}
                allowClear
                disabled={anosModalidadeDesabilita}
                onChange={x => {
                  validarFiltro();
                  onAnoModalidadeChange(x);
                }}
              />
            </Grid>
            <Grid cols={4}>
              <Label control="turmas" text="Turma" />
              <SelectComponent
                form={form}
                id="turmas"
                name="turmas"
                placeholder="Selecione uma ou mais"
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
                onChange={grupo => {
                  validarFiltro();
                  onGrupoChange(grupo);
                }}
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
          <Linha className="row mb-2">
            <Grid cols={6}>
              <Label control="tipoCalendarioId" text="Tipo de Calendário" />
              <Loader loading={carregandoTipos} tip="">
                <SelectAutocomplete
                  hideLabel
                  showList
                  isHandleSearch
                  placeholder="Selecione um calendário"
                  className="col-md-12"
                  name="tipoCalendarioId"
                  id="select-tipo-calendario"
                  lista={listaCalendario}
                  valueField="id"
                  textField="descricao"
                  onSelect={valor => selecionaTipoCalendario(valor, form)}
                  onChange={valor => selecionaTipoCalendario(valor, form)}
                  value={valorTipoCalendario}
                  form={form}
                  allowClear={true}
                  disabled={bloquearCamposCalendarioEventos}
                />
              </Loader>
            </Grid>
            <Grid cols={6}>
              <Label control="evento" text="Evento" />
              <Loader loading={carregandoEventos} tip="">
                <SelectAutocomplete
                  hideLabel
                  showList
                  isHandleSearch
                  placeholder="Selecione um evento"
                  className="col-md-12"
                  name="eventoId"
                  id="select-evento"
                  key="select-evento-key"
                  lista={listaEvento}
                  valueField="id"
                  textField="nome"
                  onSelect={valor => selecionaEvento(valor, form)}
                  onChange={valor => selecionaEvento(valor, form)}
                  value={valorEvento}
                  form={form}
                  allowClear={false}
                  disabled={bloquearCamposCalendarioEventos}
                />
              </Loader>
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
                onChange={validarFitlroDebounced}
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
