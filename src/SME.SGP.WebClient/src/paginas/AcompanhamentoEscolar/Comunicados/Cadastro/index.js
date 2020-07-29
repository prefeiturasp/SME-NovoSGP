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
  Editor,
  ModalConteudoHtml,
  Button,
} from '~/componentes';
import ListaAlunos from '~/paginas/AcompanhamentoEscolar/Comunicados/Cadastro/Lista/ListaAlunos';

import { Linha } from '~/componentes/EstilosGlobais';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import { confirmar, erro, sucesso, erros } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';

import FiltroHelper from '~/paginas/AcompanhamentoEscolar/Comunicados/Helper/helper.js';
import { Titulo } from '~/paginas/Planejamento/PlanoCiclo/planoCiclo.css';
import { exception } from 'react-ga';
import ListaAlunosSelecionados from './Lista/ListaAlunosSelecionados';

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

  const todos = [{ id: 'todas', nome: 'Todas' }];
  const todosTurmasModalidade = [{ id: '-99', nome: 'Todas' }];
  const semestresLista = [
    { id: '1', nome: '1º Semestre' },
    { id: '2', nome: '2º Semestre' },
  ];
  const alunosSelecionadoslist = [
    { id: '1', nome: 'Todos' },
    { id: '2', nome: 'Alunos Especificados' },
  ];

  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(state => state.usuario.permissoes);
  const [modoEdicaoConsulta, setModoEdicaoConsulta] = useState(false);

  const [anosLetivos, setAnosLetivos] = useState([]);
  const [modalidades, setModalidades] = useState(todosTurmasModalidade);
  const [dres, setDres] = useState(todos);
  const [ues, setUes] = useState(todos);
  const [semestres, setSemestres] = useState(semestresLista);
  const [turmas, setTurmas] = useState(todosTurmasModalidade);
  const [alunos, setAlunos] = useState([]);
  const [alunosSelecionados, setAlunosSelecionado] = useState([]);

  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [alunoEspecificado, setAlunoEspecificado] = useState();
  const [gruposId, setGruposId] = useState([]);
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState();

  const [alunosLoader, setAlunosLoader] = useState(false);

  useEffect(() => {
    setSomenteConsulta(
      verificaSomenteConsulta(
        permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]
      )
    );
  }, [permissoesTela]);

  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);

  const [idComunicado, setIdComunicado] = useState();

  useEffect(() => {
    if (match && match.params && match.params.id) {
      setIdComunicado(match.params.id);
      setBreadcrumbManual(match.url, '', RotasDto.ACOMPANHAMENTO_COMUNICADOS);
    }
  }, [match]);

  const valoresIniciaisImutaveis = {
    id: 0,
    gruposId: [],
    dataEnvio: '',
    dataExpiracao: '',
    titulo: '',
    descricao: '',
    anoLetivo: moment().year(),
    CodigoDre: 'todas',
    CodigoUe: 'todas',
    alunosEspecificados: false,
    modalidade: '-99',
    semestre: '',
    turmas: ['-99'],
    alunos: '1',
  };

  const [valoresIniciais, setValoresIniciais] = useState(
    valoresIniciaisImutaveis
  );

  const handleModoEdicao = () => {
    if (!modoEdicao) setModoEdicao(true);
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
            : 'todas',
          CodigoUe: comunicado.codigoUe ? String(comunicado.codigoUe) : 'todas',
          modalidade:
            String(comunicado.modalidade) === '0'
              ? '-99'
              : String(comunicado.modalidade),
          semestre:
            String(comunicado.semestre) === '0'
              ? ''
              : String(comunicado.semestre),
          alunos: comunicado.alunoEspecificado ? '2' : '1',
          turmas:
            comunicado.turmas.length > 0
              ? [...comunicado.turmas.map(turma => String(turma.codigoTurma))]
              : ['-99'],
          dataEnvio: comunicado.dataEnvio
            ? window.moment(comunicado.dataEnvio)
            : '',
          dataExpiracao: comunicado.dataExpiracao
            ? window.moment(comunicado.dataExpiracao)
            : '',
          titulo: comunicado.titulo,
          descricao: comunicado.descricao,
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
        ObterModalidades(String(comunicado.codigoUe));
        ObterTurmas(
          String(comunicado.anoLetivo),
          String(comunicado.codigoUe),
          String(comunicado.modalidade),
          String(comunicado.semestre)
        );

        setInseridoAlterado({
          alteradoEm: comunicado.alteradoEm,
          alteradoPor: comunicado.alteradoPor,
          alteradoRF: comunicado.alteradoRF,
          criadoEm: comunicado.criadoEm,
          criadoPor: comunicado.criadoPor,
          criadoRF: comunicado.criadoRF,
        });
      }
    }

    if (idComunicado) {
      obterPorId(idComunicado);
      setInseridoAlterado({});
      setDescricaoComunicado('');
      setNovoRegistro(false);
    }
  }, [idComunicado]);

  const [validacoes] = useState(
    Yup.object({
      anoLetivo: Yup.string().required('Campo obrigatório'),
      dataEnvio: momentSchema.required('Campo obrigatório'),
      CodigoDre: Yup.string().required('Campo obrigatório'),
      CodigoUe: Yup.string().required('Campo obrigatório'),
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
      titulo: Yup.string()
        .required('Campo obrigatório')
        .min(10, 'Deve conter no mínimo 10 caracteres')
        .max(50, 'Deve conter no máximo 50 caracteres'),
    })
  );

  const [temErroDescricao, setTemErroDescricao] = useState(false);

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

    const descricao = descricaoComunicado.replace('<p><br></p>', '');

    form.validateForm().then(() => {
      setTemErroDescricao(!descricao.length);
      if (
        refForm &&
        (!Object.entries(refForm.state.errors).length || form.isValid) &&
        descricao.length
      ) {
        form.handleSubmit(form);
      }
    });
  };

  const [refForm, setRefForm] = useState({});

  const [gruposLista, setGruposLista] = useState([]);

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
    return modalidadeSelecionada !== '3' ?? true;
  }, [modalidadeSelecionada]);

  const turmasDesabilitada = useMemo(() => {
    return turmas.length <= 1;
  }, [turmas]);

  const gruposDesabilitados = useMemo(() => {
    return (
      !!modalidadeSelecionada &&
      modalidadeSelecionada !== '' &&
      modalidadeSelecionada !== '-99'
    );
  }, [modalidadeSelecionada]);

  const estudantesDesabilitados = useMemo(() => {
    return (
      (turmasSelecionadas?.length !== 1 || turmasSelecionadas[0] === '-99') ??
      true
    );
  }, [turmasSelecionadas]);

  const estudantesVisiveis = useMemo(() => {
    return alunoEspecificado;
  }, [alunoEspecificado]);

  useEffect(() => {
    if (!refForm?.setFieldValue) return;

    async function obterListaGrupos() {
      const lista = await ServicoComunicados.listarGrupos();
      setGruposLista(lista);
    }
    obterListaGrupos();
    ObterAnoLetivo();
  }, [refForm]);

  const ObterAnoLetivo = async () => {
    const dados = await FiltroHelper.ObterAnoLetivo();

    if (!dados || dados.length === 0) return;

    setAnosLetivos(dados);
    ObterDres();
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
      ObterModalidades(dados[0].id);
    }

    setUes(dados);
  };

  const ObterModalidades = async ue => {
    const dados = await FiltroHelper.ObterModalidades(ue);

    if (!dados || dados.length === 0) return;

    if (dados.length === 1) {
      refForm.setFieldValue('modalidade', dados[0].id);
      setModalidadeSelecionada(dados[0].id);
      ObterGruposIdPorModalidade(dados[0].id);

      if (dados[0].id !== '3')
        ObterTurmas(refForm.state.values.anoLetivo, ue, dados[0].id, 0);
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
    setModalidadeSelecionada('-99');
    refForm.setFieldValue('gruposId', []);
    refForm.setFieldValue('modalidade', '-99');
    refForm.setFieldValue('semestre', '');
  };

  const onChangeAnoLetivo = async ano => {
    handleModoEdicao();
    refForm.setFieldValue('CodigoDre', 'todas');
    onChangeDre('todas');
    resetarTurmas();
    ResetarModalidade();

    if (ano == 0 || !ano || ano == '') {
      setDres(todos);
      return;
    }

    ObterDres();
  };

  const onChangeDre = async dre => {
    handleModoEdicao();
    refForm.setFieldValue('CodigoUe', 'todas');
    onChangeUe('todas');
    resetarTurmas();
    ResetarModalidade();

    if (dre == 'todas') {
      setUes(todos);
      return;
    }

    ObterUes(dre);
  };

  const onChangeUe = async ue => {
    handleModoEdicao();
    refForm.setFieldValue('modalidade', '');
    resetarTurmas();
    ResetarModalidade();

    if (ue == 'todas') {
      setModalidades(todosTurmasModalidade);
      setTurmas(todosTurmasModalidade);
      return;
    }

    onChangeModalidade('');
    ObterModalidades(ue);
  };

  const onChangeModalidade = async modalidade => {
    handleModoEdicao();

    refForm.setFieldValue('semestre', '');
    refForm.setFieldValue('gruposId', []);
    setModalidadeSelecionada(modalidade);
    resetarTurmas();

    if (
      !refForm.state.values.CodigoUe ||
      refForm.state.values.CodigoUe === 'todas'
    ) {
      setGruposId([]);
      refForm.setFieldValue('gruposId', []);
      return;
    }

    if (modalidade !== '-99') ObterGruposIdPorModalidade(modalidade);

    if (modalidade !== '3' && modalidade !== '-99')
      ObterTurmas(
        refForm.state.values.anoLetivo,
        refForm.state.values.CodigoUe,
        modalidade,
        0
      );
  };

  const resetarTurmas = () => {
    setTurmas(todosTurmasModalidade);
    refForm.setFieldValue('turmas', ['-99']);
    setTurmasSelecionadas(['-99']);
    refForm.setFieldValue('alunos', '1');
    setAlunoEspecificado(false);
    refForm.setFieldValue('alunosEspecificados', false);
    setAlunosSelecionado([]);
    setAlunos([]);
  };

  const onSemestreChange = async semestre => {
    handleModoEdicao();
    resetarTurmas();

    if (!semestre || semestre == 0) {
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
      turmas: valores.turmas.filter(x => x !== '-99'),
      modalidade: valores.modalidade === '-99' ? '' : valores.modalidade,
    };

    dadosSalvar.semestre =
      dadosSalvar.semestre === '' ? 0 : dadosSalvar.semestre;

    const salvou = await ServicoComunicados.salvar(dadosSalvar);
    if (salvou && salvou.data) {
      history.push(RotasDto.ACOMPANHAMENTO_COMUNICADOS);
      sucesso('Registro salvo com sucesso');
    } else {
      erro(salvou);
    }
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
    handleModoEdicao();
  };

  const onChangeTurmas = turmas => {
    handleModoEdicao();
    let todasSelecionado = false;

    let turmasSalvar = turmas;

    if (turmas.length > 0) {
      todasSelecionado = turmas[turmas.length - 1] === '-99';

      turmasSalvar = todasSelecionado
        ? ['-99']
        : turmas.filter(x => x !== '-99');
    }

    handleModoEdicao();

    if (turmas.length <= 0) {
      todasSelecionado = true;
      turmasSalvar.push('-99');
    }

    setTurmasSelecionadas(turmasSalvar);
    refForm.setFieldValue('turmas', turmasSalvar);

    if (todasSelecionado || (turmasSalvar?.length !== 1 ?? true)) {
      refForm.setFieldValue('alunos', '1');
      setAlunoEspecificado(false);
      refForm.setFieldValue('alunosEspecificados', false);
    }
  };

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
                    <Label control="CodigoDre" text="Dre" />
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
                    <Label control="CodigoUe" text="Unidade Escolar" />
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
                      lista={semestres}
                      allowClear
                      disabled={semestreDesabilitado || modoEdicaoConsulta}
                      onChange={x => {
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
                    <Label control="textEditor" text="Descrição" />
                    <Editor
                      form={form}
                      name="descricao"
                      inicial={descricaoComunicado}
                      onChange={onChangeDescricaoComunicado}
                      desabilitar={somenteConsulta}
                      temErro={temErroDescricao}
                    />
                    {temErroDescricao && (
                      <ErroValidacao>Campo obrigatório</ErroValidacao>
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
