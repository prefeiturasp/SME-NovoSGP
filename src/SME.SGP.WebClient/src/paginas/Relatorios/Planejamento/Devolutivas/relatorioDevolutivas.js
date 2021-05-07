import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import {
  Button,
  Card,
  CheckboxComponent,
  Colors,
  Loader,
  RadioGroupButton,
  SelectComponent,
} from '~/componentes';
import {
  AlertaPermiteSomenteTurmaInfantil,
  Cabecalho,
  FiltroHelper,
} from '~/componentes-sgp';

import { ModalidadeDTO } from '~/dtos';

import {
  AbrangenciaServico,
  ehTurmaInfantil,
  erros,
  history,
  ServicoFiltroRelatorio,
  ServicoRelatorioDevolutivas,
  sucesso,
} from '~/servicos';

const RelatorioDevolutivas = () => {
  const [anoAtual] = useState(window.moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState();
  const [bimestres, setBimestres] = useState(undefined);
  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoModalidade, setCarregandoModalidade] = useState(false);
  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [consideraHistorico, setConsideraHistorico] = useState(false);
  const [desabilitarGerar, setDesabilitarGerar] = useState(true);
  const [dreId, setDreId] = useState();
  const [exibirConteudoDevolutiva, setExibirConteudoDevolutiva] = useState(
    false
  );
  const [exibirLoaderGeral, setExibirLoaderGeral] = useState(false);

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaBimestre, setListaBimestre] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaUes, setListaUes] = useState([]);

  const [modalidadeId, setModalidadeId] = useState();
  const [naoEhInfantil, setNaoEhInfantil] = useState(false);
  const [turmaId, setTurmaId] = useState();
  const [ueId, setUeId] = useState();

  const { turmaSelecionada } = useSelector(store => store.usuario);

  const OPCAO_TODOS = '-99';
  const opcoesRadioSimNao = [
    { label: 'Não', value: false },
    { label: 'Sim', value: true },
  ];

  const limparFiltrosSelecionados = () => {
    setDreId();
    setListaDres([]);
    setUeId();
    setListaUes([]);
    setListaTurmas([]);
    setTurmaId();
    setNaoEhInfantil(false);
  };

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickCancelar = () => {
    setAnoLetivo();
    limparFiltrosSelecionados();
  };

  const gerar = async () => {
    setExibirLoaderGeral(true);

    const ue = listaUes.find(item => String(item.valor) === String(ueId));
    const retorno = await ServicoRelatorioDevolutivas.gerar({
      ano: anoLetivo,
      dreId,
      ueId: ue?.id,
      bimestres,
      turmas: turmaId,
      exibirDetalhes: exibirConteudoDevolutiva,
    })
      .catch(e => erros(e))
      .finally(setExibirLoaderGeral(false));
    if (retorno?.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
    }
  };

  const onCheckedConsideraHistorico = () => {
    limparFiltrosSelecionados();
    setConsideraHistorico(!consideraHistorico);
    setAnoLetivo(anoAtual);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);
    limparFiltrosSelecionados();
  };

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnosLetivos(true);
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
    setCarregandoAnosLetivos(false);
  }, [anoAtual]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivo}`
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data?.length) {
        const lista = resposta.data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
            id: item.id,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDreId(lista[0].valor);
        }
        return;
      }
      setDreId(undefined);
      setListaDres([]);
    }
  }, [anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (anoLetivo) {
      obterDres();
    }
  }, [anoLetivo, obterDres]);

  const onChangeDre = dre => {
    setDreId(dre);

    setListaUes([]);
    setUeId();

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeUe = ue => {
    setUeId(ue);

    setListaTurmas([]);
    setTurmaId();
    if (!ue) {
      setNaoEhInfantil(false);
    }
  };

  const obterUes = useCallback(async () => {
    if (anoLetivo && dreId) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dreId,
        `v1/abrangencias/${consideraHistorico}/dres/${dreId}/ues?anoLetivo=${anoLetivo}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
          id: item.id,
        }));

        if (lista?.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
        return;
      }
      setListaUes([]);
    }
  }, [dreId, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (dreId) {
      obterUes();
      return;
    }
    setUeId();
    setListaUes([]);
  }, [dreId, obterUes]);

  const onChangeModalidade = valor => {
    setTurmaId();
    setModalidadeId(valor);
  };

  const verificarAbrangencia = data => {
    const modalidadeInfatil = data.filter(
      item => String(item.valor) === String(ModalidadeDTO.INFANTIL)
    );
    if (!modalidadeInfatil.length) {
      setNaoEhInfantil(true);
    }
  };

  const obterModalidades = useCallback(async ue => {
    if (ue) {
      setCarregandoModalidade(true);
      const {
        data,
      } = await ServicoFiltroRelatorio.obterModalidadesPorAbrangencia(
        ue
      ).finally(() => setCarregandoModalidade(false));

      if (data?.length) {
        const lista = data.map(item => ({
          desc: item.descricao,
          valor: String(item.valor),
        }));

        setListaModalidades(lista);
        let naoInfantil = true;
        if (lista?.length === 1) {
          if (String(lista[0].valor) === String(ModalidadeDTO.INFANTIL)) {
            setModalidadeId(lista[0].valor);
            naoInfantil = false;
          }
          setNaoEhInfantil(naoInfantil);
          return;
        }
        verificarAbrangencia(data);
      }
    }
  }, []);

  useEffect(() => {
    if (anoLetivo && ueId) {
      obterModalidades(ueId);
      return;
    }
    setModalidadeId();
    setListaModalidades([]);
  }, [obterModalidades, anoLetivo, ueId]);

  const onChangeTurma = valor => {
    setTurmaId(valor);
    setBimestres([]);
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

  const obterTurmas = useCallback(async () => {
    if (dreId && ueId && modalidadeId) {
      setCarregandoTurmas(true);
      const { data } = await AbrangenciaServico.buscarTurmas(
        ueId,
        modalidadeId,
        '',
        anoLetivo,
        consideraHistorico
      ).finally(() => setCarregandoTurmas(false));
      if (data) {
        const lista = [];
        if (data.length > 1) {
          lista.push({ valor: OPCAO_TODOS, desc: 'Todas' });
        }
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
          setTurmaId([lista[0].id]);
        }
      }
    }
  }, [ueId, dreId, consideraHistorico, anoLetivo, modalidadeId]);

  useEffect(() => {
    if (ueId && !naoEhInfantil) {
      obterTurmas();
      return;
    }
    setTurmaId();
    setListaTurmas([]);
  }, [ueId, obterTurmas, naoEhInfantil]);

  const onChangeBimestre = valor => setBimestres(valor);

  const obterBimestres = useCallback(() => {
    const bi = [];
    bi.push({ desc: '1º', valor: 1 });
    bi.push({ desc: '2º', valor: 2 });

    if (modalidadeId !== ModalidadeDTO.EJA) {
      bi.push({ desc: '3º', valor: 3 });
      bi.push({ desc: '4º', valor: 4 });
    }

    bi.push({ desc: 'Todos', valor: -99 });
    setListaBimestre(bi);
  }, [modalidadeId]);

  useEffect(() => {
    if (modalidadeId) {
      obterBimestres();
      return;
    }
    setListaBimestre([]);
    setBimestres(undefined);
  }, [modalidadeId, obterBimestres]);

  useEffect(() => {
    const ehInfatil = ehTurmaInfantil(ModalidadeDTO, turmaSelecionada);
    if (Object.keys(turmaSelecionada).length) {
      setNaoEhInfantil(!ehInfatil);
      return;
    }
    setNaoEhInfantil(false);
  }, [turmaSelecionada]);

  useEffect(() => {
    const desabilitar =
      !anoLetivo || !dreId || !ueId || !turmaId?.length || !bimestres?.length;
    setDesabilitarGerar(desabilitar);
  }, [anoLetivo, dreId, ueId, turmaId, bimestres]);

  return (
    <Loader loading={exibirLoaderGeral}>
      {naoEhInfantil && (
        <AlertaPermiteSomenteTurmaInfantil
          marginBottom={3}
          exibir={naoEhInfantil}
        />
      )}
      <Cabecalho pagina="Relatório de devolutivas" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row mb-5">
            <div className="col-sm-12 d-flex justify-content-end">
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
                className="mr-2"
                onClick={onClickCancelar}
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
                disabled={desabilitarGerar}
              />
            </div>
          </div>
          <div className="row mb-4">
            <div className="col-sm-12">
              <CheckboxComponent
                id="exibir-historico"
                label="Exibir histórico?"
                onChangeCheckbox={onCheckedConsideraHistorico}
                checked={consideraHistorico}
                disabled={naoEhInfantil}
              />
            </div>
          </div>
          <div className="row mb-4">
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pr-0">
              <Loader loading={carregandoAnosLetivos} ignorarTip>
                <SelectComponent
                  id="drop-ano-letivo"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !consideraHistorico ||
                    naoEhInfantil ||
                    listaAnosLetivo?.length === 1
                  }
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-5 col-lg-5 col-xl-5 pr-0">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  id="dre"
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    naoEhInfantil || !anoLetivo || listaDres?.length === 1
                  }
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-5 col-lg-5 col-xl-5">
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  id="ue"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!dreId || listaUes?.length === 1}
                  onChange={onChangeUe}
                  valueSelect={ueId}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
          </div>
          <div className="row mb-4">
            <div className="col-sm-12 col-md-4 pr-0">
              <Loader loading={carregandoModalidade} ignorarTip>
                <SelectComponent
                  id="drop-modalidade"
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="desc"
                  disabled
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Modalidade"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-4 pr-0">
              <Loader loading={carregandoTurmas} ignorarTip>
                <SelectComponent
                  multiple
                  id="turma"
                  lista={listaTurmas}
                  valueOption="id"
                  valueText="desc"
                  label="Turma"
                  disabled={!modalidadeId || listaTurmas?.length === 1}
                  valueSelect={turmaId}
                  onChange={valores => {
                    onchangeMultiSelect(valores, turmaId, onChangeTurma);
                  }}
                  placeholder="Turma"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-4">
              <SelectComponent
                lista={listaBimestre}
                valueOption="valor"
                valueText="desc"
                label="Bimestre"
                disabled={!modalidadeId || listaBimestre?.length === 1}
                valueSelect={bimestres}
                multiple
                onChange={valores => {
                  onchangeMultiSelect(valores, bimestres, onChangeBimestre);
                }}
                placeholder="Selecione o bimestre"
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12">
              <RadioGroupButton
                label="Exibir conteúdo da devolutiva"
                opcoes={opcoesRadioSimNao}
                valorInicial
                onChange={e => {
                  setExibirConteudoDevolutiva(e.target.value);
                }}
                value={exibirConteudoDevolutiva}
                desabilitado={!dreId || !ueId || !turmaId || !bimestres}
              />
            </div>
          </div>
        </div>
      </Card>
    </Loader>
  );
};

export default RelatorioDevolutivas;
