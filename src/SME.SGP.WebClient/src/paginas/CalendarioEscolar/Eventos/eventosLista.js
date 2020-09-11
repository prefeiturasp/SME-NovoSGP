import React, { useEffect, useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import { Form, Formik } from 'formik';
import * as moment from 'moment';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import shortid from 'shortid';

import {
  Alert,
  Button,
  CampoData,
  CampoTexto,
  Card,
  Colors,
  Grid,
  ListaPaginada,
  Loader,
  momentSchema,
  SelectComponent,
  SelectAutocomplete,
} from '~/componentes';
import { Cabecalho, FiltroHelper } from '~/componentes-sgp';

import { URL_HOME } from '~/constantes';

import { RotasDto } from '~/dtos';

import {
  api,
  confirmar,
  erro,
  erros,
  history,
  ServicoCalendarios,
  ServicoEvento,
  setBreadcrumbManual,
  sucesso,
  verificaSomenteConsulta,
} from '~/servicos';

const EventosLista = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.EVENTOS];

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [listaCalendario, setListaCalendario] = useState([]);
  const [listaDre, setListaDre] = useState([]);
  const [campoUeDesabilitado, setCampoUeDesabilitado] = useState(true);
  const [dreSelecionada, setDreSelecionada] = useState();
  const [listaUe, setListaUe] = useState([]);
  const [nomeEvento, setNomeEvento] = useState('');
  const [listaTipoEvento, setListaTipoEvento] = useState([]);
  const [tipoEvento, setTipoEvento] = useState(undefined);
  const [mensagemAlerta, setMesangemAlerta] = useState(false);
  const [eventosSelecionados, setEventosSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [selecionouCalendario, setSelecionouCalendario] = useState(false);
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    ''
  );
  const [refForm, setRefForm] = useState();

  const [valoresIniciais] = useState({
    tipoCalendarioId: undefined,
    dreId: undefined,
    ueId: undefined,
    ehTodasDres: false,
    ehTodasUes: false,
    dataInicio: '',
    dataFim: '',
  });

  const [filtroValido, setFiltroValido] = useState({ valido: false });
  const [validacoes] = useState(
    Yup.object({
      dataInicio: momentSchema.test(
        'validaInicio',
        'Data obrigatória',
        function validar() {
          const { dataInicio } = this.parent;
          const { dataFim } = this.parent;
          if (!dataInicio && dataFim) {
            return false;
          }
          return true;
        }
      ),
      dataFim: momentSchema.test(
        'validaFim',
        'Data obrigatória',
        function validar() {
          const { dataInicio } = this.parent;
          const { dataFim } = this.parent;
          if (dataInicio && !dataFim) {
            return false;
          }
          return true;
        }
      ),
    })
  );

  const validarFiltrar = useCallback(async () => {
    if (refForm) {
      const valido = await refForm.validateForm();
      if (valido) {
        refForm.handleSubmit(e => e);
        setFiltroValido({ valido: true });
      } else {
        setFiltroValido({ valido: false });
      }
    }
  }, [refForm]);

  const filtrar = useCallback(
    (campo, valor) => {
      const filtroAtual = filtro;
      filtroAtual[campo] = valor;
      setFiltro({ ...filtroAtual });
      validarFiltrar();
    },
    [filtro, validarFiltrar]
  );

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('DD/MM/YYYY');
    }
    return <span> {dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Nome do evento',
      dataIndex: 'nome',
      width: '45%',
    },
    {
      title: 'Tipo de evento',
      dataIndex: 'tipo',
      width: '20%',
      render: (text, row) => <span> {row.tipoEvento.descricao}</span>,
    },
    {
      title: 'Data início',
      dataIndex: 'dataInicio',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Data fim',
      dataIndex: 'dataFim',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
  ];

  const listarDres = async () => {
    const dres = await ServicoEvento.listarDres();

    if (dres.sucesso) {
      dres.conteudo.sort(FiltroHelper.ordenarLista('nome'));
      if (dres.conteudo.length > 1) {
        dres.conteudo.unshift({ codigo: 0, nome: 'Todas' });
      }
      setListaDre(dres.conteudo);
      return;
    }

    erro(dres.erro);
    setListaDre([]);
  };

  const [dreDesabilitada, setDreDesabilitada] = useState(false);
  const [ueDesabilitada, setUeDesabilitada] = useState(false);

  useEffect(() => {
    if (
      listaDre.length === 1 &&
      (usuario.possuiPerfilDre || !usuario.possuiPerfilSmeOuDre)
    ) {
      refForm.setFieldValue('dreId', listaDre[0].codigo.toString());
      setDreSelecionada(listaDre[0].codigo.toString());
      setDreDesabilitada(true);
    }
  }, [
    listaDre,
    refForm,
    usuario.possuiPerfilDre,
    usuario.possuiPerfilSmeOuDre,
  ]);

  useEffect(() => {
    const calendarioSelecionado = sessionStorage.getItem(
      'calendarioSelecionadoEventos'
    );
    if (
      refForm &&
      listaCalendario &&
      listaCalendario.length &&
      calendarioSelecionado &&
      match &&
      match.params &&
      match.params.tipoCalendarioId
    ) {
      const { tipoCalendarioId } = match.params;
      const temTipoParaSetar = listaCalendario.find(
        item => item.id == tipoCalendarioId
      );

      const calendarioConvertido =
        calendarioSelecionado && JSON.parse(calendarioSelecionado);

      const valorDescricao = temTipoParaSetar
        ? temTipoParaSetar.descricao
        : calendarioConvertido
        ? calendarioConvertido.descricao
        : null;

      if (calendarioSelecionado && !temTipoParaSetar) {
        setPesquisaTipoCalendario(calendarioConvertido.descricao);
      }

      sessionStorage.clear('calendarioSelecionadoEventos');
      refForm.setFieldValue('tipoCalendarioId', tipoCalendarioId);
      setValorTipoCalendario(valorDescricao);
      setSelecionouCalendario(true);
      filtrar('tipoCalendarioId', tipoCalendarioId);
    }
  }, [match, listaCalendario, refForm, filtrar]);

  useEffect(() => {
    const obterListaEventos = async () => {
      const tiposEvento = await api.get('v1/calendarios/eventos/tipos/listar');

      if (tiposEvento && tiposEvento.data && tiposEvento.data.items) {
        setListaTipoEvento(tiposEvento.data.items);
      } else {
        setListaTipoEvento([]);
      }
    };

    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
    obterListaEventos();
    listarDres();
  }, [permissoesTela]);

  useEffect(() => {
    const semTipoSelecionado =
      !filtro || !filtro.tipoCalendarioId || filtro.tipoCalendarioId === 0;

    setMesangemAlerta(semTipoSelecionado);
  }, [filtro]);

  useEffect(() => {
    if (listaUe.length === 1 && !usuario.possuiPerfilSmeOuDre) {
      refForm.setFieldValue('ueId', listaUe[0].codigo.toString());
      setUeDesabilitada(true);
    }
  }, [listaUe, refForm, usuario.possuiPerfilSmeOuDre]);

  useEffect(() => {
    const listarUes = async () => {
      if (dreSelecionada && dreSelecionada.toString() === '0') {
        const uesTodas = [{ codigo: 0, nome: 'Todas' }];
        setListaUe(uesTodas);
        return;
      }

      if (
        !dreSelecionada ||
        dreSelecionada === '' ||
        Object.entries(dreSelecionada).length === 0
      )
        return;

      const { tipoCalendarioId } = refForm.getFormikContext().values;
      const calendarioSelecionado = listaCalendario.find(
        item => item.id === tipoCalendarioId
      );

      const ues = await ServicoEvento.listarUes(
        dreSelecionada,
        ServicoCalendarios.converterModalidade(
          calendarioSelecionado?.modalidade
        )
      );

      if (!sucesso) {
        setListaUe([]);
        erro(ues.erro);
        setListaDre([]);
        return;
      }

      if (
        !ues.conteudo ||
        ues.conteudo.length === 0 ||
        Object.entries(ues.conteudo).length === 0
      )
        setCampoUeDesabilitado(true);

      if (ues.conteudo) {
        ues.conteudo.sort(FiltroHelper.ordenarLista('nome'));
        if (ues.conteudo.length > 1) {
          ues.conteudo.unshift({ codigo: 0, nome: 'Todas' });
        }
        setListaUe(ues.conteudo);
      }
    };
    if (dreSelecionada) listarUes();
  }, [dreSelecionada, selecionouCalendario, listaCalendario, refForm]);

  const setSessionStorage = value => {
    sessionStorage.setItem(
      'calendarioSelecionadoEventos',
      JSON.stringify(value)
    );
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUeId = async ueId => {
    filtrar('ehTodasUes', ueId === '0');
    filtrar('ueId', ueId === '0' ? '' : ueId);
  };

  const onChangeDreId = async dreId => {
    refForm.setFieldValue('ueId', undefined);
    filtrar('ehTodasDres', dreId === '0');
    filtrar('dreId', dreId === '0' ? '' : dreId);

    if (dreId) {
      setDreSelecionada(dreId);
      setCampoUeDesabilitado(false);
      return;
    }

    filtrar('ehTodasUes', false);
    filtrar('ueId', '');

    setCampoUeDesabilitado(true);
    setListaUe([]);
    setDreSelecionada([]);
  };

  const onClickExcluir = async () => {
    if (eventosSelecionados && eventosSelecionados.length > 0) {
      const listaNomeExcluir = eventosSelecionados.map(item => item.nome);
      const confirmado = await confirmar(
        'Excluir evento',
        listaNomeExcluir,
        `Deseja realmente excluir ${
          eventosSelecionados.length > 1 ? 'estes eventos' : 'este evento'
        }?`,
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const idsDeletar = eventosSelecionados.map(c => c.id);
        const excluir = await ServicoEvento.deletar(idsDeletar).catch(e =>
          erros(e)
        );
        if (excluir && excluir.status === 200) {
          const mensagemSucesso = `${
            eventosSelecionados.length > 1
              ? 'Eventos excluídos'
              : 'Evento excluído'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          validarFiltrar();
          setEventosSelecionados([]);
          filtrar();
        }
      }
    }
  };

  const onClickNovo = () => {
    const calendarioId = tipoCalendarioSelecionado;
    const calendarioSelecionado = listaCalendario?.find(
      t => t.id === calendarioId
    );
    setSessionStorage(calendarioSelecionado);
    history.push(`/calendario-escolar/eventos/novo/${calendarioId}`);
  };

  const onChangeNomeEvento = e => {
    setNomeEvento(e.target.value);
    filtrar('nomeEvento', e.target.value);
  };

  const onChangeTipoEvento = tipo => {
    setTipoEvento(tipo);
    filtrar('tipoEventoId', tipo);
  };

  const validaDataInicio = dataInicio => {
    setFiltroValido({ valido: false });

    const filtroAtual = filtro;

    filtroAtual.dataInicio = dataInicio && dataInicio.toDate();
    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataInicio && filtroAtual.dataFim) {
      validarFiltrar();
    }
  };

  const validaDataFim = dataFim => {
    setFiltroValido({ valido: false });

    const filtroAtual = filtro;
    filtroAtual.dataFim = dataFim && dataFim.toDate();

    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataInicio && filtroAtual.dataFim) {
      validarFiltrar();
    }
  };

  const onClickEditar = evento => {
    const calendarioId = tipoCalendarioSelecionado;
    const calendarioSelecionado = listaCalendario?.find(
      t => t.id === calendarioId
    );
    setSessionStorage(calendarioSelecionado);
    history.push(
      `/calendario-escolar/eventos/editar/${evento.id}/${filtro.tipoCalendarioId}`
    );
  };

  const [podeAlterarExcluir, setPodeAlterarExcluir] = useState(false);

  const onSelecionarItems = items => {
    setEventosSelecionados(items);
    setPodeAlterarExcluir(
      items.filter(
        item =>
          usuario.possuiPerfilSme === true ||
          (usuario.possuiPerfilDre === true && item.dreId && item.ueId) ||
          item.criadoRF === usuario.rf
      ).length
    );
  };

  const selecionaTipoCalendario = descricao => {
    const tipo = listaCalendario?.find(t => t.descricao === descricao);
    if (tipo?.id) {
      setSelecionouCalendario(true);
      filtrar('tipoCalendarioId', tipo.id);
      setBreadcrumbManual(
        `${match.url}/${tipo.id}`,
        '',
        '/calendario-escolar/eventos'
      );
    } else {
      setFiltroValido(false);
      setSelecionouCalendario(false);
      setDreSelecionada([]);
      setListaUe([]);
      setCampoUeDesabilitado(true);
      setTipoEvento('');
      setNomeEvento('');
      refForm.resetForm();
    }
    setValorTipoCalendario(descricao);
    setTipoCalendarioSelecionado(tipo?.id);
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      setPesquisaTipoCalendario(descricao);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      setCarregandoTipos(true);

      const {
        data,
      } = await ServicoCalendarios.obterTiposCalendarioAutoComplete(
        pesquisaTipoCalendario
      );

      if (isSubscribed) {
        setListaCalendario(data);
        setCarregandoTipos(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [pesquisaTipoCalendario, tipoCalendarioSelecionado]);

  return (
    <>
      {mensagemAlerta && (
        <Grid cols={12} className="mb-3">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'AlertaPrincipal',
              mensagem:
                'Para cadastrar ou listar eventos você precisa selecionar um tipo de calendário',
            }}
            className="mb-0"
          />
        </Grid>
      )}
      <Cabecalho pagina="Eventos do calendário escolar" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            id={shortid.generate()}
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
          <Button
            id={shortid.generate()}
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mr-2"
            onClick={onClickExcluir}
            disabled={
              !permissoesTela.podeExcluir ||
              !selecionouCalendario ||
              (eventosSelecionados && eventosSelecionados.length < 1) ||
              !podeAlterarExcluir
            }
          />
          <Button
            id={shortid.generate()}
            label="Novo"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickNovo}
            disabled={
              somenteConsulta ||
              !permissoesTela.podeIncluir ||
              !selecionouCalendario
            }
          />
        </div>

        <Formik
          ref={refFormik => setRefForm(refFormik)}
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={() => true}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row">
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
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
                      onSelect={selecionaTipoCalendario}
                      onChange={selecionaTipoCalendario}
                      handleSearch={handleSearch}
                      value={valorTipoCalendario}
                      form={form}
                    />
                  </Loader>
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
                  <SelectComponent
                    name="dreId"
                    id="select-dre"
                    lista={listaDre}
                    valueOption="codigo"
                    valueText="nome"
                    onChange={onChangeDreId}
                    disabled={dreDesabilitada}
                    placeholder="Selecione uma DRE (Opcional)"
                    form={form}
                  />
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
                  <SelectComponent
                    name="ueId"
                    id="select-ue"
                    lista={listaUe}
                    valueOption="codigo"
                    valueText="nome"
                    onChange={ueId => onChangeUeId(ueId)}
                    disabled={campoUeDesabilitado || ueDesabilitada}
                    placeholder="Selecione uma UE (Opcional)"
                    form={form}
                  />
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
                  <CampoTexto
                    placeholder="Digite o nome do evento"
                    onChange={onChangeNomeEvento}
                    value={nomeEvento}
                    desabilitado={!selecionouCalendario}
                  />
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
                  <SelectComponent
                    name="select-tipo-evento"
                    id="select-tipo-evento"
                    lista={listaTipoEvento}
                    valueOption="id"
                    valueText="descricao"
                    onChange={onChangeTipoEvento}
                    valueSelect={tipoEvento || undefined}
                    placeholder="Selecione um tipo"
                    disabled={!selecionouCalendario}
                  />
                </div>

                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2 pr-2">
                  <CampoData
                    formatoData="DD/MM/YYYY"
                    name="dataInicio"
                    onChange={data => validaDataInicio(data)}
                    placeholder="Data início"
                    form={form}
                    desabilitado={!selecionouCalendario}
                  />
                </div>
                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2 pl-2">
                  <CampoData
                    formatoData="DD/MM/YYYY"
                    name="dataFim"
                    onChange={data => validaDataFim(data)}
                    placeholder="Data fim"
                    form={form}
                    desabilitado={!selecionouCalendario}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        <div className="col-md-12 pt-2">
          {selecionouCalendario ? (
            <ListaPaginada
              url="v1/calendarios/eventos"
              id="lista-eventos"
              colunaChave="id"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
              filtroEhValido={filtroValido.valido}
            />
          ) : (
            ''
          )}
        </div>
      </Card>
    </>
  );
};

EventosLista.defaultProps = {
  match: {},
};

EventosLista.propTypes = {
  match: PropTypes.instanceOf(Object),
};

export default EventosLista;
