import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import RadioGroupButton from '~/componentes/radioGroupButton';
import SelectComponent from '~/componentes/select';
import eventoTipoData from '~/dtos/eventoTipoData';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import servicoEvento from '~/servicos/Paginas/Calendario/ServicoEvento';

import { CaixaDiasLetivos, ListaCopiarEventos, TextoDiasLetivos } from './eventos.css';

const EventosForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);

  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [exibirModalCopiarEvento, setExibirModalCopiarEvento] = useState(false);
  const [eventoTipoFeriadoSelecionado, setEventoTipoFeriadoSelecionado] = useState(false);
  const [tipoDataUnico, setTipoDataUnico] = useState(true);

  const [listaFeriados, setListaFeriados] = useState([]);
  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTipoEvento, setListaTipoEvento] = useState([]);
  const [listaCalendarioParaCopiar, setlistaCalendarioParaCopiar] = useState([]);
  const [
    listaCalendarioParaCopiarInicial,
    setlistaCalendarioParaCopiarInicial,
  ] = useState([]);

  const [idEvento, setIdEvento] = useState(0);
  const [valoresIniciais, setValoresIniciais] = useState({
    dataFim: '',
    dataInicio: '',
    descricao: '',
    dreId: undefined,
    feriadoId: undefined,
    letivo: 1,
    nome: '',
    tipoCalendarioId: undefined,
    tipoEventoId: undefined,
    ueId: undefined
  });

  const opcoesLetivo = [
    { label: 'Sim', value: 1 },
    { label: 'Não', value: 2 },
  ];

  const [validacoes, setValidacoes] = useState({});

  useEffect(() => {
    const carregarDres = async () => {
      const dres = await api.get('v1/dres');
      setListaDres(dres.data);
    };

    const consultaTipos = async () => {
      const listaTipo = await api.get('v1/tipo-calendario');
      if (listaTipo && listaTipo.data && listaTipo.data.length) {
        listaTipo.data.map(item => {
          item.id = String(item.id);
          item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
        });
        setListaCalendarioEscolar(listaTipo.data);
      } else {
        setListaCalendarioEscolar([]);
      }
    };

    const consultaTipoEvento = async () => {
      const tiposEvento = await api.get('v1/calendarios/eventos/tipos/listar');
      if (tiposEvento && tiposEvento.data && tiposEvento.data.items) {
        setListaTipoEvento(tiposEvento.data.items);
      } else {
        setListaTipoEvento([]);
      }
    };

    carregarDres();
    consultaTipos();
    montaValidacoes();

    consultaTipoEvento();
    }, []);

  useEffect(() => {
    consultaModoEdicao();
  }, [listaTipoEvento]);

  useEffect(() => {
    montaValidacoes();
  }, [eventoTipoFeriadoSelecionado, tipoDataUnico]);

  const consultaModoEdicao = ()=> {
    if (match && match.params && match.params.id) {
      setBreadcrumbManual(
        match.url,
        'Cadastro de Eventos no Calendário Escolar',
        '/calendario-escolar/eventos'
        );
        setIdEvento(match.params.id);
        consultaPorId(match.params.id);
    }
  }

  const montaValidacoes = ()=> {
    let val = {
      dataInicio: momentSchema.required('Data obrigatória'),
      descricao: Yup.string().required('Descrição obrigatória'),
      dreId: Yup.string().required('DRE obrigatória'),
      nome: Yup.string().required('Nome obrigatório'),
      tipoCalendarioId: Yup.string().required('Calendário obrigatório'),
      tipoEventoId: Yup.string().required('Tipo obrigatório'),
      ueId: Yup.string().required('UE obrigatória'),
    };

    if (eventoTipoFeriadoSelecionado) {
      val.feriadoId = Yup.string().required('Feriado obrigatório');
    };

    if (!tipoDataUnico) {
      val.dataFim = Yup.string().required('Data obrigatória');
    };

    setValidacoes(Yup.object(val));
  }

  const consultaPorId = async id => {
    const evento = await api.get(`v1/calendarios/eventos/${id}`).catch(e => erros(e));

    if (evento && evento.data) {
      if (evento.data.dreId && evento.data.dreId > 0) {
        carregarUes(evento.data.dreId);
      }
      setValoresIniciais({
        dataFim: evento.data.dataFim ? window.moment(evento.data.dataFim) : '',
        dataInicio: window.moment(evento.data.dataInicio),
        descricao: evento.data.descricao,
        dreId: String(evento.data.dreId),
        feriadoId: evento.data.feriadoId || '',
        letivo: evento.data.letivo,
        nome: evento.data.nome,
        tipoCalendarioId: String(evento.data.tipoCalendarioId),
        tipoEventoId: String(evento.data.tipoEventoId),
        ueId: String(evento.data.ueId),
        id: evento.data.id
      });
      setAuditoria({
        criadoPor: evento.data.criadoPor,
        criadoRf: evento.data.criadoRF > 0 ? evento.data.criadoRF : '',
        criadoEm: evento.data.criadoEm,
        alteradoPor: evento.data.alteradoPor,
        alteradoRf: evento.data.alteradoRF > 0 ? evento.data.alteradoRF : '',
        alteradoEm: evento.data.alteradoEm,
      });

      onChangeTipoEvento(evento.data.tipoEventoId);

      setNovoRegistro(false);
      setExibirAuditoria(true);
    }
  };

  const consultaFeriados = async ()=> {
    const feriados = await api.post('v1/calendarios/feriados/listar', {});
    setListaFeriados(feriados.data);
  }

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (confirmado) {
        history.push('/calendario-escolar/eventos');
      }
    } else {
      history.push('/calendario-escolar/eventos');
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

  const onClickCadastrar = async valoresForm => {
    valoresForm.listaCalendarioParaCopiar = listaCalendarioParaCopiar;
    const cadastrado = await servicoEvento.salvar(idEvento || 0, valoresForm)
      .catch(e => erros(e));
    if (cadastrado && cadastrado.status == 200) {
      sucesso('Suas informações foram salvas com sucesso.');
      history.push('/calendario-escolar/eventos');
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
        const parametrosDelete = { data: [idEvento] };
        const excluir = await api
          .delete('v1/eventos', parametrosDelete)
          .catch(erros => erros(erros));
        if (excluir) {
          sucesso('Evento excluído com sucesso.');
          history.push('/calendario-escolar/eventos');
        }
      }
    }
  };

  const onChangeDre = dre => {
    setListaUes([]);
    if (dre) {
      carregarUes(dre);
    }
    onChangeCampos();
  };

  const carregarUes = async dre => {
    const ues = await api.get(`/v1/dres/${dre}/ues`);
    setListaUes(ues.data || []);
  };

  const onClickRepetir = () => {
    console.log('onClickRepetir');
  };

  const onClickCopiarEvento = () => {
    setlistaCalendarioParaCopiarInicial(listaCalendarioParaCopiar);
    setExibirModalCopiarEvento(true);
  };

  const onConfirmarCopiarEvento = () => {
    onCloseCopiarConteudo();
    console.log(listaCalendarioParaCopiar);
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
      const tipoEventoSelecionado = listaTipoEvento.find(item => item.id == evento)
      if (tipoEventoSelecionado && String(tipoEventoSelecionado.descricao).toUpperCase() === 'FERIADO') {
        setEventoTipoFeriadoSelecionado(true);
        consultaFeriados();
      } else {
        setEventoTipoFeriadoSelecionado(false);
        if (form) {
          form.setFieldValue('feriadoId', '');
        }
      }
      if (tipoEventoSelecionado && tipoEventoSelecionado.tipoData === eventoTipoData.Unico) {
        setTipoDataUnico(true);
        if (form) {
          form.setFieldValue('dataFim', '');
        }
      } else if (tipoEventoSelecionado && tipoEventoSelecionado.tipoData === eventoTipoData.InicioFim) {
        setTipoDataUnico(false);
      }
    } else {
      setEventoTipoFeriadoSelecionado(false);
    }
  };

  const montarEcibicaoEventosCopiar = () => {
    return listaCalendarioParaCopiar.map((id, i)=> {
      const calendario = listaCalendarioEscolar.find(e => e.id == id);
      if (calendario && calendario.descricaoTipoCalendario) {
        return <div className="font-weight-bold"  key={'calendario-' + i} >{ '-  ' + calendario.descricaoTipoCalendario}</div>;
      } else {
        return '';
      }
    });
  };

  return (
    <>
      <Cabecalho pagina="Cadastro de Eventos no Calendário Escolar" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row pb-5">
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 pb-2">
                  <SelectComponent
                    form={form}
                    name="tipoCalendarioId"
                    id="calendario-escolar"
                    lista={listaCalendarioEscolar}
                    valueOption="id"
                    valueText="descricaoTipoCalendario"
                    onChange={onChangeCampos}
                    placeholder=" Selecione um Calendário Escolar"
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 pb-2">
                  <div className="row">
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
                    onClick={()=> onClickCancelar(form)}
                    disabled={!modoEdicao}
                  />
                  <Button
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    hidden={novoRegistro}
                    onClick={onClickExcluir}
                  />
                  <Button
                    label="Cadastrar"
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    type="submit"
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
                  <SelectComponent
                    form={form}
                    name="dreId"
                    lista={listaDres}
                    valueOption="id"
                    valueText="nome"
                    onChange={onChangeDre}
                    label="Diretoria Regional de Educação (DRE)"
                    placeholder="Diretoria Regional de Educação (DRE)"
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
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 pb-2">
                  <CampoTexto
                    form={form}
                    label="Nome do evento"
                    placeholder="Nome do evento"
                    onChange={onChangeCampos}
                    name="nome"
                  />
                </div>
                <div className={
                  `col-sm-12 ${eventoTipoFeriadoSelecionado ?
                      'col-md-3 col-lg-3 col-xl-3' : 'col-md-6 col-lg-6 col-xl-6' } pb-2`
                  }>
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
                  />
                </div>
                {
                  eventoTipoFeriadoSelecionado ?
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
                      />
                    </div> : ''
                }
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
                  <CampoData
                    form={form}
                    label="Data início do evento"
                    placeholder="Data início do evento"
                    formatoData="DD/MM/YYYY"
                    name="dataInicio"
                    onChange={onChangeCampos}
                  />
                </div>
                {
                  tipoDataUnico ? '' :
                    <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
                      <CampoData
                        form={form}
                        label="Data fim do evento"
                        placeholder="Data fim do evento"
                        formatoData="DD/MM/YYYY"
                        name="dataFim"
                        onChange={onChangeCampos}
                      />
                    </div>
                }
                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2">
                  <Button
                    label="Repetir"
                    icon="fas fa-retweet"
                    color={Colors.Azul}
                    border
                    className="mt-4"
                    onClick={onClickRepetir}
                  />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    label="Letivo"
                    form={form}
                    opcoes={opcoesLetivo}
                    name="letivo"
                    valorInicial
                    onChange={onChangeCampos}
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
                  />
                  {
                    listaCalendarioParaCopiar && listaCalendarioParaCopiar.length ?
                      <ListaCopiarEventos>
                        <div className="mb-1">Evento será copiado para os calendários:</div>
                      { montarEcibicaoEventosCopiar() }
                      </ListaCopiarEventos>
                    : ''
                  }
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
          />
        </ModalConteudoHtml>
      </Card>
    </>
  );
};

export default EventosForm;
