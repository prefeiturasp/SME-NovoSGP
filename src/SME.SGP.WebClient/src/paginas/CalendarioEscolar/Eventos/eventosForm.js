import React, { useEffect, useState } from 'react';
import shortid from 'shortid';

// Redux
import { useSelector } from 'react-redux';

// Form
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Componentes SGP
import Cabecalho from '~/componentes-sgp/cabecalho';

// Components globais
import {
  Auditoria,
  Button,
  CampoData,
  momentSchema,
  CampoTexto,
  Card,
  Colors,
  ModalConteudoHtml,
  RadioGroupButton,
  SelectComponent,
} from '~/componentes';

// Components locais
import ModalRecorrencia from './components/ModalRecorrencia';

import eventoLetivo from '~/dtos/eventoLetivo';
import eventoTipoData from '~/dtos/eventoTipoData';
import RotasDto from '~/dtos/rotasDto';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import servicoEvento from '~/servicos/Paginas/Calendario/ServicoEvento';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Styles
import {
  CaixaDiasLetivos,
  ListaCopiarEventos,
  TextoDiasLetivos,
} from './eventos.css';

// Utils
import { parseScreenObject } from '~/utils/parsers/eventRecurrence';

const EventosForm = ({ match }) => {
  const usuarioStore = useSelector(store => store.usuario);

  const permissoesTela = usuarioStore.permissoes[RotasDto.EVENTOS];
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);

  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [exibirModalCopiarEvento, setExibirModalCopiarEvento] = useState(false);
  const [
    exibirModalRetornoCopiarEvento,
    setExibirModalRetornoCopiarEvento,
  ] = useState(false);
  const [
    eventoTipoFeriadoSelecionado,
    setEventoTipoFeriadoSelecionado,
  ] = useState(false);
  const [tipoDataUnico, setTipoDataUnico] = useState(true);
  const [desabilitarOpcaoLetivo, setDesabilitarOpcaoLetivo] = useState(true);

  const [listaMensagensCopiarEvento, setListaMensagensCopiarEvento] = useState(
    []
  );
  const [listaFeriados, setListaFeriados] = useState([]);
  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [calendarioEscolarAtual, setCalendarioEscolarAtual] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTipoEvento, setListaTipoEvento] = useState([]);
  const [listaCalendarioParaCopiar, setlistaCalendarioParaCopiar] = useState(
    []
  );
  const [
    listaCalendarioParaCopiarInicial,
    setlistaCalendarioParaCopiarInicial,
  ] = useState([]);

  const [idEvento, setIdEvento] = useState(0);
  const inicial = {
    dataFim: '',
    dataInicio: '',
    descricao: '',
    dreId: '',
    feriadoId: '',
    letivo: 1,
    nome: '',
    tipoCalendarioId: '',
    tipoEventoId: '',
    ueId: '',
    recorrenciaEventos: null,
  };
  const [valoresIniciais, setValoresIniciais] = useState(inicial);

  const opcoesLetivo = [{ label: 'Sim', value: 1 }, { label: 'Não', value: 2 }];

  const [validacoes, setValidacoes] = useState({});

  // Recorrencia de evento
  const [showModalRecorrencia, setShowModalRecorrencia] = useState(false);
  const [habilitaRecorrencia, setHabilitaRecorrencia] = useState(false);
  const [dataInicioEvento, setDataInicioEvento] = useState(null);
  const [dataAlterada, setDataAlterada] = useState(false);
  const [recorrencia, setRecorrencia] = useState(null);

  useEffect(() => {
    const montarConsultas = async () => {
      const dres = await api.get('v1/abrangencias/dres');
      setListaDres(dres.data || []);

      const tiposEvento = await api.get('v1/calendarios/eventos/tipos/listar');
      if (tiposEvento && tiposEvento.data && tiposEvento.data.items) {
        setListaTipoEvento(tiposEvento.data.items);
      } else {
        setListaTipoEvento([]);
      }
    };
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));

    montarConsultas();
  }, []);

  useEffect(() => {
    const desabilitar = novoRegistro
      ? somenteConsulta || !permissoesTela.podeIncluir
      : somenteConsulta || !permissoesTela.podeAlterar;
    setDesabilitarCampos(desabilitar);
  }, [somenteConsulta, novoRegistro]);

  useEffect(() => {
    validarConsultaModoEdicaoENovo();
  }, [listaTipoEvento]);

  useEffect(() => {
    montaValidacoes();
  }, [eventoTipoFeriadoSelecionado, tipoDataUnico]);

  const validarConsultaModoEdicaoENovo = async () => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(
        match.url,
        'Cadastro de Eventos no Calendário Escolar',
        '/calendario-escolar/eventos'
      );
      setIdEvento(match.params.id);
      consultaPorId(match.params.id);
    } else {
      montarTipoCalendarioPorId(match.params.tipoCalendarioId);

      if (listaDres && listaDres.length == 1) {
        inicial.dreId = String(listaDres[0].codigo);
        const ues = await obterUesPorDre(inicial.dreId);
        setListaUes(ues.data || []);
        if (ues.data.length == 1) {
          inicial.ueId = String(ues.data[0].codigo);
        }
      }
      inicial.tipoCalendarioId = match.params.tipoCalendarioId;
    }
  };

  const montarTipoCalendarioPorId = async id => {
    const tipoCalendario = await api.get(`v1/calendarios/tipos/${id}`);
    if (tipoCalendario && tipoCalendario.data) {
      tipoCalendario.data.id = String(tipoCalendario.data.id);
      tipoCalendario.data.descricaoTipoCalendario = `${tipoCalendario.data.anoLetivo} - ${tipoCalendario.data.nome} - ${tipoCalendario.data.descricaoPeriodo}`;
      setCalendarioEscolarAtual([tipoCalendario.data]);
    } else {
      setCalendarioEscolarAtual([]);
    }
  };

  const montaValidacoes = () => {
    let val = {
      dataInicio: momentSchema.required('Data obrigatória'),
      nome: Yup.string().required('Nome obrigatório'),
      tipoCalendarioId: Yup.string().required('Calendário obrigatório'),
      tipoEventoId: Yup.string().required('Tipo obrigatório'),
    };

    if (usuarioStore.possuiPerfilDre) {
      val.dreId = Yup.string().required('DRE obrigatória');
    }

    if (!usuarioStore.possuiPerfilSmeOuDre) {
      val.dreId = Yup.string().required('DRE obrigatória');
      val.ueId = Yup.string().required('UE obrigatória');
    }

    if (eventoTipoFeriadoSelecionado) {
      val.feriadoId = Yup.string().required('Feriado obrigatório');
    }

    if (!tipoDataUnico) {
      val.dataFim = Yup.string().required('Data obrigatória');
    }

    setValidacoes(Yup.object(val));
  };

  const eventoCalendarioEdicao = useSelector(
    state => state.calendarioEscolar.eventoCalendarioEdicao
  );

  const consultaPorId = async id => {
    const evento = await servicoEvento.obterPorId(id).catch(e => erros(e));

    if (evento && evento.data) {
      if (evento.data.dreId && evento.data.dreId > 0) {
        carregarUes(evento.data.dreId);
      }

      montarTipoCalendarioPorId(evento.data.tipoCalendarioId);

      setValoresIniciais({
        dataFim: evento.data.dataFim ? window.moment(evento.data.dataFim) : '',
        dataInicio: window.moment(evento.data.dataInicio),
        descricao: evento.data.descricao,
        dreId: !validaSeValorInvalido(evento.data.dreId)
          ? String(evento.data.dreId)
          : undefined,
        feriadoId: !validaSeValorInvalido(evento.data.feriadoId)
          ? String(evento.data.feriadoId)
          : undefined,
        letivo: evento.data.letivo,
        nome: evento.data.nome,
        tipoCalendarioId: !validaSeValorInvalido(evento.data.tipoCalendarioId)
          ? String(evento.data.tipoCalendarioId)
          : undefined,
        tipoEventoId: !validaSeValorInvalido(evento.data.tipoEventoId)
          ? String(evento.data.tipoEventoId)
          : undefined,
        ueId: !validaSeValorInvalido(evento.data.ueId)
          ? String(evento.data.ueId)
          : undefined,
        id: evento.data.id,
        recorrenciaEventos: evento.data.recorrenciaEventos,
      });
      setAuditoria({
        criadoPor: evento.data.criadoPor,
        criadoRf: evento.data.criadoRF > 0 ? evento.data.criadoRF : '',
        criadoEm: evento.data.criadoEm,
        alteradoPor: evento.data.alteradoPor,
        alteradoRf: evento.data.alteradoRF > 0 ? evento.data.alteradoRF : '',
        alteradoEm: evento.data.alteradoEm,
      });

      verificarAlteracaoLetivoEdicao(listaTipoEvento, evento.data.tipoEventoId);

      onChangeTipoEvento(evento.data.tipoEventoId);

      setExibirAuditoria(true);

      if (Object.entries(eventoCalendarioEdicao).length > 0) {
        setBreadcrumbManual(
          match.url,
          'Cadastro de Eventos no Calendário Escolar',
          '/calendario-escolar'
        );
      }
    }
  };

  const verificarAlteracaoLetivoEdicao = (listaTipos, idTipoEvento) => {
    if (!listaTipos) return;

    const tipoEdicao = listaTipos.find(x => x.id === idTipoEvento);

    if (!tipoEdicao) return;

    const tipoEventoOpcional = tipoEdicao.letivo === eventoLetivo.Opcional;

    setDesabilitarOpcaoLetivo(!tipoEventoOpcional);
  };

  const validaSeValorInvalido = valor => {
    const validacoesObjeto =
      !valor ||
      (typeof valor === 'object' && Object.entries(valor).length === 0);

    return (
      !valor ||
      valor === '' ||
      valor === null ||
      valor === 'null' ||
      valor === undefined ||
      validacoesObjeto
    );
  };

  const consultaFeriados = async () => {
    const feriados = await api.post('v1/calendarios/feriados/listar', {});
    setListaFeriados(feriados.data);
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (Object.entries(eventoCalendarioEdicao).length > 0) {
        history.push('/calendario-escolar');
      } else if (confirmado) {
        history.push('/calendario-escolar/eventos');
      }
    } else {
      if (Object.entries(eventoCalendarioEdicao).length > 0) {
        history.push('/calendario-escolar');
      } else {
        history.push('/calendario-escolar/eventos');
      }
    }
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        resetarTela(form);
      }
    }
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
    onChangeTipoEvento(form.initialValues.tipoEventoId);
  };

  const exibirModalAtualizarEventos = async () => {
    if (idEvento > 0 && !dataAlterada && valoresIniciais.recorrenciaEventos) {
      return confirmar(
        'Atualizar série',
        '',
        'Deseja também atualizar os eventos futuros pertencentes a mesma série que este?',
        'Atualizar',
        'Cancelar'
      );
    }
    return false;
  };

  const exibirModalConfirmaData = response => {
    return confirmar('Confirmar data', '', response.mensagens[0], 'Sim', 'Não');
  };

  const onClickCadastrar = async valoresForm => {
    if (tipoDataUnico) valoresForm.dataFim = valoresForm.dataInicio;

    const tiposCalendarioParaCopiar = listaCalendarioParaCopiar.map(id => {
      const calendario = listaCalendarioEscolar.find(e => e.id === id);
      return {
        tipoCalendarioId: calendario.id,
        nomeCalendario: calendario.descricaoTipoCalendario,
      };
    });

    /**
     * @description Metodo a ser disparado quando receber a mensagem do servidor
     */
    const sucessoAoSalvar = resposta => {
      if (tiposCalendarioParaCopiar && tiposCalendarioParaCopiar.length > 0) {
        setListaMensagensCopiarEvento(resposta.data);
        setExibirModalRetornoCopiarEvento(true);
      } else {
        sucesso(resposta.data[0].mensagem);
        history.push('/calendario-escolar/eventos');
      }
    };

    let payload = {};
    let cadastrado = {};
    try {
      payload = {
        ...valoresForm,
        recorrenciaEventos: recorrencia ? { ...recorrencia } : null,
        tiposCalendarioParaCopiar,
      };

      const atualizarEventosFuturos = await exibirModalAtualizarEventos();
      if (atualizarEventosFuturos) {
        payload = {
          ...payload,
          AlterarARecorrenciaCompleta: true,
        };
      }

      cadastrado = await servicoEvento.salvar(idEvento || 0, payload);
      if (cadastrado && cadastrado.status === 200) {
        sucessoAoSalvar(cadastrado);
      }
    } catch (e) {
      if (e && e.response && e.response.status === 602) {
        const confirmaData = await exibirModalConfirmaData(e.response.data);
        if (confirmaData) {
          const request = await servicoEvento.salvar(idEvento || 0, {
            ...payload,
            DataConfirmada: true,
          });
          if (request) {
            sucessoAoSalvar(request);
          }
        }
        return false;
      }
      erros(e);
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir tipo de calendário escolar',
        '',
        'Deseja realmente excluir este calendário?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const excluir = await servicoEvento
          .deletar([idEvento])
          .catch(e => erros(e));
        if (excluir) {
          sucesso('Evento excluído com sucesso.');
          history.push('/calendario-escolar/eventos');
        }
      }
    }
  };

  const onChangeDre = (dre, form) => {
    setListaUes([]);
    form.setFieldValue('ueId', undefined);
    if (dre) {
      carregarUes(dre);
    }
    onChangeCampos();
  };

  const carregarUes = async dre => {
    const ues = await obterUesPorDre(dre);
    setListaUes(ues.data || []);
  };

  const obterUesPorDre = dre => {
    return api.get(`/v1/abrangencias/dres/${dre}/ues`);
  };

  const onClickRecorrencia = () => {
    setShowModalRecorrencia(true);
  };

  const onClickCopiarEvento = async () => {
    const tiposCalendario = await api.get('v1/calendarios/tipos');
    if (
      tiposCalendario &&
      tiposCalendario.data &&
      tiposCalendario.data.length
    ) {
      tiposCalendario.data.map(item => {
        item.id = String(item.id);
        item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
      });
      const listaSemCalendarioAtual = tiposCalendario.data.filter(
        item => item.id != calendarioEscolarAtual[0].id
      );
      setListaCalendarioEscolar(listaSemCalendarioAtual);
    } else {
      setListaCalendarioEscolar([]);
    }

    setlistaCalendarioParaCopiarInicial(listaCalendarioParaCopiar);
    setExibirModalCopiarEvento(true);
  };

  const onConfirmarCopiarEvento = () => {
    onCloseCopiarConteudo();
  };

  const onCancelarCopiarEvento = () => {
    setlistaCalendarioParaCopiar(listaCalendarioParaCopiarInicial);
    onCloseCopiarConteudo();
  };

  const onCloseCopiarConteudo = () => {
    setExibirModalCopiarEvento(false);
  };

  const onChangeCopiarEvento = eventos => {
    setlistaCalendarioParaCopiar(eventos);
  };

  const onChangeTipoEvento = (evento, form) => {
    if (evento) {
      const tipoEventoSelecionado = listaTipoEvento.find(
        item => item.id == evento
      );
      if (
        tipoEventoSelecionado &&
        String(tipoEventoSelecionado.descricao).toUpperCase() === 'FERIADO'
      ) {
        setEventoTipoFeriadoSelecionado(true);
        consultaFeriados();
      } else {
        setEventoTipoFeriadoSelecionado(false);
        if (form) {
          form.setFieldValue('feriadoId', '');
        }
      }
      if (
        tipoEventoSelecionado &&
        tipoEventoSelecionado.tipoData === eventoTipoData.Unico
      ) {
        setTipoDataUnico(true);
        if (form) {
          form.setFieldValue('dataFim', '');
        }
      } else if (
        tipoEventoSelecionado &&
        tipoEventoSelecionado.tipoData === eventoTipoData.InicioFim
      ) {
        setTipoDataUnico(false);
      }

      if (form && tipoEventoSelecionado && tipoEventoSelecionado.letivo) {
        if (tipoEventoSelecionado.letivo === eventoLetivo.Opcional) {
          setDesabilitarOpcaoLetivo(false);
        } else {
          setDesabilitarOpcaoLetivo(true);
          form.setFieldValue('letivo', tipoEventoSelecionado.letivo);
        }
      }
    } else {
      setEventoTipoFeriadoSelecionado(false);
    }
  };

  const montarExibicaoEventosCopiar = () => {
    return listaCalendarioParaCopiar.map((id, i) => {
      const calendario = listaCalendarioEscolar.find(e => e.id === id);
      if (calendario && calendario.descricaoTipoCalendario) {
        return (
          <div
            className="font-weight-bold"
            key={`calendario-${shortid.generate()}`}
          >
            `- ${calendario.descricaoTipoCalendario}`
          </div>
        );
      }
      return '';
    });
  };

  useEffect(() => {
    setHabilitaRecorrencia(!!dataInicioEvento);
  }, [dataInicioEvento]);

  useEffect(() => {
    if (recorrencia) {
      onCloseRecorrencia();
    }
  }, [recorrencia]);

  const onValidate = values => {
    setDataAlterada(
      valoresIniciais.id && values.dataInicio !== valoresIniciais.dataInicio
    );
    setDataInicioEvento(values.dataInicio || null);
  };

  const onCloseRecorrencia = () => {
    setShowModalRecorrencia(false);
  };

  const onSaveRecorrencia = recurrence => {
    setRecorrencia(parseScreenObject(recurrence));
  };

  const desabilitarData = current => {
    if (current) {
      return (
        current < window.moment().startOf('year') ||
        current > window.moment().endOf('year')
      );
    }
    return false;
  };

  const onCloseRetornoCopiarEvento = () => {
    setExibirModalRetornoCopiarEvento(false);
    history.push('/calendario-escolar/eventos');
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(inicial);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  return (
    <>
      <Cabecalho pagina="Cadastro de Eventos no Calendário Escolar" />
      <ModalRecorrencia
        onCloseRecorrencia={onCloseRecorrencia}
        onSaveRecorrencia={onSaveRecorrencia}
        show={showModalRecorrencia}
        initialValues={{ dataInicio: dataInicioEvento }}
      />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
          validate={values => onValidate(values)}
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row pb-5">
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 pb-2">
                  <SelectComponent
                    form={form}
                    name="tipoCalendarioId"
                    id="calendario-escolar"
                    lista={calendarioEscolarAtual}
                    valueOption="id"
                    valueText="descricaoTipoCalendario"
                    onChange={onChangeCampos}
                    placeholder=" Selecione um Calendário Escolar"
                    disabled
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 pb-2">
                  <div className="row">
                    {/* TODO - Mock */}
                    <CaixaDiasLetivos>2016</CaixaDiasLetivos>
                    <TextoDiasLetivos>
                      Nº de Dias Letivos no Calendário
                    </TextoDiasLetivos>
                  </div>
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-4 pb-2 d-flex justify-content-end">
                  <Button
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={onClickVoltar}
                  />
                  <Button
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    className="mr-2"
                    onClick={() => onClickCancelar(form)}
                    disabled={!modoEdicao}
                  />
                  <Button
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    hidden={novoRegistro}
                    onClick={onClickExcluir}
                    disabled={
                      somenteConsulta ||
                      !permissoesTela.podeExcluir ||
                      novoRegistro
                    }
                  />
                  <Button
                    label={novoRegistro ? 'Cadastrar' : 'Alterar'}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    onClick={() => validaAntesDoSubmit(form)}
                    disabled={desabilitarCampos}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
                  <SelectComponent
                    form={form}
                    name="dreId"
                    lista={listaDres}
                    valueOption="codigo"
                    valueText="nome"
                    onChange={e => onChangeDre(e, form)}
                    label="Diretoria Regional de Educação (DRE)"
                    placeholder="Diretoria Regional de Educação (DRE)"
                    disabled={desabilitarCampos}
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
                  <SelectComponent
                    form={form}
                    name="ueId"
                    lista={listaUes}
                    valueOption="codigo"
                    valueText="nome"
                    onChange={onChangeCampos}
                    label="Unidade Escolar (UE)"
                    placeholder="Unidade Escolar (UE)"
                    disabled={desabilitarCampos}
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 pb-2">
                  <CampoTexto
                    form={form}
                    label="Nome do evento"
                    placeholder="Nome do evento"
                    onChange={onChangeCampos}
                    name="nome"
                    desabilitado={desabilitarCampos}
                  />
                </div>
                <div
                  className={`col-sm-12 ${
                    eventoTipoFeriadoSelecionado
                      ? 'col-md-3 col-lg-3 col-xl-3'
                      : 'col-md-6 col-lg-6 col-xl-6'
                  } pb-2`}
                >
                  <SelectComponent
                    form={form}
                    name="tipoEventoId"
                    lista={listaTipoEvento}
                    valueOption="id"
                    valueText="descricao"
                    onChange={e => {
                      onChangeTipoEvento(e, form);
                      onChangeCampos();
                    }}
                    label="Tipo evento"
                    placeholder="Selecione um tipo"
                    disabled={desabilitarCampos}
                  />
                </div>
                {eventoTipoFeriadoSelecionado ? (
                  <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 pb-2">
                    <SelectComponent
                      form={form}
                      label="Nome feriado"
                      name="feriadoId"
                      lista={listaFeriados}
                      valueOption="id"
                      valueText="nome"
                      onChange={onChangeCampos}
                      placeholder="Selecione o feriado"
                      disabled={desabilitarCampos}
                    />
                  </div>
                ) : (
                  ''
                )}
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
                  <CampoData
                    form={form}
                    label={
                      tipoDataUnico ? 'Data do evento' : 'Data início do evento'
                    }
                    placeholder="Data início do evento"
                    formatoData="DD/MM/YYYY"
                    name="dataInicio"
                    onChange={onChangeCampos}
                    desabilitarData={desabilitarData}
                    desabilitado={desabilitarCampos}
                  />
                </div>
                {tipoDataUnico ? (
                  ''
                ) : (
                  <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
                    <CampoData
                      form={form}
                      label="Data fim do evento"
                      placeholder="Data fim do evento"
                      formatoData="DD/MM/YYYY"
                      name="dataFim"
                      onChange={onChangeCampos}
                      desabilitado={desabilitarCampos}
                    />
                  </div>
                )}
                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2">
                  <Button
                    label="Repetir"
                    icon="fas fa-retweet"
                    color={Colors.Azul}
                    border
                    className="mt-4"
                    onClick={onClickRecorrencia}
                    disabled={
                      desabilitarCampos ||
                      !habilitaRecorrencia ||
                      !!valoresIniciais.id
                    }
                  />
                  {!!recorrencia && (
                    <small>Existe recorrência cadastrada</small>
                  )}
                </div>
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    label="Letivo"
                    form={form}
                    opcoes={opcoesLetivo}
                    name="letivo"
                    valorInicial
                    onChange={onChangeCampos}
                    desabilitado={desabilitarCampos || desabilitarOpcaoLetivo}
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 pb-2">
                  <CampoTexto
                    form={form}
                    label="Descrição"
                    placeholder="Descrição"
                    onChange={onChangeCampos}
                    name="descricao"
                    type="textarea"
                    desabilitado={desabilitarCampos}
                  />
                </div>
              </div>

              <div className="col-md-12 pb-2 ">
                <div className="row">
                  <Button
                    label="Copiar Evento"
                    icon="fas fa-share"
                    color={Colors.Azul}
                    border
                    className="mt-4 mr-3"
                    onClick={onClickCopiarEvento}
                    disabled={desabilitarCampos}
                  />
                  {listaCalendarioParaCopiar &&
                  listaCalendarioParaCopiar.length ? (
                    <ListaCopiarEventos>
                      <div className="mb-1">
                        Evento será copiado para os calendários:
                      </div>
                      {montarExibicaoEventosCopiar()}
                    </ListaCopiarEventos>
                  ) : (
                    ''
                  )}
                </div>
              </div>
            </Form>
          )}
        </Formik>
        {exibirAuditoria ? (
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRf}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRf}
          />
        ) : (
          ''
        )}
        <ModalConteudoHtml
          key="copiarEvento"
          visivel={exibirModalCopiarEvento}
          onConfirmacaoPrincipal={onConfirmarCopiarEvento}
          onConfirmacaoSecundaria={onCancelarCopiarEvento}
          onClose={onCloseCopiarConteudo}
          labelBotaoPrincipal="Selecionar"
          labelBotaoSecundario="Cancelar"
          titulo="Copiar evento"
          closable={false}
          fecharAoClicarFora={false}
          fecharAoClicarEsc={false}
        >
          <SelectComponent
            id="copiar-evento-select"
            lista={listaCalendarioEscolar}
            valueOption="id"
            valueText="descricaoTipoCalendario"
            onChange={onChangeCopiarEvento}
            valueSelect={listaCalendarioParaCopiar}
            multiple
            placeholder="Selecione 1 ou mais tipos de calendários"
          />
        </ModalConteudoHtml>

        <ModalConteudoHtml
          key="retornoCopiarEvento"
          visivel={exibirModalRetornoCopiarEvento}
          onConfirmacaoSecundaria={onCloseRetornoCopiarEvento}
          onClose={onCloseRetornoCopiarEvento}
          labelBotaoSecundario="Ok"
          titulo="Cadastro de evento"
          closable={false}
          fecharAoClicarFora={false}
          fecharAoClicarEsc={false}
          esconderBotaoPrincipal
        >
          {listaMensagensCopiarEvento.map((item, i) => (
            <p key={i}>
              {item.sucesso ? (
                <strong>
                  <i className="fas fa-check text-success mr-2" />
                  {item.mensagem}
                </strong>
              ) : (
                <strong className="text-danger">
                  <i className="fas fa-times mr-3" />
                  {item.mensagem}
                </strong>
              )}
            </p>
          ))}
        </ModalConteudoHtml>
      </Card>
    </>
  );
};

export default EventosForm;
