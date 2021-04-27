import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import {
  CampoData,
  CheckboxComponent,
  Loader,
  SelectAutocomplete,
  SelectComponent,
} from '~/componentes';
import { FiltroHelper } from '~/componentes-sgp';
import Alert from '~/componentes/alert';
import { ModalidadeDTO } from '~/dtos';
import {
  limparDadosDashboardEscolaAqui,
  setDadosDeLeituraDeComunicadosAgrupadosPorModalidade,
} from '~/redux/modulos/dashboardEscolaAqui/actions';
import { AbrangenciaServico, api, erros } from '~/servicos';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoGraficoPizzaComValorEPercentual,
  obterDadosComunicadoSelecionado,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import DataUltimaAtualizacaoDashboardEscolaAqui from '../ComponentesDashboardEscolaAqui/dataUltimaAtualizacaoDashboardEscolaAqui';
import GraficoPizzaDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoPizzaDashboard';
import LeituraDeComunicadosAgrupadosPorDre from './leituraDeComunicadosAgrupadosPorDre';
import LeituraDeComunicadosPorAlunos from './leituraDeComunicadosPorAlunos';
import LeituraDeComunicadosPorModalidades from './leituraDeComunicadosPorModalidades';
import LeituraDeComunicadosPorModalidadesETurmas from './leituraDeComunicadosPorModalidadesETurmas';
import LeituraDeComunicadosPorTurmas from './leituraDeComunicadosPorTurmas';
import FiltroHelperComunicados from '~/paginas/AcompanhamentoEscolar/Comunicados/Helper/helper';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';
import { ordenarListaMaiorParaMenor } from '~/utils/funcoes/gerais';

