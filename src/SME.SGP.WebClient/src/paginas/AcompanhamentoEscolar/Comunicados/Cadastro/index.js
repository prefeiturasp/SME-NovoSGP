import React, { useState, useEffect, useMemo } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';

import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import * as moment from 'moment';

import { Cabecalho } from '~/componentes-sgp';
import {
  Loader,
  Card,
  ButtonGroup,
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
  momentSchema,
  Base,
  SelectAutocomplete,
} from '~/componentes';
import ListaAlunos from '~/paginas/AcompanhamentoEscolar/Comunicados/Cadastro/Lista/ListaAlunos';

import { Linha } from '~/componentes/EstilosGlobais';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import ServicoComunicadoEvento from '~/servicos/Paginas/AcompanhamentoEscolar/ComunicadoEvento/ServicoComunicadoEvento';
import { confirmar, erro, erros, sucesso } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';

import FiltroHelper from '~/paginas/AcompanhamentoEscolar/Comunicados/Helper/helper.js';
import ListaAlunosSelecionados from './Lista/ListaAlunosSelecionados';
import { ServicoCalendarios } from '~/servicos';
import modalidade from '~/dtos/modalidade';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

const TODAS_UE_ID = '-99';
const TODAS_DRE_ID = '-99';
const TODAS_MODALIDADES_ID = '-99';
const TODAS_TURMAS_ID = '-99';
const MODALIDADE_EJA_ID = '3';
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

