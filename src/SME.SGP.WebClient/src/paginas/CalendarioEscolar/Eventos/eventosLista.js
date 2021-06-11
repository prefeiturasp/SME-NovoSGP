import React, { useEffect, useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import { Form, Formik } from 'formik';
import * as moment from 'moment';
import { useSelector, useDispatch } from 'react-redux';
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

import { setFiltroCalendarioEscolar } from '~/redux/modulos/calendarioEscolar/actions';

const EventosLista = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.EVENTOS];

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [listaCalendario, setListaCalendario] = useState([]);
  const [listaDre, setListaDre] = useState([]);
  const [campoUeDesabilitado, setCampoUeDesabilitado] = useState(false);
  const [dreSelecionada, setDreSelecionada] = useState();
  const [ueSelecionada, setUeSelecionada] = useState();
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
      dataFim: momentSchema
        .test('validaFim', 'Data obrigatória', function validar() {
          const { dataInicio } = this.parent;
          const { dataFim } = this.parent;
          if (dataInicio && !dataFim) {
            return false;
          }
          return true;
        })
        .test('validaFim', 'Data inicial maior que final', function validar() {
          const { dataInicio, dataFim } = this.parent;
          if (dataInicio > dataFim) return false;
          return true;
        }),
    })
  );

  const { filtroCalendarioEscolar } = useSelector(
    state => state.calendarioEscolar.filtroCalendarioEscolar
  );

  const dispatch = useDispatch();
  const setFiltroCalendario = useCallback(
    value => dispatch(setFiltroCalendarioEscolar(value)),
    [dispatch]
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

  const selecionaTipoCalendario = useCallback(
    descricao => {
      const tipo = listaCalendario?.find(t => t.descricao === descricao);
      const filtroAtual = filtro;
      const dreId = filtroCalendarioEscolar?.dreId || dreSelecionada;
      const ueId = filtroCalendarioEscolar?.ueId || ueSelecionada;

      if (tipo?.id) {
        filtroAtual.dreId = dreId;
        filtroAtual.ueId = ueId;
        filtroAtual.ehTodasDres = false;
        filtroAtual.ehTodasUes = false;
        filtroAtual.tipoCalendarioId = tipo.id;

        setSelecionouCalendario(true);

        validarFiltrar();
        setFiltro({ ...filtroAtual });
        setBreadcrumbManual(
          `${match.url}/${tipo.id}`,
          '',
          '/calendario-escolar/eventos'
        );
      } else {
        setFiltroValido(false);
        setSelecionouCalendario(false);
        setTipoEvento('');
        setNomeEvento('');
        refForm.resetForm();
        setFiltro({});
      }
      refForm.setFieldValue('dreId', dreId);
      refForm.setFieldValue('ueId', ueId);
      setDreSelecionada(dreId);
      setUeSelecionada(ueId);
      setValorTipoCalendario(descricao);
      setTipoCalendarioSelecionado(tipo?.id);
      setFiltroCalendario({ ...filtroAtual });
    },
    [
      filtro,
      listaCalendario,
      match.url,
      refForm,
      validarFiltrar,
      setFiltroCalendario,
      filtroCalendarioEscolar,
      dreSelecionada,
      ueSelecionada,
    ]
  );

  useEffect(() => {
    if (
      refForm &&
      listaCalendario?.length &&
      filtroCalendarioEscolar?.eventoCalendarioId &&
      match?.params?.tipoCalendarioId
    ) {
      const { tipoCalendarioId } = match.params;
      const tipoCalendarioIdParseado = Number(tipoCalendarioId);
      const temTipoParaSetar = listaCalendario.find(
        item => item.id === tipoCalendarioIdParseado
      );

      const valorDescricao = temTipoParaSetar
        ? temTipoParaSetar.descricao
        : null;

      if (!temTipoParaSetar) {
        setPesquisaTipoCalendario(valorDescricao);
      }

      refForm.setFieldValue('tipoCalendarioId', valorDescricao);
      setValorTipoCalendario(valorDescricao);
      setSelecionouCalendario(true);
      selecionaTipoCalendario(valorDescricao, refForm);
      filtrar('tipoCalendarioId', tipoCalendarioIdParseado);
    }
  }, [
    match,
    filtro,
    listaCalendario,
    refForm,
    filtrar,
    selecionaTipoCalendario,
    filtroCalendarioEscolar,
    setFiltroCalendario,
  ]);

  useEffect(() => {
    if (!match?.params?.tipoCalendarioId) {
      setFiltroCalendario({});
    }
  }, [match, setFiltroCalendario]);

  const onChangeTipoEvento = tipo => {
    setTipoEvento(tipo);
    filtrar('tipoEventoId', tipo);
  };

  useEffect(() => {
    const obterListaEventos = async () => {
      const tiposEvento = await api.get('v1/calendarios/eventos/tipos/listar');

      if (tiposEvento?.data?.items) {
        if (tiposEvento.data.items.length === 1) {
          onChangeTipoEvento(String(tiposEvento.data.items[0].id));
        }
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
      const ueId = listaUe[0].codigo.toString();
      refForm.setFieldValue('ueId', ueId);
      setUeSelecionada(ueId);
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

      const modalidadeConvertida = !calendarioSelecionado?.modalidade
        ? 0
        : ServicoCalendarios.converterModalidade(
            calendarioSelecionado?.modalidade
          );
      const ues = await ServicoEvento.listarUes(
        dreSelecionada,
        modalidadeConvertida
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

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUeId = async (ueId, form) => {
    form.setFieldValue('ehTodasUes', ueId === '0');
    const filtroAtual = filtro;
    filtroAtual.ueId = ueId === '0' ? '' : ueId;
    filtroAtual.ehTodasUes = ueId === '0';
    setFiltro({ ...filtroAtual });
    setFiltroCalendario({ ...filtroAtual });
    validarFiltrar();
  };

  const onChangeDreId = async (dreId, form) => {
    const filtroAtual = filtro;
    filtroAtual.ehTodasUes = false;
    filtroAtual.ueId = undefined;
    filtroAtual.ehTodasUes = dreId === '0';
    filtroAtual.dreId = dreId === '0' ? '' : dreId;

    form.setFieldValue('ehTodasUes', false);
    form.setFieldValue('ueId', undefined);
    form.setFieldValue('ehTodasDres', dreId === '0');
    setFiltro({ ...filtroAtual });
    setFiltroCalendario({ ...filtroAtual });
    validarFiltrar();

    if (dreId) {
      setDreSelecionada(dreId);
      setCampoUeDesabilitado(false);
      return;
    }

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
    setFiltroCalendario({
      ...filtroCalendarioEscolar,
      eventoCalendarioId: true,
    });
    history.push(
      `/calendario-escolar/eventos/novo/${tipoCalendarioSelecionado}`
    );
  };

  const onChangeNomeEvento = e => {
    setNomeEvento(e.target.value);
    filtrar('nomeEvento', e.target.value);
  };

  const validaDataInicio = dataInicio => {
    setFiltroValido({ valido: false });

    const filtroAtual = filtro;

    filtroAtual.dataInicio =
      dataInicio && moment(dataInicio).format('MM-DD-YYYY');
    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataInicio && filtroAtual.dataFim) {
      validarFiltrar();
    }
  };

  const validaDataFim = dataFim => {
    setFiltroValido({ valido: false });

    const filtroAtual = filtro;
    filtroAtual.dataFim = dataFim && moment(dataFim).format('MM-DD-YYYY');

    setFiltro({ ...filtroAtual });

    if (filtroAtual.dataInicio && filtroAtual.dataFim) {
      validarFiltrar();
    }
  };

  const onClickEditar = evento => {
    setFiltroCalendario({
      ...filtroCalendarioEscolar,
      eventoCalendarioId: true,
    });
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
                      onSelect={valor => selecionaTipoCalendario(valor)}
                      onChange={valor => selecionaTipoCalendario(valor)}
                      handleSearch={handleSearch}
                      value={valorTipoCalendario}
                      form={form}
                      allowClear={false}
                    />
                  </Loader>
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 pb-2">
                  <SelectComponent
                    name="dreId"
                    id="select-ue"
                    lista={listaDre}
                    valueOption="codigo"
                    valueText="nome"
                    onChange={valor => onChangeDreId(valor, form)}
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
                    onChange={ueId => onChangeUeId(ueId, form)}
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
                    disabled={
                      !selecionouCalendario || listaTipoEvento?.length === 1
                    }
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