const DadosComunicadosLeitura = props => {
  const { codigoDre, codigoUe } = props;

  const dispatch = useDispatch();

  const [exibirLoader, setExibirLoader] = useState(false);

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [anoLetivo, setAnoLetivo] = useState();

  const [carregandoModalidades, setCarregandoModalidades] = useState(false);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [modalidadeId, setModalidadeId] = useState();

  const [carregandoSemestres, setCarregandoSemestres] = useState(false);
  const [listaSemestres, setListaSemestres] = useState([]);
  const [semestre, setSemestre] = useState();

  const [carregandoAnosEscolares, setCarregandoAnosEscolares] = useState(false);
  const [listaAnosEscolares, setListaAnosEscolares] = useState([]);
  const [anosEscolares, setAnosEscolares] = useState(undefined);

  const [listaGrupo, setListaGrupo] = useState([]);
  const [grupo, setGrupo] = useState();

  const [dataInicio, setDataInicio] = useState();
  const [dataFim, setDataFim] = useState();

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [codigoTurma, setCodigoTurma] = useState();
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaTurmasOriginal, setListaTurmasOriginal] = useState([]);

  const [carregandoComunicados, setCarregandoComunicados] = useState(false);
  const [listaComunicado, setListaComunicado] = useState([]);
  const [comunicado, setComunicado] = useState();
  const [pesquisaComunicado, setPesquisaComunicado] = useState('');

  const [
    dadosDeLeituraDeComunicados,
    setDadosDeLeituraDeComunicados,
  ] = useState([]);

  const [timeoutCampoPesquisa, setTimeoutCampoPesquisa] = useState();

  const [consideraHistorico, setConsideraHistorico] = useState(false);

  // TODO Verificar no componente de gráficos outra forma de fazer!
  const chavesGrafico = [
    'Usuários que não receberam o comunicado (CPF válido porém que não tem o APP instalado)',
    'Usuário que receberam o comunicado e ainda não visualizaram',
    'Visualizaram o comunicado',
  ];

  const [listaVisualizacao] = useState([
    {
      valor: '1',
      descricao: 'Responsáveis',
    },
    {
      valor: '2',
      descricao: 'Estudantes',
    },
  ]);
  const [visualizacao, setVisualizacao] = useState('1');

  const OPCAO_TODOS = '-99';

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const obterAnosLetivos = useCallback(async () => {
    setExibirLoader(true);
    let anosLetivos = [];

    const anosLetivoComHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: true,
    });
    const anosLetivoSemHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: false,
    });

    anosLetivos = anosLetivos.concat(anosLetivoComHistorico);

    anosLetivoSemHistorico.forEach(ano => {
      if (!anosLetivoComHistorico.find(a => a.valor === ano.valor)) {
        anosLetivos.push(ano);
      }
    });

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    if (anosLetivos && anosLetivos.length) {
      const temAnoAtualNaLista = anosLetivos.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) setAnoLetivo(anoAtual);
      else setAnoLetivo(anosLetivos[0].valor);
    }
    setListaAnosLetivo(ordenarListaMaiorParaMenor(anosLetivos, 'valor'));
    setExibirLoader(false);
  }, [anoAtual]);

  const obterListaGrupos = async () => {
    const resposta = await api
      .get('v1/comunicacao/grupos/listar')
      .catch(e => erros(e));

    if (resposta?.data?.length) {
      const lista = resposta.data.map(g => {
        return {
          valor: g.id,
          desc: g.nome,
        };
      });

      if (lista.length > 1) {
        lista.unshift({ valor: OPCAO_TODOS, desc: 'Todos' });
      }
      if (lista?.length === 1) {
        setGrupo([lista[0].valor]);
      }

      setListaGrupo(lista);
    } else {
      setListaGrupo([]);
    }
  };

  useEffect(() => {
    obterAnosLetivos();
    obterListaGrupos();
  }, [obterAnosLetivos]);

  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      setCarregandoModalidades(true);
      const resposta = await ServicoFiltroRelatorio.obterModalidadesPorAbrangencia(
        ue,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoModalidades(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.descricao,
          valor: String(item.valor),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
    }
  };

  useEffect(() => {
    setModalidadeId();
    setListaModalidades([]);
    setGrupo([]);
    if (anoLetivo && codigoUe) {
      obterModalidades(codigoUe, anoLetivo);
    }
  }, [anoLetivo, codigoUe]);

  const obterAnosEscolaresPorModalidade = useCallback(async () => {
    const resposta = await ServicoComunicados.buscarAnosPorModalidade(
      modalidadeId,
      codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setCarregandoAnosEscolares(false));

    if (resposta.data?.length) {
      const dadosMap = resposta.data.map(item => {
        return {
          valor: item.ano,
          descricao: item.ano,
        };
      });
      setListaAnosEscolares(dadosMap);
    } else {
      setListaAnosEscolares([]);
    }
  }, [modalidadeId, codigoUe]);

  useEffect(() => {
    if (
      modalidadeId &&
      (modalidadeId == ModalidadeDTO.ENSINO_MEDIO ||
        modalidadeId == ModalidadeDTO.FUNDAMENTAL)
    ) {
      obterAnosEscolaresPorModalidade();
    } else {
      setAnosEscolares();
      setListaAnosEscolares([]);
    }
  }, [modalidadeId]);

  const obterSemestres = useCallback(async () => {
    setCarregandoSemestres(true);
    const retorno = await api.get(
      `v1/abrangencias/${consideraHistorico}/semestres?anoLetivo=${anoLetivo}&modalidade=${modalidadeId ||
        0}`
    );
    if (retorno && retorno.data) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista && lista.length && lista.length === 1) {
        setSemestre(lista[0].valor);
      }
      setListaSemestres(lista);
    }
    setCarregandoSemestres(false);
  }, [modalidadeId, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (
      modalidadeId &&
      anoLetivo &&
      String(modalidadeId) === String(ModalidadeDTO.EJA)
    ) {
      obterSemestres();
    } else {
      setSemestre();
      setListaSemestres([]);
    }
  }, [modalidadeId, anoLetivo, consideraHistorico, obterSemestres]);

  const obterTurmas = useCallback(async () => {
    if (codigoDre && codigoUe && modalidadeId) {
      setCarregandoTurmas(true);
      const resultado = await AbrangenciaServico.buscarTurmas(
        codigoUe,
        modalidadeId,
        '',
        anoLetivo,
        consideraHistorico
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTurmas(false));

      if (resultado?.data) {
        const turmas = [];

        resultado.data.map(item =>
          turmas.push({
            desc: item.nome,
            valor: item.codigo,
            id: item.id,
            ano: item.ano,
          })
        );

        setListaTurmas(turmas);
        setListaTurmasOriginal(turmas);
        if (turmas.length === 1) {
          setCodigoTurma(turmas[0].valor);
        }
      }
      setCarregandoTurmas(false);
    }
  }, [modalidadeId]);

  const obterGruposIdPorModalidade = async mod => {
    if (!mod) return;

    const dados = await FiltroHelperComunicados.ObterGruposIdPorModalidade(mod);

    if (dados?.length === 0) return;

    setGrupo(dados);
  };

  useEffect(() => {
    if (modalidadeId) {
      obterGruposIdPorModalidade(modalidadeId);
    }
  }, [modalidadeId]);

  const filterTurmasAnoSelecionado = useCallback(() => {
    if (anosEscolares === OPCAO_TODOS) {
      setListaTurmas(listaTurmasOriginal);
    } else {
      const turmas = listaTurmasOriginal.filter(a => a.ano === anosEscolares);
      setListaTurmas(turmas);
    }
  }, [anosEscolares, listaTurmasOriginal]);

  useEffect(() => {
    setCodigoTurma();
    if (anosEscolares) {
      filterTurmasAnoSelecionado();
    } else {
      setListaTurmas(listaTurmasOriginal);
    }
  }, [anosEscolares, listaTurmasOriginal]);

  useEffect(() => {
    if (modalidadeId && codigoUe && codigoDre) {
      obterTurmas();
    } else {
      setCodigoTurma();
      setListaTurmas([]);
      setListaTurmasOriginal([]);
    }
  }, [modalidadeId]);

  useEffect(() => {
    if (codigoUe === OPCAO_TODOS) {
      setCodigoTurma();
      setListaTurmas([]);
    }
  }, [codigoUe]);

  const desabilitarData = current => {
    if (current && anoLetivo) {
      const ano = moment(`${anoLetivo}-01-01`);
      return current < ano.startOf('year') || current > ano.endOf('year');
    }
    return false;
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      if (timeoutCampoPesquisa) {
        clearTimeout(timeoutCampoPesquisa);
      }
      const timeout = setTimeout(() => {
        setPesquisaComunicado(descricao);
      }, 500);
      setTimeoutCampoPesquisa(timeout);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      if (isSubscribed && anoLetivo && codigoDre && codigoUe) {
        if (
          modalidadeId &&
          String(modalidadeId) === String(ModalidadeDTO.EJA) &&
          !semestre
        ) {
          return;
        }

        const todosGrupos =
          grupo && grupo[0] === OPCAO_TODOS
            ? listaGrupo
                .filter(item => item.valor !== OPCAO_TODOS)
                .map(g => g.valor)
            : grupo;

        setCarregandoComunicados(true);
        const resposta = await ServicoDashboardEscolaAqui.obterComunicadosAutoComplete(
          anoLetivo || '',
          codigoDre === OPCAO_TODOS ? '' : codigoDre || '',
          codigoUe === OPCAO_TODOS ? '' : codigoUe || '',
          todosGrupos,
          '',
          semestre || '',
          anosEscolares || '',
          codigoTurma === OPCAO_TODOS ? '' : codigoTurma || '',
          dataInicio || '',
          dataFim || '',
          pesquisaComunicado || ''
        )
          .catch(e => erros(e))
          .finally(() => setCarregandoComunicados(false));

        if (resposta?.data?.length) {
          const lista = resposta.data.map(item => {
            return {
              ...item,
              descricao: `${item.id} - ${item.titulo} - ${moment(
                item.dataEnvio
              ).format('DD/MM/YYYY')}`,
            };
          });
          setListaComunicado([]);
          setListaComunicado(lista);
        } else {
          setListaComunicado([]);
        }
        if (!pesquisaComunicado) {
          setComunicado();
        }
      } else {
        setListaComunicado([]);
        setComunicado();
        setPesquisaComunicado();
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [
    anoLetivo,
    codigoDre,
    codigoUe,
    grupo,
    modalidadeId,
    semestre,
    anosEscolares,
    codigoTurma,
    dataInicio,
    dataFim,
    pesquisaComunicado,
    listaGrupo,
  ]);

  const mapearParaDtoGraficoPizza = dados => {
    const dadosParaMapear = [];

    if (dados.naoReceberamComunicado) {
      const naoReceberamComunicado = {
        label: chavesGrafico[0],
        value: dados.naoReceberamComunicado || 0,
      };
      dadosParaMapear.push(naoReceberamComunicado);
    }

    if (dados.receberamENaoVisualizaram) {
      const receberamENaoVisualizaram = {
        label: chavesGrafico[1],
        value: dados.receberamENaoVisualizaram || 0,
      };
      dadosParaMapear.push(receberamENaoVisualizaram);
    }

    if (dados.visualizaramComunicado) {
      const visualizaramComunicado = {
        label: chavesGrafico[2],
        value: dados.visualizaramComunicado || 0,
      };
      dadosParaMapear.push(visualizaramComunicado);
    }

    const dadosMapeados = mapearParaDtoGraficoPizzaComValorEPercentual(
      dadosParaMapear
    );
    return dadosMapeados;
  };

  const obterDadosDeLeituraDeComunicados = useCallback(async () => {
    const dadosComunicado = obterDadosComunicadoSelecionado(
      comunicado,
      listaComunicado
    );
    if (dadosComunicado?.id) {
      setExibirLoader(true);

      dispatch(setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]));
      const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicados(
        dadosComunicado.codigoDre || '',
        dadosComunicado.codigoUe || '',
        dadosComunicado.id,
        dadosComunicado.agruparModalidade,
        visualizacao
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data) {
        const dados = mapearParaDtoGraficoPizza(resposta.data[0]);
        setDadosDeLeituraDeComunicados(dados);
      } else {
        setDadosDeLeituraDeComunicados([]);
      }
    } else {
      setDadosDeLeituraDeComunicados([]);
    }
  }, [visualizacao, comunicado, listaComunicado]);

  useEffect(() => {
    if (visualizacao && comunicado && listaComunicado.length) {
      obterDadosDeLeituraDeComunicados();
    }
  }, [
    comunicado,
    visualizacao,
    listaComunicado,
    obterDadosDeLeituraDeComunicados,
  ]);

  useEffect(() => {
    if (!comunicado) {
      setDadosDeLeituraDeComunicados([]);
    }
    dispatch(limparDadosDashboardEscolaAqui());
  }, [comunicado, dispatch]);

  useEffect(() => {
    setDadosDeLeituraDeComunicados([]);
  }, [codigoUe]);

  useEffect(() => {
    dispatch(limparDadosDashboardEscolaAqui([]));
    return () => {
      dispatch(limparDadosDashboardEscolaAqui([]));
    };
  }, [dispatch]);

  useEffect(() => {
    setAnoLetivo(anoAtual);
  }, [consideraHistorico, anoAtual]);

  const onChangeModalidade = valor => {
    setCodigoTurma();
    setModalidadeId(valor);
    setGrupo([]);
  };

  const onChangeSemestre = valor => {
    setSemestre(valor);
  };

  const onChangeTurma = valor => {
    setCodigoTurma(valor);
  };

  const onChangeAnoLetivo = async valor => {
    setGrupo();
    setModalidadeId();
    setCodigoTurma();
    setAnoLetivo(valor);
  };

  return (
    <>
      {!comunicado ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'selecionar-comunicado',
            mensagem:
              'Você precisa selecionar um comunicado para visualizar os dados de leitura',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      <Loader loading={exibirLoader}>
        <div className="row">
          <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
            <CheckboxComponent
              label="Exibir histórico?"
              onChangeCheckbox={e => {
                setAnoLetivo();
                setGrupo();
                setDataFim();
                setDataInicio();
                setConsideraHistorico(e.target.checked);
              }}
              checked={consideraHistorico}
            />
          </div>
        </div>
        <div className="row">
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
            <SelectComponent
              id="select-ano-letivo"
              label="Ano Letivo"
              lista={listaAnosLetivo}
              valueOption="valor"
              valueText="desc"
              disabled={!consideraHistorico || listaAnosLetivo?.length === 1}
              onChange={onChangeAnoLetivo}
              valueSelect={anoLetivo}
              placeholder="Selecione o ano"
              allowClear={false}
            />
          </div>
          <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
            <SelectComponent
              id="select-grupo"
              label="Grupo"
              lista={listaGrupo}
              valueOption="valor"
              valueText="desc"
              valueSelect={grupo}
              placeholder="Selecione o grupo"
              multiple
              onChange={valores => {
                const opcaoTodosJaSelecionado = grupo
                  ? grupo.includes(OPCAO_TODOS)
                  : false;
                if (opcaoTodosJaSelecionado) {
                  const listaSemOpcaoTodos = valores.filter(
                    v => v !== OPCAO_TODOS
                  );
                  setGrupo(listaSemOpcaoTodos);
                } else if (valores.includes(OPCAO_TODOS)) {
                  setGrupo([OPCAO_TODOS]);
                } else {
                  setGrupo(valores);
                }
              }}
              disabled={modalidadeId}
            />
          </div>
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3  mb-2">
            <Loader loading={carregandoModalidades} tip="">
              <SelectComponent
                id="drop-modalidade"
                label="Modalidade"
                lista={listaModalidades}
                valueOption="valor"
                valueText="desc"
                disabled={!codigoUe || listaModalidades?.length === 1}
                onChange={onChangeModalidade}
                valueSelect={modalidadeId}
                placeholder="Modalidade"
              />
            </Loader>
          </div>
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
            <Loader loading={carregandoSemestres} tip="">
              <SelectComponent
                id="select-semestre"
                lista={listaSemestres}
                valueOption="valor"
                valueText="desc"
                label="Semestre"
                disabled={
                  !modalidadeId ||
                  (listaSemestres && listaSemestres.length === 1) ||
                  String(modalidadeId) !== String(ModalidadeDTO.EJA)
                }
                valueSelect={semestre}
                onChange={onChangeSemestre}
                placeholder="Semestre"
              />
            </Loader>
          </div>
          <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
            <Loader loading={carregandoAnosEscolares} tip="">
              <SelectComponent
                id="select-ano-escolar"
                lista={listaAnosEscolares}
                valueOption="valor"
                valueText="descricao"
                label="Ano"
                disabled={!modalidadeId || listaAnosEscolares?.length === 1}
                valueSelect={anosEscolares}
                onChange={setAnosEscolares}
                placeholder="Selecione o ano"
              />
            </Loader>
          </div>
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
            <Loader loading={carregandoTurmas} tip="">
              <SelectComponent
                id="select-turma"
                lista={listaTurmas}
                valueOption="valor"
                valueText="desc"
                label="Turma"
                disabled={
                  codigoUe === OPCAO_TODOS ||
                  !modalidadeId ||
                  (listaTurmas && listaTurmas.length === 1)
                }
                valueSelect={codigoTurma}
                onChange={onChangeTurma}
                placeholder="Turma"
              />
            </Loader>
          </div>

          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
            <CampoData
              if="data-inicio"
              label="Data de envio início"
              placeholder="DD/MM/AAAA"
              formatoData="DD/MM/YYYY"
              onChange={setDataInicio}
              desabilitarData={desabilitarData}
              valor={dataInicio}
            />
          </div>
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 pb-2">
            <CampoData
              id="data-fim"
              label="Data de envio fim"
              placeholder="DD/MM/AAAA"
              formatoData="DD/MM/YYYY"
              onChange={setDataFim}
              desabilitarData={desabilitarData}
              valor={dataFim}
            />
          </div>
          <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
            <Loader loading={carregandoComunicados} tip="">
              <SelectAutocomplete
                id="autocomplete-comunicados"
                label="Comunicado"
                showList
                isHandleSearch
                placeholder="Selecione um comunicado"
                className="col-md-12"
                lista={listaComunicado}
                valueField="id"
                textField="descricao"
                onSelect={setComunicado}
                onChange={setComunicado}
                handleSearch={handleSearch}
                value={comunicado}
              />
            </Loader>
          </div>
          <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
            <SelectComponent
              lista={listaVisualizacao}
              valueOption="valor"
              valueText="descricao"
              label="Visualização"
              valueSelect={visualizacao}
              onChange={setVisualizacao}
              placeholder="Selecione a visualização"
              allowClear={false}
            />
          </div>
          {dadosDeLeituraDeComunicados?.length ? (
            <div className="col-md-12 mt-5">
              <DataUltimaAtualizacaoDashboardEscolaAqui
                nomeConsulta="ConsolidarLeituraNotificacao"
                tituloAdicional="Os dados estão considerando a situação atual dos estudantes e responsáveis"
              />
            </div>
          ) : (
            ''
          )}

          <div className="col-md-12">
            {dadosDeLeituraDeComunicados?.length ? (
              <GraficoPizzaDashboard
                titulo="Dados de leitura"
                dadosGrafico={dadosDeLeituraDeComunicados}
              />
            ) : (
              ''
            )}
          </div>
          {dadosDeLeituraDeComunicados?.length ? (
            <LeituraDeComunicadosAgrupadosPorDre
              chavesGrafico={chavesGrafico}
              modoVisualizacao={visualizacao}
              comunicado={comunicado}
              listaComunicado={listaComunicado}
            />
          ) : (
            ''
          )}
          {dadosDeLeituraDeComunicados?.length ? (
            <LeituraDeComunicadosPorModalidades
              chavesGrafico={chavesGrafico}
              modoVisualizacao={visualizacao}
              comunicado={comunicado}
              listaComunicado={listaComunicado}
            />
          ) : (
            ''
          )}
          {dadosDeLeituraDeComunicados?.length ? (
            <LeituraDeComunicadosPorModalidadesETurmas
              chavesGrafico={chavesGrafico}
              modoVisualizacao={visualizacao}
              comunicado={comunicado}
              listaComunicado={listaComunicado}
            />
          ) : (
            ''
          )}
          {dadosDeLeituraDeComunicados?.length ? (
            <LeituraDeComunicadosPorTurmas
              chavesGrafico={chavesGrafico}
              modoVisualizacao={visualizacao}
              comunicado={comunicado}
              listaComunicado={listaComunicado}
            />
          ) : (
            ''
          )}

          {dadosDeLeituraDeComunicados?.length && comunicado ? (
            <LeituraDeComunicadosPorAlunos
              comunicado={comunicado}
              listaComunicado={listaComunicado}
            />
          ) : (
            ''
          )}
        </div>
      </Loader>
    </>
  );
};

DadosComunicadosLeitura.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosComunicadosLeitura.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosComunicadosLeitura;