const ComunicadosCadastro = ({ match }) => {
  const ErroValidacao = styled.span`
    color: ${Base.Vermelho};
  `;

  const InseridoAlterado = styled.div`
    color: ${Base.CinzaMako};
    font-weight: bold;
    font-style: normal;
    font-size: 10px;
    object-fit: contain;
    p {
      margin: 0;
    }
  `;

  const todos = [{ id: TODAS_UE_ID, nome: 'Todas' }];
  const todosTurmasModalidade = [{ id: TODAS_TURMAS_ID, nome: 'Todas' }];
  const semestresLista = [
    { id: '1', nome: '1º Semestre' },
    { id: '2', nome: '2º Semestre' },
  ];
  const alunosSelecionadoslist = [
    { id: '1', nome: 'Todos' },
    { id: '2', nome: 'Alunos Especificados' },
  ];
  const [loaderSecao, setLoaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(state => state.usuario.permissoes);
  const [modoEdicaoConsulta, setModoEdicaoConsulta] = useState(false);

  const [anosLetivos, setAnosLetivos] = useState([]);
  const [modalidades, setModalidades] = useState(todosTurmasModalidade);
  const [dres, setDres] = useState(todos);
  const [ues, setUes] = useState(todos);
  const [turmas, setTurmas] = useState(todosTurmasModalidade);
  const [alunos, setAlunos] = useState([]);
  const [alunosSelecionados, setAlunosSelecionado] = useState([]);

  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [alunoEspecificado, setAlunoEspecificado] = useState();
  const [gruposId, setGruposId] = useState([]);
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState();

  const [alunosLoader, setAlunosLoader] = useState(false);

  const [unidadeEscolarUE, setUnidadeEscolarUE] = useState(false);

  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [listaCalendario, setListaCalendario] = useState([]);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');

  const [carregandoEventos, setCarregandoEventos] = useState(false);
  const [listaEvento, setListaEvento] = useState([]);
  const [selecionouEvento, setSelecionouEvento] = useState(false);
  const [valorEvento, setValorEvento] = useState('');
  const [eventoSelecionado, setEventoSelecionado] = useState('');
  const [pesquisaEvento, setPesquisaEvento] = useState('');
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);

  const [idComunicado, setIdComunicado] = useState();

  const [carregouInformacoes, setCarregouInformacoes] = useState(false);
  const [refForm, setRefForm] = useState({});
  const [
    bloquearCamposCalendarioEventos,
    setBloquearCamposCalendarioEventos,
  ] = useState(false);

  const selecionaTipoCalendario = (descricao, form, tipoCalend, onChange) => {
    let tipo = '';

    if (tipoCalend) {
      tipo = tipoCalend;
    } else {
      tipo = listaCalendario?.find(t => {
        return t.descricao === descricao;
      });
    }

    if (tipo?.id) {
      setValorTipoCalendario(tipo.descricao);
      setTipoCalendarioSelecionado(tipo.id);
      if (form && form.setFieldValue) {
        form.setFieldValue('tipoCalendarioId', tipo.descricao);
      }
    } else {
      setValorTipoCalendario('');
      setTipoCalendarioSelecionado();
      if (form && form.setFieldValue) {
        form.setFieldValue('tipoCalendarioId', '');
      }
    }

    if (onChange) {
      selecionaEvento('', form);
    }
  };

  const selecionaEvento = (nome, form, onChange, tipoEvento) => {
    let evento = '';

    if (tipoEvento) {
      evento = tipoEvento;
    } else {
      evento = listaEvento?.find(t => {
        return t.nome === nome;
      });
    }

    if (evento?.id) {
      setSelecionouEvento(true);
      setValorEvento(evento.nome);
      setEventoSelecionado(evento);
      if (form && form.setFieldValue) {
        form.setFieldValue('eventoId', evento.nome);
      }
    } else if (!onChange) {
      setSelecionouEvento(false);
      setValorEvento('');
      setEventoSelecionado('');
      if (form && form.setFieldValue) {
        form.setFieldValue('eventoId', '');
      }
    }

    if (onChange && !evento) {
      setValorEvento(nome);
    }
  };

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
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTipos(false));

      if (isSubscribed && !bloquearCamposCalendarioEventos) {
        let allowedList = filterAllowedCalendarTypes(data);
        setListaCalendario(allowedList);

        if (idComunicado) {
          selecionaTipoCalendario(
            allowedList.length > 0 ? allowedList[0].descricao : '',
            refForm,
            allowedList.length > 0
              ? allowedList?.find(t => {
                  return t.id === valoresIniciais.tipoCalendarioId;
                })
              : ''
          );
        } else {
          selecionaTipoCalendario(
            allowedList.length > 0 ? allowedList[0].descricao : '',
            refForm,
            allowedList.length > 0
              ? allowedList?.find(t => {
                  return t.descricao === allowedList[0].descricao;
                })
              : ''
          );
        }
        setCarregandoTipos(false);
        return;
      }
      selecionaTipoCalendario('', refForm, '', true);
      selecionaEvento('', refForm);
    })();

    return () => {
      isSubscribed = false;
    };
  };

  const loadEventosEffect = () => {
    let isSubscribed = true;

    (async () => {
      setCarregandoEventos(true);

      const _form = refForm?.state?.values;
      let filter = {
        tipoCalendario: +(tipoCalendarioSelecionado ?? null),
        anoLetivo: +(_form?.anoLetivo ?? null),
        modalidade: +(_form?.modalidade ?? null),
        codigoDre:
          _form?.CodigoDre && _form?.CodigoDre != TODAS_DRE_ID
            ? null
            : _form?.CodigoDre,
        codigoUe:
          _form?.CodigoUe && _form?.CodigoUe != TODAS_UE_ID
            ? null
            : _form?.codigoUe,
      };

      Object.keys(filter).forEach(key => {
        if (filter[key] == null || filter[key] == TODAS_DRE_ID)
          delete filter[key];
      });

      const retorno = await ServicoComunicadoEvento.listarPor(filter)
        .catch(e => erros(e))
        .finally(() => setCarregandoEventos(false));

      if (isSubscribed) {
        if (retorno?.data?.length) {
          retorno.data.forEach(
            item =>
              (item.nome = `${item.id} - ${item.nome} (${item.tipoEvento})`)
          );
        }

        if (idComunicado) {
          selecionaEvento(
            '',
            refForm,
            false,
            retorno.data.length > 0
              ? retorno.data?.find(t => {
                  return t.id === valoresIniciais.eventoId;
                })
              : ''
          );
        }
        if (retorno?.data?.length) {
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
  const [anosModalidade, setAnosModalidade] = useState([]);

  useEffect(() => {
    setSomenteConsulta(
      verificaSomenteConsulta(
        permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]
      )
    );
  }, [permissoesTela]);

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setIdComunicado(match.params.id);
      setBreadcrumbManual(match.url, '', RotasDto.ACOMPANHAMENTO_COMUNICADOS);
    }
  }, [match]);

  useEffect(() => {
    const anoLetivo = refForm?.state?.values?.anoLetivo;
    if (anoLetivo){
      ObterModalidades(TODAS_MODALIDADES_ID, anoLetivo);
    }
  }, [refForm?.state?.values?.anoLetivo]);

  const valoresIniciaisImutaveis = {
    id: 0,
    gruposId: [],
    anosModalidade: [],
    seriesResumidas: '',
    dataEnvio: '',
    dataExpiracao: '',
    titulo: '',
    descricao: '',
    anoLetivo: moment().year(),
    CodigoDre: TODAS_DRE_ID,
    CodigoUe: TODAS_UE_ID,
    alunosEspecificados: false,
    modalidade: TODAS_MODALIDADES_ID,
    semestre: '',
    turmas: [TODAS_TURMAS_ID],
    alunos: '1',
    tipoCalendarioId: '',
    eventoId: '',
  };

  const [valoresIniciais, setValoresIniciais] = useState(
    valoresIniciaisImutaveis
  );

  const handleModoEdicao = () => {
    if (!modoEdicao && carregouInformacoes) setModoEdicao(true);
  };

  const [descricaoComunicado, setDescricaoComunicado] = useState('');

  const onChangeDescricaoComunicado = descricao => {
    setDescricaoComunicado(descricao);
    handleModoEdicao();
  };

  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '',
    alteradoPor: '',
    alteradoRF: '',
    criadoEm: '',
    criadoPor: '',
    criadoRF: '',
  });

  useEffect(() => {
    async function obterPorId(id) {
      const comunicado = await ServicoComunicados.consultarPorId(id);
      if (comunicado && Object.entries(comunicado).length) {
        setValoresIniciais({
          id: comunicado.id,
          anoLetivo: comunicado.anoLetivo,
          gruposId: [...comunicado.grupos.map(grupo => String(grupo.id))],
          CodigoDre: comunicado.codigoDre
            ? String(comunicado.codigoDre)
            : TODAS_DRE_ID,
          CodigoUe: comunicado.codigoUe
            ? String(comunicado.codigoUe)
            : TODAS_UE_ID,
          modalidade:
            String(comunicado.modalidade) === '0'
              ? TODAS_MODALIDADES_ID
              : String(comunicado.modalidade),
          semestre:
            String(comunicado.semestre) === '0'
              ? ''
              : String(comunicado.semestre),
          alunos: comunicado.alunoEspecificado ? '2' : '1',
          turmas:
            comunicado.turmas.length > 0
              ? [...comunicado.turmas.map(turma => String(turma.codigoTurma))]
              : [TODAS_TURMAS_ID],
          dataEnvio: comunicado.dataEnvio
            ? window.moment(comunicado.dataEnvio)
            : '',
          dataExpiracao: comunicado.dataExpiracao
            ? window.moment(comunicado.dataExpiracao)
            : '',
          titulo: comunicado.titulo,
          descricao: comunicado.descricao,
          tipoCalendarioId: comunicado.tipoCalendarioId,
          eventoId: comunicado.eventoId,
        });

        setModoEdicaoConsulta(true);

        if (comunicado.alunoEspecificado) {
          await ObterAlunos(
            comunicado.turmas[0].codigoTurma,
            comunicado.anoLetivo
          );
          setAlunoEspecificado(comunicado.alunoEspecificado);
          setAlunosSelecionado(comunicado.alunos.map(x => x.alunoCodigo));
        }

        setDescricaoComunicado(comunicado.descricao);

        ObterUes(String(comunicado.codigoDre));
        ObterModalidades(String(comunicado.codigoUe), comunicado.anoLetivo);

        if (modoEdicaoConsulta) {
          ObterTurmas(
            String(comunicado.anoLetivo),
            String(comunicado.codigoUe ? comunicado.codigoUe : '-99'),
            String(
              comunicado.modalidade
                ? comunicado.modalidade
                : TODAS_MODALIDADES_ID
            ),
            String(comunicado.semestre)
          );
        }

        setInseridoAlterado({
          alteradoEm: comunicado.alteradoEm,
          alteradoPor: comunicado.alteradoPor,
          alteradoRF: comunicado.alteradoRF,
          criadoEm: comunicado.criadoEm,
          criadoPor: comunicado.criadoPor,
          criadoRF: comunicado.criadoRF,
        });

        setCarregouInformacoes(true);
      }
    }

    if (idComunicado) {
      obterPorId(idComunicado);
      setInseridoAlterado({});
      setDescricaoComunicado('');
      setNovoRegistro(false);
    } else {
      setCarregouInformacoes(true);
    }
  }, [idComunicado]);

  const validarBloquearCamposCalendarioEventos = ue => {
    const ueEscolhida = ues?.find(item => item?.id?.toString() === ue);
    const ueEncontrada = TIPOS_ESCOLA_BLOQUEAR.find(
      id => id === ueEscolhida?.tipoEscola
    );

    return !!ueEncontrada;
  };

  const validacoes = Yup.object({
    descricao: Yup.string().required('Campo obrigatório'),
    anoLetivo: Yup.string().required('Campo obrigatório'),
    dataEnvio: momentSchema.required('Campo obrigatório'),
    CodigoDre: Yup.string().required('Campo obrigatório'),
    CodigoUe: Yup.string().required('Campo obrigatório'),
    gruposId: Yup.string().required('Campo obrigatório'),
    eventoId: Yup.string().test(
      'validaEventoId',
      'Campo obrigatório',
      function validar() {
        const { CodigoUe, eventoId } = this.parent;
        const bloquear = validarBloquearCamposCalendarioEventos(CodigoUe);
        if (bloquear) {
          return true;
        }
        if (CodigoUe !== '-99' && !eventoId) {
          return false;
        }
        return true;
      }
    ),
    tipoCalendarioId: Yup.string().test(
      'validaTipoCalendarioId',
      'Campo obrigatório',
      function validar() {
        const { CodigoUe, tipoCalendarioId } = this.parent;
        const bloquear = validarBloquearCamposCalendarioEventos(CodigoUe);
        if (bloquear) {
          return true;
        }
        if (CodigoUe !== '-99' && !tipoCalendarioId) {
          return false;
        }
        return true;
      }
    ),
    dataExpiracao: momentSchema
      .required('Campo obrigatório')
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
      )
      .test(
        'validaDataAnoMaiorQueAnoAtual',
        'Data de expiração não pode ser maior que ano atual',
        function validar() {
          const { dataExpiracao } = this.parent;
          if (
            moment(dataExpiracao).format('YYYY') >
            moment(new Date()).format('YYYY')
          ) {
            return false;
          }

          return true;
        }
      ),
    titulo: Yup.string()
      .required('Campo obrigatório')
      .min(10, 'Deve conter no mínimo 10 caracteres')
      .max(50, 'Deve conter no máximo 50 caracteres'),
  });

  const validarAntesDeSalvar = form => {
    const arrayCampos = Object.keys(valoresIniciais);

    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });

    if (
      alunoEspecificado &&
      (!alunosSelecionados || alunosSelecionados.length === 0)
    ) {
      erro('Deve selecionar um ou mais estudantes');
      return;
    }

    form.validateForm().then(() => {
      if (
        refForm &&
        (!Object.entries(refForm.state.errors).length || form.isValid)
      ) {
        form.handleSubmit(form);
      }
    });
  };

  const [gruposLista, setGruposLista] = useState([]);

  const dreDesabilitada = useMemo(() => {
    return dres.length <= 1;
  }, [dres]);

  const ueDesabilitada = useMemo(() => {
    return ues.length <= 1;
  }, [ues]);

  const modalidadeDesabilitada = useMemo(() => {
    return modalidades.length <= 1 || gruposId?.length != 0;
  }, [refForm, modalidades, ues, dres, gruposId]);

  const semestreDesabilitado = useMemo(() => {
    return modalidadeSelecionada !== '3' ?? true;
  }, [modalidadeSelecionada]);

  const turmasDesabilitada = useMemo(() => {
    return (
      turmas.length <= 1 ||
      modalidadeSelecionada === TODAS_MODALIDADES_ID ||
      modalidadeSelecionada === 'Todas' ||
      modalidadeSelecionada === ''
    );
  }, [modalidadeSelecionada, anosModalidade, turmas]);

  const gruposDesabilitados = useMemo(() => {
    return (
      (!!modalidadeSelecionada &&
        modalidadeSelecionada !== '' &&
        modalidadeSelecionada !== TODAS_MODALIDADES_ID) ||
      unidadeEscolarUE
    );
  }, [modalidadeSelecionada]);

  const estudantesDesabilitados = useMemo(() => {
    return (
      (turmasSelecionadas?.length !== 1 ||
        turmasSelecionadas[0] === TODAS_TURMAS_ID) ??
      true
    );
  }, [turmasSelecionadas]);

  const estudantesVisiveis = useMemo(() => {
    return alunoEspecificado;
  }, [alunoEspecificado]);

  useEffect(() => {
    if (!refForm?.setFieldValue) return;
    let isSubscribed = true;

    async function obterListaGrupos() {
      const lista = await ServicoComunicados.listarGrupos();
      setGruposLista(lista);
    }

    obterListaGrupos();
    ObterAnoLetivo();

    return () => (isSubscribed = false);
  }, [refForm]);

  const ObterAnoLetivo = async () => {
    const dados = await FiltroHelper.ObterAnoLetivo();

    if (!dados || dados.length === 0) return;

    setAnosLetivos(dados);
    await ObterDres();
  };

  const ObterDres = async () => {
    const dados = await FiltroHelper.ObterDres();

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('CodigoDre', dados[0].id);
      ObterUes(dados[0].id);
    }

    setDres(dados);
  };

  const ObterUes = async dre => {
    const dados = await FiltroHelper.ObterUes(dre);

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('CodigoUe', dados[0].id);
      const anoLetivo = refForm?.state?.values?.anoLetivo;
      ObterModalidades(dados[0].id, anoLetivo);
    }

    setUes(dados);
  };

  const ObterModalidades = async (ue, anoLetivo) => {
    const dados = await FiltroHelper.obterModalidadesAnoLetivo(ue, anoLetivo);

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('modalidade', dados[0].id);
      setModalidadeSelecionada(dados[0].id);
      ObterGruposIdPorModalidade(dados[0].id);

      if (dados[0].id !== '3')
        ObterTurmas(refForm.state.values.anoLetivo, ue, dados[0].id, 0);
    }

    if (ue !== TODAS_MODALIDADES_ID && dados.length > 1) {
      const data = [];
      dados.forEach(value => {
        if (value.id !== TODAS_MODALIDADES_ID) {
          const dataItem = gruposLista.filter(item => item.nome === value.nome);
          data.push(...dataItem);
        }
      });
      const arrayString = data.map(x => `${x.id}`);

      refForm.setFieldValue('gruposId', arrayString);
    }

    setModalidades(dados);
  };

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
    if (!modalidade || modalidade === '') return;

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
      return;
    }
  };

  const chainLimpaAnos = (dados, modalidade) => {
    if (modalidade != TODAS_MODALIDADES_ID && modalidade != MODALIDADE_EJA_ID) {
      refForm.setFieldValue('ano', '');
      return;
    }
  };

  const ObterAnosPorModalidade = async (modalidade, codigoUe = TODAS_UE_ID) => {
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

  const ObterGruposIdPorModalidadeFiltro = async modalidade => {
    if (!modalidade || modalidade === '') return;

    const dados = await FiltroHelper.ObterGruposIdPorModalidade(modalidade);

    if (!dados || dados.length === 0) return;

    return dados;
  };

  const obterTurmasEspecificas = async anos => {
    refForm.setFieldValue('turmas', []);

    setAlunosLoader(true);
    const response = await FiltroHelper.obterTurmasEspecificas(
      refForm.state.values.CodigoUe,
      refForm.state.values.anoLetivo,
      refForm.state.values.semestre ?? '',
      refForm.state.values.modalidade,
      anos
    );

    setAlunosLoader(false);
    const dados = response.map(x => {
      return { id: x.valor, nome: x.descricao };
    });

    dados.unshift({ id: TODAS_TURMAS_ID, nome: 'Todas' });
    if (dados.length == 1) {
      refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
    }

    setTurmas(dados);
    refForm.setFieldValue('seriesResumidas', anos?.join(',') ?? '');
  };

  const ObterAlunos = async (codigoTurma, anoLetivo) => {
    setAlunosLoader(true);

    const dados = await FiltroHelper.ObterAlunos(codigoTurma, anoLetivo);

    if (!dados || dados.length === 0) return false;

    setAlunosLoader(false);
    setAlunos(dados);
    return true;
  };

  const ResetarModalidade = async () => {
    setGruposId([]);
    setModalidades(todosTurmasModalidade);
    setModalidadeSelecionada(TODAS_MODALIDADES_ID);
    refForm.setFieldValue('gruposId', []);
    refForm.setFieldValue('modalidade', TODAS_MODALIDADES_ID);
    refForm.setFieldValue('semestre', '');
  };

  const onChangeAnoLetivo = async ano => {
    handleModoEdicao();
    refForm.setFieldValue('CodigoDre', TODAS_DRE_ID);
    refForm.setFieldValue('tipoCalendarioId', '');
    onChangeDre(TODAS_DRE_ID);
    resetarTurmas();
    ResetarModalidade();

    if (ano == 0 || !ano || ano == '') {
      setDres(todos);
      return;
    }

    ObterDres();
    loadTiposCalendarioEffect();
  };

  const onChangeDre = async dre => {
    const anoLetivo = refForm?.state?.values?.anoLetivo;
    handleModoEdicao();
    refForm.setFieldValue('CodigoUe', TODAS_UE_ID);
    onChangeUe(TODAS_UE_ID);
    resetarTurmas();
    ObterModalidades(TODAS_MODALIDADES_ID, anoLetivo);
    setUnidadeEscolarUE(false);

    if (dre === TODAS_DRE_ID) {
      setUes(todos);
      return;
    }

    ObterUes(dre);
    loadTiposCalendarioEffect();
  };

  const onChangeUe = async ue => {
    const anoLetivo = refForm?.state?.values?.anoLetivo;
    handleModoEdicao();
    resetarTurmas();
    ResetarModalidade();

    if (ue === TODAS_UE_ID) {
      setTurmas(todosTurmasModalidade);
    }

    setUnidadeEscolarUE(true);
    onChangeModalidade('');
    ObterModalidades(ue, anoLetivo);
  };

  useEffect(() => {
    const ue = refForm?.state?.values?.CodigoUe;
    if (ue) {
      const bloquear = validarBloquearCamposCalendarioEventos(ue);
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
  }, [refForm?.state?.values?.CodigoUe]);

  useEffect(() => {
    if (
      modalidadeSelecionada == modalidade.ENSINO_MEDIO ||
      modalidadeSelecionada == modalidade.FUNDAMENTAL
    ) {
      ObterAnosPorModalidade(
        modalidadeSelecionada,
        refForm?.state?.values.CodigoUe ?? null
      );
    }
  }, [modalidadeSelecionada]);

  const onChangeModalidade = async modalidade => {
    handleModoEdicao();

    refForm.setFieldValue('semestre', '');
    refForm.setFieldValue('gruposId', []);

    refForm.setFieldValue('anosModalidade', []);

    setModalidadeSelecionada(modalidade);
    loadTiposCalendarioEffect();

    if (modalidade !== TODAS_MODALIDADES_ID) {
      await ObterGruposIdPorModalidade(modalidade);
      await ObterAnosPorModalidade(
        modalidade,
        refForm?.state?.values.CodigoUe ?? null
      );
    }

    if (modalidade !== MODALIDADE_EJA_ID && modalidade !== TODAS_MODALIDADES_ID)
      ObterTurmas(
        refForm.state.values.anoLetivo,
        refForm.state.values.CodigoUe,
        modalidade,
        0
      );
  };

  const resetarTurmas = () => {
    setTurmas(todosTurmasModalidade);
    refForm.setFieldValue('turmas', [TODAS_TURMAS_ID]);
    setTurmasSelecionadas([TODAS_TURMAS_ID]);
    refForm.setFieldValue('alunos', '1');
    setAlunoEspecificado(false);
    refForm.setFieldValue('alunosEspecificados', false);
    setAlunosSelecionado([]);
    setAlunos([]);
  };

  const onSemestreChange = async semestre => {
    handleModoEdicao();
    resetarTurmas();

    if (!semestre || semestre === 0) {
      ObterTurmas(
        refForm.state.values.anoLetivo,
        refForm.state.values.CodigoUe,
        refForm.state.values.modalidade,
        0
      );
    }

    ObterTurmas(
      refForm.state.values.anoLetivo,
      refForm.state.values.CodigoUe,
      refForm.state.values.modalidade,
      semestre
    );
  };

  const onChangeAnosModalidades = async anos => {
    obterTurmasEspecificas(anos);
  };

  const onClickExcluir = async () => {
    if (idComunicado) {
      const confirmado = await confirmar(
        'Atenção',
        'Você tem certeza que deseja excluir este registro?'
      );
      if (confirmado) {
        const exclusao = await ServicoComunicados.excluir([idComunicado]);
        if (exclusao && exclusao.status === 200) {
          history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
          sucesso('Registro excluído com sucesso');
        } else {
          erro(exclusao);
        }
      }
    }
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
    } else {
      history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
    }
  };

  const onClickBotaoPrincipal = form => {
    validarAntesDeSalvar(form);
  };

  const onClickSalvar = async valores => {
    const dadosSalvar = {
      ...valores,
      descricao: descricaoComunicado,
      alunos: alunosSelecionados,
      alunosEspecificados: alunoEspecificado,
      turmas: valores.turmas.filter(x => x !== TODAS_TURMAS_ID),
      modalidade: null,
      seriesResumidas: valores.anosModalidade?.join(',') ?? '',
      CodigoUe: valores.CodigoUe == TODAS_UE_ID ? 'todas' : valores.CodigoUe,
      CodigoDre:
        valores.CodigoDre == TODAS_DRE_ID ? 'todas' : valores.CodigoDre,
      dataEnvio: valores?.dataEnvio?.set({ hour: 0, minute: 0, second: 0 }),
      dataExpiracao: valores?.dataExpiracao?.set({
        hour: 23,
        minute: 59,
        second: 59,
      }),
    };

    dadosSalvar.anosModalidade = null;
    dadosSalvar.tipoCalendarioId = tipoCalendarioSelecionado ?? null;
    dadosSalvar.eventoId = eventoSelecionado?.id ?? null;
    dadosSalvar.semestre =
      dadosSalvar.semestre === '' ? 0 : dadosSalvar.semestre;

    setLoaderSecao(true);
    const salvou = await ServicoComunicados.salvar(dadosSalvar);
    if (salvou && salvou.data) {
      history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
      sucesso('Registro salvo com sucesso');
    } else {
      erro(salvou);
    }
    setLoaderSecao(false);
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
    setModoEdicaoConsulta(false);
    refForm.setFieldValue('anoLetivo', `${window.moment().year()}`);
    onChangeAnoLetivo(window.moment().year());
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) resetarTela(form);
    }
  };

  const onChangeGruposId = gruposId => {
    setGruposId(gruposId);
    refForm.setFieldValue('modalidade', TODAS_MODALIDADES_ID);
    setModalidadeSelecionada(TODAS_MODALIDADES_ID);
    handleModoEdicao();
  };

  const onChangeTurmas = turmas => {
    handleModoEdicao();
    let todasSelecionado = false;

    let turmasSalvar = turmas;

    if (turmas.length > 0) {
      todasSelecionado = turmas[turmas.length - 1] === TODAS_TURMAS_ID;

      turmasSalvar = todasSelecionado
        ? [TODAS_TURMAS_ID]
        : turmas.filter(x => x !== TODAS_TURMAS_ID);
    }

    handleModoEdicao();

    if (turmas.length <= 0) {
      todasSelecionado = true;
      turmasSalvar.push(TODAS_TURMAS_ID);
    }

    setTurmasSelecionadas(turmasSalvar);
    refForm.setFieldValue('turmas', turmasSalvar);

    if (todasSelecionado || (turmasSalvar?.length !== 1 ?? true)) {
      refForm.setFieldValue('alunos', '1');
      setAlunoEspecificado(false);
      refForm.setFieldValue('alunosEspecificados', false);
    }
  };

  useEffect(loadTiposCalendarioEffect, [
    pesquisaTipoCalendario,
    modalidadeSelecionada,
    refForm,
    valoresIniciais,
    bloquearCamposCalendarioEventos,
  ]);

  useEffect(loadEventosEffect, [
    pesquisaEvento,
    tipoCalendarioSelecionado,
    valorTipoCalendario,
    modalidadeSelecionada,
    refForm,
    valoresIniciais,
    bloquearCamposCalendarioEventos,
  ]);

  return (
    <>
      <Cabecalho pagina="Cadastro de comunicados" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <Formik
            enableReinitialize
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            ref={refFormik => setRefForm(refFormik)}
            onSubmit={valores => onClickSalvar(valores)}
            validateOnBlur
            validateOnChange
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <ButtonGroup
                  form={form}
                  novoRegistro={novoRegistro}
                  modoEdicao={modoEdicao}
                  somenteConsulta={somenteConsulta}
                  permissoesTela={
                    permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]
                  }
                  onClickExcluir={onClickExcluir}
                  onClickVoltar={onClickVoltar}
                  onClickCancelar={onClickCancelar}
                  onClickBotaoPrincipal={() => onClickBotaoPrincipal(form)}
                  desabilitarBotaoPrincipal={!modoEdicao}
                  labelBotaoPrincipal={idComunicado ? 'Alterar' : 'Cadastrar'}
                />
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
                      disabled={modoEdicaoConsulta}
                      lista={anosLetivos}
                      allowClear={false}
                      onChange={x => {
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
                      disabled={dreDesabilitada || modoEdicaoConsulta}
                      valueText="nome"
                      value={form.values.CodigoDre}
                      lista={dres}
                      allowClear={false}
                      onChange={x => {
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
                      disabled={ueDesabilitada || modoEdicaoConsulta}
                      valueOption="id"
                      valueText="nome"
                      value={form.values.CodigoUe}
                      lista={ues}
                      allowClear={false}
                      onChange={x => {
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
                      disabled={modalidadeDesabilitada || modoEdicaoConsulta}
                      onChange={x => {
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
                      lista={semestresLista}
                      allowClear
                      disabled={semestreDesabilitado || modoEdicaoConsulta}
                      onChange={x => {
                        onSemestreChange(x);
                      }}
                    />
                  </Grid>
                  <Grid cols={2}>
                    <Label control="anosModalidade" text="Ano" />
                    <SelectComponent
                      form={form}
                      id="anosModalidade"
                      name="anosModalidade"
                      placeholder="Selecione ano"
                      valueOption="ano"
                      valueText="ano"
                      value={form.values.anosModalidade}
                      lista={anosModalidade}
                      multiple
                      allowClear
                      disabled={
                        modoEdicaoConsulta ||
                        anosModalidade?.length <= 1 ||
                        (refForm &&
                          refForm.state &&
                          refForm.state.values &&
                          (refForm.state.values.modalidade ===
                            TODAS_MODALIDADES_ID ||
                            refForm.state.values.modalidade ===
                              MODALIDADE_EJA_ID ||
                            refForm.state.values.modalidade ===
                              String(modalidade.INFANTIL)))
                      }
                      onChange={value => {
                        onChangeAnosModalidades(value);
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
                      disabled={turmasDesabilitada || modoEdicaoConsulta}
                      lista={turmas}
                      allowClear={false}
                      multiple
                      onChange={codigosTurmas => onChangeTurmas(codigosTurmas)}
                    />
                  </Grid>
                </Linha>
                <Linha className="row mb-2">
                  <Grid cols={3}>
                    <Label control="gruposId" text="Grupo" />
                    <SelectComponent
                      form={form}
                      name="gruposId"
                      placeholder="Selecione um grupo"
                      valueSelect={form.values.gruposId}
                      multiple
                      onChange={onChangeGruposId}
                      lista={gruposLista}
                      valueOption="id"
                      valueText="nome"
                      disabled={gruposDesabilitados || modoEdicaoConsulta}
                    />
                  </Grid>
                  <Grid cols={3}>
                    <Label control="dataEnvio" text="Data de envio" />
                    <CampoData
                      form={form}
                      name="dataEnvio"
                      placeholder="Selecione a data de envio"
                      formatoData="DD/MM/YYYY"
                      desabilitado={somenteConsulta || modoEdicaoConsulta}
                      onChange={handleModoEdicao}
                    />
                  </Grid>
                  <Grid cols={3}>
                    <Label control="dataExpiracao" text="Data de expiração" />
                    <CampoData
                      form={form}
                      name="dataExpiracao"
                      placeholder="Selecione a data de expiração"
                      formatoData="DD/MM/YYYY"
                      desabilitado={somenteConsulta}
                      onChange={handleModoEdicao}
                    />
                  </Grid>
                  <Grid cols={3}>
                    <Label control="Alunos" text="Estudantes"></Label>
                    <SelectComponent
                      form={form}
                      name="alunos"
                      valueSelect={form.values.alunos}
                      lista={alunosSelecionadoslist}
                      valueOption="id"
                      valueText="nome"
                      disabled={estudantesDesabilitados || modoEdicaoConsulta}
                      allowClear={false}
                      onChange={tipoAluno => {
                        setAlunoEspecificado(tipoAluno === '2');
                        refForm.setFieldValue(
                          'setAlunoEspecificado',
                          tipoAluno === '2'
                        );
                      }}
                    />
                  </Grid>
                </Linha>
                <Linha className="row mb-2">
                  <Grid cols={6}>
                    <Label
                      control="tipoCalendarioId"
                      text="Tipo de Calendário"
                    />
                    <Loader loading={carregandoTipos} tip="">
                      <SelectAutocomplete
                        disabled={
                          idComunicado || bloquearCamposCalendarioEventos
                        }
                        form={form}
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
                        onSelect={valor =>
                          selecionaTipoCalendario(valor, form, '', true)
                        }
                        onChange={valor =>
                          selecionaTipoCalendario(valor, form, '', true)
                        }
                        value={valorTipoCalendario}
                      />
                    </Loader>
                  </Grid>
                  <Grid cols={6}>
                    <Label control="evento" text="Evento" />
                    <Loader loading={carregandoEventos} tip="">
                      <SelectAutocomplete
                        disabled={
                          idComunicado || bloquearCamposCalendarioEventos
                        }
                        hideLabel
                        showList
                        isHandleSearch
                        placeholder="Selecione um evento"
                        className="col-md-12"
                        name="eventoId"
                        id="select-evento"
                        key="select-evento-key"
                        lista={listaEvento.filter(
                          item =>
                            item.nome.toLowerCase().indexOf(valorEvento) > -1
                        )}
                        valueField="id"
                        textField="nome"
                        onSelect={valor => selecionaEvento(valor, form)}
                        onChange={valor => selecionaEvento(valor, form, true)}
                        value={valorEvento}
                        form={form}
                      />
                    </Loader>
                  </Grid>
                </Linha>
                <Linha className="row mb-2">
                  <Grid cols={12}>
                    <Label control="titulo" text="Título" />
                    <CampoTexto
                      form={form}
                      name="titulo"
                      placeholder="Título do comunicado"
                      value={form.values.titulo}
                      desabilitado={somenteConsulta}
                      onChange={handleModoEdicao}
                    />
                  </Grid>
                </Linha>
                <Linha className="row mb-4">
                  <Grid cols={12}>
                    {carregouInformacoes ? (
                      <JoditEditor
                        label="Descrição"
                        form={form}
                        name="descricao"
                        value={form.values.descricao}
                        onChange={valor => {
                          if (valor !== valoresIniciais.descricao) {
                            onChangeDescricaoComunicado(valor);
                          }
                        }}
                        desabilitar={somenteConsulta}
                        permiteInserirArquivo={false}
                      />
                    ) : (
                      ''
                    )}
                    <InseridoAlterado>
                      {inseridoAlterado &&
                      inseridoAlterado.criadoPor &&
                      inseridoAlterado.criadoPor.length ? (
                        <p className="pt-2">
                          INSERIDO por {inseridoAlterado.criadoPor} (
                          {inseridoAlterado.criadoRF}) em{' '}
                          {window
                            .moment(inseridoAlterado.criadoEm)
                            .format('DD/MM/YYYY HH:mm:ss')}
                        </p>
                      ) : (
                        ''
                      )}
                      {inseridoAlterado &&
                      inseridoAlterado.alteradoPor &&
                      inseridoAlterado.alteradoPor.length ? (
                        <p>
                          ALTERADO por {inseridoAlterado.alteradoPor} (
                          {inseridoAlterado.alteradoRF}) em{' '}
                          {window
                            .moment(inseridoAlterado.alteradoEm)
                            .format('DD/MM/YYYY HH:mm:ss')}
                        </p>
                      ) : (
                        ''
                      )}
                    </InseridoAlterado>
                  </Grid>
                </Linha>
                {estudantesVisiveis ? (
                  <>
                    <ListaAlunos
                      dadosAlunos={alunos}
                      alunosSelecionados={alunosSelecionados}
                      alunosLoader={alunosLoader}
                      ObterAlunos={ObterAlunos}
                      modoEdicaoConsulta={modoEdicaoConsulta}
                      onClose={() => {}}
                      onConfirm={alunosSel => {
                        setAlunosSelecionado([
                          ...alunosSelecionados,
                          ...alunosSel,
                        ]);
                      }}
                      refForm={refForm}
                    ></ListaAlunos>
                    <ListaAlunosSelecionados
                      dadosAlunos={alunos}
                      alunosSelecionados={alunosSelecionados}
                      modoEdicaoConsulta={modoEdicaoConsulta}
                      onRemove={codigoAluno => {
                        setAlunosSelecionado(
                          alunosSelecionados.filter(x => x !== codigoAluno)
                        );
                      }}
                    />
                  </>
                ) : (
                  ''
                )}
              </Form>
            )}
          </Formik>
        </Card>
      </Loader>
    </>
  );
};

ComunicadosCadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

ComunicadosCadastro.defaultProps = {
  match: {},
};

export default ComunicadosCadastro;
