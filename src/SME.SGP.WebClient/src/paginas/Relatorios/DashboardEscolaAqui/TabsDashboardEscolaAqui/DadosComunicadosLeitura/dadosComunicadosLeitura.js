import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import {
  CampoData,
  Loader,
  SelectAutocomplete,
  SelectComponent,
} from '~/componentes';
import modalidade from '~/dtos/modalidade';
import { AbrangenciaServico, api, erros } from '~/servicos';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';

const DadosComunicadosLeitura = props => {
  const { codigoUe } = props;

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

  const [carrecandoComunicado, setCarrecandoComunicado] = useState(false);
  const [listaComunicado, setListaComunicado] = useState([]);
  const [comunicado, setComunicado] = useState();
  const [pesquisaComunicado, setPesquisaComunicado] = useState('');

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

  const obterAnosLetivos = useCallback(async () => {
    setExibirLoader(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos()
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (anosLetivo?.data?.length) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

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
      const resposta = await api
        .get(`/v1/ues/${ue}/modalidades?ano=${ano}`)
        .catch(e => erros(e))
        .finally(() => setCarregandoModalidades(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.id),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      } else {
        setListaModalidades([]);
      }
    }
  };

  useEffect(() => {
    setModalidadeId();
    if (anoLetivo && codigoUe && codigoUe !== OPCAO_TODOS) {
      obterModalidades(codigoUe, anoLetivo);
    } else {
      setListaModalidades([]);
    }
  }, [anoLetivo, codigoUe]);

  const obterAnosEscolares = useCallback(async (mod, ue) => {
    setCarregandoAnosEscolares(true);
    const respota = await ServicoFiltroRelatorio.obterAnosEscolares(ue, mod)
      .catch(e => erros(e))
      .finally(() => setCarregandoAnosEscolares(false));

    if (respota?.data?.length) {
      setListaAnosEscolares(respota.data);

      if (respota.data && respota.data.length && respota.data.length === 1) {
        setAnosEscolares(respota.data[0].valor);
      }
    } else {
      setListaAnosEscolares([]);
    }
  }, []);

  useEffect(() => {
    if (modalidadeId && codigoUe) {
      obterAnosEscolares(modalidadeId, codigoUe);
    } else {
      setAnosEscolares(undefined);
      setListaAnosEscolares([]);
    }
  }, [modalidadeId, obterAnosEscolares]);

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    setCarregandoSemestres(true);
    const retorno = await api.get(
      `v1/abrangencias/false/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
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
  };

  useEffect(() => {
    if (
      modalidadeId &&
      anoLetivo &&
      String(modalidadeId) === String(modalidade.EJA)
    ) {
      obterSemestres(modalidadeId, anoLetivo);
    } else {
      setSemestre();
      setListaSemestres([]);
    }
  }, [modalidadeId, anoLetivo]);

  const desabilitarData = current => {
    if (current) {
      return (
        current < window.moment().startOf('year') ||
        current > window.moment().endOf('year')
      );
    }
    return false;
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      setPesquisaComunicado(descricao);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      setCarrecandoComunicado(true);

      const resposta = await ServicoDashboardEscolaAqui.obterComunicadosAutoComplete(
        pesquisaComunicado
      )
        .catch(e => erros(e))
        .finally(() => setCarrecandoComunicado(false));

      if (isSubscribed) {
        if (resposta?.data?.length) {
          setListaComunicado(resposta.data);
        } else {
          setListaComunicado([]);
        }
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [pesquisaComunicado]);

  return (
    <Loader loading={exibirLoader}>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
          <SelectComponent
            id="select-ano-letivo"
            label="Ano Letivo"
            lista={listaAnosLetivo}
            valueOption="valor"
            valueText="desc"
            disabled={listaAnosLetivo?.length === 1}
            onChange={setAnoLetivo}
            valueSelect={anoLetivo}
            placeholder="Selecione o ano"
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
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3  mb-2">
          <Loader loading={carregandoModalidades} tip="">
            <SelectComponent
              id="select-modalidade"
              label="Modalidade"
              lista={listaModalidades}
              valueOption="valor"
              valueText="desc"
              onChange={setModalidadeId}
              valueSelect={modalidadeId}
              placeholder="Modalidade"
              disabled={
                codigoUe === OPCAO_TODOS || listaModalidades?.length === 1
              }
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
                String(modalidadeId) !== String(modalidade.EJA)
              }
              valueSelect={semestre}
              onChange={setSemestre}
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
              disabled={
                !modalidadeId ||
                codigoUe === OPCAO_TODOS ||
                listaAnosEscolares?.length === 1
              }
              valueSelect={anosEscolares}
              onChange={setAnosEscolares}
              placeholder="Selecione o ano"
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
          <Loader loading={carrecandoComunicado} tip="">
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
          />
        </div>
      </div>
    </Loader>
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
