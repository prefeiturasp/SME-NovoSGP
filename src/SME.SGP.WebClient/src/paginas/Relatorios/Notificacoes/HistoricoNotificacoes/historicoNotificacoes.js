import React, { useCallback, useEffect, useState } from 'react';
import {
  Loader,
  Localizador,
  RadioGroupButton,
  SelectComponent,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoHistoricoNotificacoes from '~/servicos/Paginas/Relatorios/Historico/HistoricoNotificacoes/ServicoHistoricoNotificacoes';

const HistoricoNotificacoes = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaSemestre, setListaSemestre] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaCategorias, setListaCategorias] = useState([]);
  const [listaTipos, setListaTipos] = useState([]);
  const [listaSituacao, setListaSituacao] = useState([]);

  const [anoAtual] = useState(window.moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState(anoAtual);
  const [codigoDre, setCodigoDre] = useState(undefined);
  const [codigoUe, setCodigoUe] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [usuarioRf, setUsuarioRf] = useState(undefined);
  const [categorias, setCategorias] = useState([]);
  const [tipos, setTipos] = useState([]);
  const [situacoes, setSituacoes] = useState([]);
  const [exibirDescricao, setExibirDescricao] = useState(false);
  const [
    exibirNotificacoesExcluidas,
    setExibirNotificacoesExcluidas,
  ] = useState(false);

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);

  const OPCAO_TODAS = '-99';

  const opcoesExibirDescricao = [
    { label: 'Sim', value: true },
    { label: 'Não', value: false },
  ];

  const opcoesExibirNotificacoesExcluidas = [
    { label: 'Sim', value: true },
    { label: 'Não', value: false },
  ];

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoGeral(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos().catch(
      e => {
        erros(e);
        setCarregandoGeral(false);
      }
    );
    if (anosLetivo && anosLetivo.data) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
    setCarregandoGeral(false);
  }, []);

  const obterModalidades = async ue => {
    if (ue) {
      setCarregandoGeral(true);
      const retorno = await ServicoFiltroRelatorio.obterModalidadesPorAbrangencia(
        ue
      ).catch(e => {
        erros(e);
        setCarregandoGeral(false);
      });
      if (retorno && retorno.data) {
        if (retorno.data && retorno.data.length && retorno.data.length === 1) {
          setModalidadeId(retorno.data[0].valor);
        }
        setListaModalidades(retorno.data);
      }
      setCarregandoGeral(false);
    }
  };

  const obterUes = useCallback(async dre => {
    if (dre) {
      setCarregandoGeral(true);
      const retorno = await ServicoFiltroRelatorio.obterUes(dre).catch(e => {
        erros(e);
        setCarregandoGeral(false);
      });
      if (retorno && retorno.data) {
        const lista = retorno.data.map(item => ({
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
      setCarregandoGeral(false);
    }
  }, []);

  const onChangeDre = dre => {
    setCodigoDre(dre);

    setListaUes([]);
    setCodigoUe(undefined);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId();
  };

  const obterDres = async () => {
    setCarregandoGeral(true);
    const retorno = await ServicoFiltroRelatorio.obterDres().catch(e => {
      erros(e);
      setCarregandoGeral(false);
    });
    if (retorno && retorno.data && retorno.data.length) {
      setListaDres(retorno.data);

      if (retorno && retorno.data.length && retorno.data.length === 1) {
        setCodigoDre(retorno.data[0].codigo);
      }
    } else {
      setListaDres([]);
    }
    setCarregandoGeral(false);
  };

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    setCarregandoGeral(true);
    const retorno = await api
      .get(
        `v1/abrangencias/false/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}`
      )
      .catch(e => {
        erros(e);
        setCarregandoGeral(false);
      });
    if (retorno && retorno.data) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista && lista.length && lista.length === 1) {
        setSemestre(lista[0].valor);
      }
      setListaSemestre(lista);
    }
    setCarregandoGeral(false);
  };

  const obterTurmas = useCallback(async () => {
    if (codigoUe === OPCAO_TODAS) {
      setListaTurmas([{ valor: '-99', descricao: 'Todas' }]);
      setTurmaId(OPCAO_TODAS);
      return;
    }

    setCarregandoGeral(true);
    const resposta = await ServicoFiltroRelatorio.obterTurmasPorCodigoUeModalidadeSemestre(
      anoLetivo,
      codigoUe,
      modalidadeId,
      semestre
    )
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (resposta?.data?.length) {
      const lista = resposta.data;
      if (lista.length > 1) {
        lista.unshift({ valor: '-99', descricao: 'Todas' });
      }

      setListaTurmas(lista);
      if (lista.length === 1) {
        setTurmaId(lista[0].valor);
      }
    }
  }, [anoLetivo, codigoUe, modalidadeId, semestre]);

  useEffect(() => {
    if (modalidadeId && codigoUe && anoLetivo && modalidadeId) {
      obterTurmas();
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [modalidadeId, codigoUe, anoLetivo, semestre, obterTurmas]);

  useEffect(() => {
    if (codigoUe) {
      obterModalidades(codigoUe);
    } else {
      setModalidadeId(undefined);
      setListaModalidades([]);
    }
  }, [codigoUe]);

  useEffect(() => {
    if (codigoDre) {
      obterUes(codigoDre);
    } else {
      setCodigoUe(undefined);
      setListaUes([]);
    }
  }, [codigoDre, obterUes]);

  useEffect(() => {
    if (modalidadeId && anoLetivo) {
      if (modalidadeId == modalidade.EJA) {
        obterSemestres(modalidadeId, anoLetivo);
      } else {
        setSemestre(undefined);
        setListaSemestre([]);
      }
    } else {
      setSemestre(undefined);
      setListaSemestre([]);
    }
  }, [modalidadeId, anoLetivo]);

  useEffect(() => {
    const desabilitar = !anoLetivo || !codigoDre || !codigoUe;

    if (modalidadeId == modalidade.EJA) {
      setDesabilitarBtnGerar(!semestre || desabilitar);
    } else {
      setDesabilitarBtnGerar(desabilitar);
    }
  }, [
    anoLetivo,
    codigoDre,
    codigoUe,
    modalidadeId,
    semestre,
    turmaId,
    usuarioRf,
    categorias,
    tipos,
    situacoes,
    exibirDescricao,
    exibirNotificacoesExcluidas,
  ]);

  const carregarListas = async () => {
    const status = await api.get('v1/notificacoes/status').catch(e => erros(e));
    if (status?.data?.length) {
      if (status.data.length > 1) {
        status.data.unshift({ descricao: 'Todas', id: OPCAO_TODAS });
      }
      setListaSituacao(status.data);
    } else {
      setListaSituacao([]);
    }

    const cat = await api
      .get('v1/notificacoes/categorias')
      .catch(e => erros(e));

    if (cat?.data?.length) {
      if (cat.data.length > 1) {
        cat.data.unshift({ descricao: 'Todas', id: OPCAO_TODAS });
      }
      setListaCategorias(cat.data);
    } else {
      setListaCategorias([]);
    }

    const tip = await api.get('v1/notificacoes/tipos').catch(e => erros(e));
    if (tip?.data?.length) {
      if (tip.data.length > 1) {
        tip.data.unshift({ descricao: 'Todos', id: OPCAO_TODAS });
      }
      setListaTipos(tip.data);
    } else {
      setListaTipos([]);
    }
  };

  useEffect(() => {
    obterAnosLetivos();
    obterDres();
    carregarListas();
  }, [obterAnosLetivos]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo(anoAtual);
    setCodigoDre();
    setCodigoUe();
    setModalidadeId();
    setSemestre();
    setTurmaId();
    setUsuarioRf();
    setCategorias([]);
    setTipos([]);
    setSituacoes([]);
    setExibirDescricao(false);
    setExibirNotificacoesExcluidas(false);
    setDesabilitarBtnGerar(true);

    setListaAnosLetivo([]);
    setListaDres([]);
    setListaUes([]);
    setListaModalidades([]);
    setListaSemestre([]);
    setListaTurmas([]);

    obterAnosLetivos();
    obterDres();
  };

  const onClickGerar = async () => {
    const params = {
      anoLetivo,
      dre: codigoDre,
      ue: codigoUe,
      modalidadeTurma: modalidadeId,
      semestre,
      turma: turmaId,
      usuarioBuscaRf: usuarioRf,
      categorias: categorias,
      tipos: tipos,
      situacoes: situacoes,
      exibirDescricao,
      exibirNotificacoesExcluidas,
    };

    setCarregandoGeral(true);
    const retorno = await ServicoHistoricoNotificacoes.gerar(params)
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (retorno && retorno.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
      setDesabilitarBtnGerar(true);
    }
  };

  const onChangeUe = ue => {
    setCodigoUe(ue);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeModalidade = novaModalidade => {
    setModalidadeId(novaModalidade);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeSemestre = valor => setSemestre(valor);

  const onchangeMultiSelect = (valores, valoreAtual, funSetarNovoValor) => {
    const opcaoTodosJaSelecionado = valoreAtual
      ? valoreAtual.includes('-99')
      : false;
    if (opcaoTodosJaSelecionado) {
      const listaSemOpcaoTodos = valores.filter(v => v !== '-99');
      funSetarNovoValor(listaSemOpcaoTodos);
    } else if (valores.includes('-99')) {
      funSetarNovoValor(['-99']);
    } else {
      funSetarNovoValor(valores);
    }
  };

  return (
    <>
      <Cabecalho pagina="Relatório de notificações" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id="btn-voltar"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={() => onClickCancelar()}
                />
                <Button
                  id="btn-gerar"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  className="mr-2"
                  onClick={() => onClickGerar()}
                  disabled={desabilitarBtnGerar}
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo && listaAnosLetivo.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Selecione o ano"
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-9 col-xl-5 mb-2">
                <SelectComponent
                  label="DRE"
                  lista={listaDres}
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaDres && listaDres.length === 1}
                  onChange={onChangeDre}
                  valueSelect={codigoDre}
                  placeholder="Diretoria Regional de Educação (DRE)"
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-12 col-xl-5 mb-2">
                <SelectComponent
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaUes && listaUes.length === 1}
                  onChange={onChangeUe}
                  valueSelect={codigoUe}
                  placeholder="Unidade Escolar (UE)"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-5 mb-2">
                <SelectComponent
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="descricao"
                  disabled={listaModalidades && listaModalidades.length === 1}
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Selecione uma modalidade"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaSemestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Semestre"
                  disabled={
                    !modalidadeId ||
                    modalidadeId != modalidade.EJA ||
                    (listaSemestre && listaSemestre.length === 1)
                  }
                  valueSelect={semestre}
                  onChange={onChangeSemestre}
                  placeholder="Selecione o semestre"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-5 mb-2">
                <SelectComponent
                  id="drop-turma"
                  lista={listaTurmas}
                  valueOption="valor"
                  valueText="descricao"
                  label="Turma"
                  disabled={
                    !modalidadeId || (listaTurmas && listaTurmas.length === 1)
                  }
                  valueSelect={turmaId}
                  onChange={setTurmaId}
                  placeholder="Turma"
                />
              </div>
              <div className="col-md-12 mb-2">
                <div className="row pr-3">
                  <Localizador
                    rfEdicao={usuarioRf}
                    buscandoDados={setCarregandoGeral}
                    dreId={codigoDre}
                    anoLetivo={anoLetivo}
                    showLabel
                    onChange={valores => {
                      if (valores && valores.professorRf) {
                        setUsuarioRf(valores.professorRf);
                      } else {
                        setUsuarioRf();
                      }
                    }}
                    buscarOutrosCargos
                  />
                </div>
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
                <SelectComponent
                  label="Categoria"
                  id="categoria-noti"
                  lista={listaCategorias}
                  valueOption="id"
                  valueText="descricao"
                  onChange={valores => {
                    onchangeMultiSelect(valores, categorias, setCategorias);
                  }}
                  valueSelect={categorias}
                  placeholder="Categoria"
                  multiple
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
                <SelectComponent
                  label="Tipo"
                  id="tipo-noti"
                  lista={listaTipos}
                  valueOption="id"
                  valueText="descricao"
                  onChange={valores => {
                    onchangeMultiSelect(valores, tipos, setTipos);
                  }}
                  valueSelect={tipos}
                  placeholder="Tipo"
                  multiple
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
                <SelectComponent
                  label="Situação"
                  id="situacao-noti"
                  lista={listaSituacao}
                  valueOption="id"
                  valueText="descricao"
                  onChange={valores => {
                    onchangeMultiSelect(valores, situacoes, setSituacoes);
                  }}
                  valueSelect={situacoes}
                  placeholder="Situação"
                  multiple
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <RadioGroupButton
                  label="Exibir descrição"
                  opcoes={opcoesExibirDescricao}
                  valorInicial
                  onChange={e => {
                    setExibirDescricao(e.target.value);
                  }}
                  value={exibirDescricao}
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <RadioGroupButton
                  label="Exibir notificações excluídas"
                  opcoes={opcoesExibirNotificacoesExcluidas}
                  valorInicial
                  onChange={e => {
                    setExibirNotificacoesExcluidas(e.target.value);
                  }}
                  value={exibirNotificacoesExcluidas}
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default HistoricoNotificacoes;
