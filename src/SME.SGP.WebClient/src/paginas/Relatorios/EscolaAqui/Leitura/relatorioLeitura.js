import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import {
  CampoData,
  CheckboxComponent,
  Loader,
  RadioGroupButton,
  SelectAutocomplete,
  SelectComponent,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { ModalidadeDTO } from '~/dtos';
import modalidade from '~/dtos/modalidade';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import ServicoHistoricoAlteracoesNotas from '~/servicos/Paginas/Relatorios/Fechamento/HistoricoAlteracoesNotas/ServicoHistoricoAlteracoesNotas';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const RelatorioLeitura = () => {
  const [exibirLoader, setExibirLoader] = useState(false);

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaGrupos, setListaGrupos] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaSemestres, setListaSemestres] = useState([]);
  const [listaAnosEscolares, setListaAnosEscolares] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaComunicado, setListaComunicado] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState();
  const [codigoDre, setCodigoDre] = useState();
  const [codigoUe, setCodigoUe] = useState();
  const [grupos, setGrupos] = useState();
  const [modalidadeId, setModalidadeId] = useState();
  const [semestre, setSemestre] = useState();
  const [anosEscolares, setAnosEscolares] = useState();
  const [turmaId, setTurmaId] = useState();
  const [dataInicio, setDataInicio] = useState();
  const [dataFim, setDataFim] = useState();
  const [comunicado, setComunicado] = useState();
  const [pesquisaComunicado, setPesquisaComunicado] = useState('');
  const [
    listarResponsaveisEstudantes,
    setListarResponsaveisEstudantes,
  ] = useState(false);
  const [listarComunicadosExpirados, setListarComunicadosExpirados] = useState(
    false
  );

  const [consideraHistorico, setConsideraHistorico] = useState(false);

  const OPCAO_TODOS = '-99';

  const desabilitarGerar = !anoLetivo || !codigoDre || !codigoUe;

  const opcoesRadioSimNao = [
    { label: 'Não', value: false },
    { label: 'Sim', value: true },
  ];

  const onChangeAnoLetivo = async valor => {
    setCodigoDre();
    setCodigoUe();
    setModalidadeId();
    setTurmaId();
    setAnoLetivo(valor);
  };

  const onChangeDre = valor => {
    setCodigoDre(valor);
    setCodigoUe();
    setModalidadeId();
    setTurmaId();

    setCodigoUe(undefined);
  };

  const onChangeUe = valor => {
    setModalidadeId();
    setTurmaId();
    setCodigoUe(valor);
  };

  const onChangeModalidade = valor => {
    setTurmaId();
    setModalidadeId(valor);
  };

  const onChangeSemestre = valor => {
    setSemestre(valor);
  };

  const onChangeTurma = valor => {
    setTurmaId(valor);
  };

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setExibirLoader(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivo}`,
        consideraHistorico
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
          abrev: item.abreviacao,
        }));

        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setCodigoDre(lista[0].valor);
        }
      } else {
        setListaDres([]);
        setCodigoDre(undefined);
      }
    }
  }, [anoLetivo, consideraHistorico]);

  useEffect(() => {
    obterDres();
  }, [obterDres, anoLetivo, consideraHistorico]);

  useEffect(() => {
    setAnoLetivo(anoAtual);
  }, [consideraHistorico, anoAtual]);

  const obterUes = useCallback(async () => {
    if (codigoDre) {
      setExibirLoader(true);
      const resposta = await AbrangenciaServico.buscarUes(
        codigoDre,
        `v1/abrangencias/${consideraHistorico}/dres/${codigoDre}/ues?anoLetivo=${anoLetivo}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista && lista.length && lista.length === 1) {
          setCodigoUe(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, [consideraHistorico, anoLetivo, codigoDre]);

  useEffect(() => {
    if (codigoDre) {
      obterUes();
    } else {
      setCodigoUe();
      setListaUes([]);
    }
  }, [codigoDre, anoLetivo, consideraHistorico, obterUes]);

  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      setExibirLoader(true);
      const {
        data,
      } = await ServicoFiltroRelatorio.obterModalidadesPorAbrangencia(ue);

      if (data) {
        const lista = data.map(item => ({
          desc: item.descricao,
          valor: String(item.valor),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
      setExibirLoader(false);
    }
  };

  useEffect(() => {
    if (anoLetivo && codigoUe) {
      obterModalidades(codigoUe, anoLetivo);
    } else {
      setModalidadeId();
      setListaModalidades([]);
    }
  }, [anoLetivo, codigoUe]);

  const obterTurmas = useCallback(async () => {
    if (codigoDre && codigoUe && modalidadeId) {
      setExibirLoader(true);
      const { data } = await AbrangenciaServico.buscarTurmas(
        codigoUe,
        modalidadeId,
        '',
        anoLetivo,
        consideraHistorico
      );
      if (data) {
        const lista = [];

        data.map(item =>
          lista.push({
            desc: item.nome,
            valor: item.codigo,
            id: item.id,
            ano: item.ano,
          })
        );
        setListaTurmas(lista);
        if (lista.length === 1) {
          setTurmaId(lista[0].valor);
        }
      }
      setExibirLoader(false);
    }
  }, [modalidadeId]);

  useEffect(() => {
    if (modalidadeId && codigoUe && codigoDre) {
      obterTurmas();
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [modalidadeId]);

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

    setListaAnosLetivo(anosLetivos);
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
        setGrupos([lista[0].valor]);
      }

      setListaGrupos(lista);
    } else {
      setListaGrupos([]);
    }
  };

  useEffect(() => {
    obterAnosLetivos();
    obterListaGrupos();
  }, [obterAnosLetivos]);

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    setExibirLoader(true);
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
    setExibirLoader(false);
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
  }, [obterAnosLetivos, modalidadeId, anoLetivo]);

  const obterAnosEscolares = useCallback(async () => {
    setExibirLoader(true);
    const respota = await AbrangenciaServico.buscarAnosEscolares(
      codigoUe,
      modalidadeId,
      consideraHistorico
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (respota?.data?.length) {
      setListaAnosEscolares(respota.data);

      if (respota?.data?.length === 1) {
        setAnosEscolares(respota.data[0].valor);
      }
    } else {
      setListaAnosEscolares([]);
    }
  }, [codigoUe, modalidadeId, consideraHistorico]);

  useEffect(() => {
    if (modalidadeId && codigoUe && codigoUe !== OPCAO_TODOS) {
      obterAnosEscolares();
    } else {
      setAnosEscolares();
      setListaAnosEscolares([]);
    }
  }, [modalidadeId]);

  const cancelar = async () => {
    await setCodigoDre();
    await setCodigoUe();
    await setModalidadeId();
    await setTurmaId();
    await setAnoLetivo();
    await setAnoLetivo(anoAtual);
  };

  const gerar = async () => {
    const params = {
      codigoDre,
      codigoUe,
      anoLetivo,
      modalidadeId,
      semestre,
      turmaId,
    };

    setExibirLoader(true);
    const retorno = await ServicoHistoricoAlteracoesNotas.gerar(params)
      .catch(e => erros(e))
      .finally(setExibirLoader(false));
    if (retorno && retorno.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
    }
  };

  const onchangeMultiSelect = (valores, valoreAtual, funSetarNovoValor) => {
    const opcaoTodosJaSelecionado = valoreAtual
      ? valoreAtual.includes(OPCAO_TODOS)
      : false;
    if (opcaoTodosJaSelecionado) {
      const listaSemOpcaoTodos = valores.filter(v => v !== OPCAO_TODOS);
      funSetarNovoValor(listaSemOpcaoTodos);
    } else if (valores.includes(OPCAO_TODOS)) {
      funSetarNovoValor([OPCAO_TODOS]);
    } else {
      funSetarNovoValor(valores);
    }
  };

  const desabilitarData = current => {
    if (current) {
      return (
        current < moment().startOf('year') || current > moment().endOf('year')
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
      if (isSubscribed && anoLetivo && codigoDre && codigoUe) {
        if (
          modalidadeId &&
          String(modalidadeId) === String(ModalidadeDTO.EJA) &&
          !semestre
        ) {
          return;
        }

        const todosGrupos =
          grupos && grupos[0] === OPCAO_TODOS
            ? listaGrupos
                .filter(item => item.valor !== OPCAO_TODOS)
                .map(g => g.valor)
            : grupos;

        setExibirLoader(true);
        setComunicado();
        const resposta = await ServicoDashboardEscolaAqui.obterComunicadosAutoComplete(
          anoLetivo || '',
          codigoDre === OPCAO_TODOS ? '' : codigoDre || '',
          codigoUe === OPCAO_TODOS ? '' : codigoUe || '',
          todosGrupos,
          modalidadeId || '',
          semestre || '',
          anosEscolares === OPCAO_TODOS ? '' : anosEscolares || '',
          turmaId || '',
          dataInicio || '',
          dataFim || '',
          pesquisaComunicado || ''
        )
          .catch(e => erros(e))
          .finally(() => setExibirLoader(false));

        if (resposta?.data?.length) {
          const lista = resposta.data.map(item => {
            return {
              id: item.id,
              descricao: `${item.titulo} - ${moment(item.dataEnvio).format(
                'DD/MM/YYYY'
              )}`,
            };
          });
          setListaComunicado(lista);
        } else {
          setListaComunicado([]);
        }
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [
    anoLetivo,
    codigoDre,
    codigoUe,
    grupos,
    modalidadeId,
    semestre,
    anosEscolares,
    turmaId,
    dataInicio,
    dataFim,
    pesquisaComunicado,
    listaGrupos,
  ]);

  return (
    <Loader loading={exibirLoader}>
      <Cabecalho pagina="Relatório de leitura" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
              <Button
                id="btn-voltar"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={() => {
                  history.push('/');
                }}
              />
              <Button
                id="btn-cancelar"
                label="Cancelar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={() => {
                  cancelar();
                }}
              />
              <Button
                id="btn-gerar"
                icon="print"
                label="Gerar"
                color={Colors.Azul}
                border
                bold
                className="mr-0"
                onClick={gerar}
                disabled={
                  desabilitarGerar ||
                  String(modalidadeId) === String(modalidade.INFANTIL)
                }
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
              <CheckboxComponent
                label="Exibir histórico?"
                onChangeCheckbox={e => {
                  setAnoLetivo();
                  setCodigoDre();
                  setConsideraHistorico(e.target.checked);
                }}
                checked={consideraHistorico}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-2 col-xl-2 mb-2">
              <SelectComponent
                id="drop-ano-letivo"
                label="Ano Letivo"
                lista={listaAnosLetivo}
                valueOption="valor"
                valueText="desc"
                disabled={!consideraHistorico || listaAnosLetivo?.length === 1}
                onChange={onChangeAnoLetivo}
                valueSelect={anoLetivo}
                placeholder="Ano letivo"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
              <SelectComponent
                id="drop-dre"
                label="Diretoria Regional de Educação (DRE)"
                lista={listaDres}
                valueOption="valor"
                valueText="desc"
                disabled={!anoLetivo || listaDres?.length === 1}
                onChange={onChangeDre}
                valueSelect={codigoDre}
                placeholder="Diretoria Regional De Educação (DRE)"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
              <SelectComponent
                id="drop-ue"
                label="Unidade Escolar (UE)"
                lista={listaUes}
                valueOption="valor"
                valueText="desc"
                disabled={!codigoDre || listaUes?.length === 1}
                onChange={onChangeUe}
                valueSelect={codigoUe}
                placeholder="Unidade Escolar (UE)"
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
              <SelectComponent
                id="select-grupo"
                label="Grupo"
                lista={listaGrupos}
                valueOption="valor"
                valueText="desc"
                valueSelect={grupos}
                placeholder="Selecione o grupo"
                multiple
                onChange={valores => {
                  onchangeMultiSelect(valores, grupos, setGrupos);
                }}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
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
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-4 mb-2">
              <SelectComponent
                id="drop-semestre"
                lista={listaSemestres}
                valueOption="valor"
                valueText="desc"
                label="Semestre"
                disabled={
                  !modalidadeId ||
                  listaSemestres?.length === 1 ||
                  String(modalidadeId) !== String(modalidade.EJA)
                }
                valueSelect={semestre}
                onChange={onChangeSemestre}
                placeholder="Semestre"
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-3 mb-2">
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
            </div>
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
              <SelectComponent
                id="drop-turma"
                lista={listaTurmas}
                valueOption="valor"
                valueText="desc"
                label="Turma"
                disabled={!modalidadeId || listaTurmas?.length === 1}
                valueSelect={turmaId}
                placeholder="Turma"
                onChange={onChangeTurma}
              />
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
            </div>
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
              <RadioGroupButton
                label="Listar responsáveis/estudantes"
                opcoes={opcoesRadioSimNao}
                valorInicial
                onChange={e => {
                  setListarResponsaveisEstudantes(e.target.value);
                }}
                value={listarResponsaveisEstudantes}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
              <RadioGroupButton
                label="Listar comunicados expirados"
                opcoes={opcoesRadioSimNao}
                valorInicial
                onChange={e => {
                  setListarComunicadosExpirados(e.target.value);
                }}
                value={listarComunicadosExpirados}
              />
            </div>
          </div>
        </div>
      </Card>
    </Loader>
  );
};

export default RelatorioLeitura;
