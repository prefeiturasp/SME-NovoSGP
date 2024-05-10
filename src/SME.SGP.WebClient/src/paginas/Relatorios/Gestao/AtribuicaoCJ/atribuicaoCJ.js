import React, { useCallback, useState, useEffect } from 'react';

import {
  Loader,
  Button,
  Card,
  Colors,
  SelectComponent,
  Localizador,
  RadioGroupButton,
} from '~/componentes';
import { Cabecalho, FiltroHelper } from '~/componentes-sgp';

import {
  erros,
  sucesso,
  history,
  ServicoRelatorioAtribuicoes,
  AbrangenciaServico,
  ServicoFiltroRelatorio,
} from '~/servicos';

import { ordenarDescPor } from '~/utils/funcoes/gerais';

import { ModalidadeDTO } from '~/dtos';

import { URL_HOME } from '~/constantes';

const AtribuicaoCJ = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaSemestres, setListaSemestres] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);

  const [anoAtual] = useState(window.moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [dreCodigo, setDreCodigo] = useState(undefined);
  const [ueCodigo, setUeCodigo] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [turmaId, setTurmaId] = useState([]);
  const [usuarioRf, setUsuarioRf] = useState(undefined);
  const [exibirAulas, setExibirAulas] = useState(false);
  const [
    exibirAtribuicoesExporadicas,
    setExibirAtribuicoesExporadicas,
  ] = useState(false);
  const [tipoVisualizacao, setTipoVisualizacao] = useState(1);

  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);
  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const OPCAO_TODOS = '-99';

  const opcoesExibir = [
    { label: 'Não', value: false },
    { label: 'Sim', value: true },
  ];

  const opcoesTipoVisualizacao = [
    { label: 'Turma', value: 1 },
    { label: 'Professor', value: 2 },
  ];

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo();
    setAnoLetivo(anoAtual);
    setDreCodigo();
    setUeCodigo();
    setModalidadeId();
    setSemestre();
    setTurmaId([]);
    setUsuarioRf();
    setExibirAulas(false);
    setExibirAtribuicoesExporadicas(false);
    setTipoVisualizacao(1);
  };

  const onClickGerar = async () => {
    let turmasCodigo = turmaId;
    setCarregandoGeral(true);

    if (turmaId?.length && turmaId?.find(item => item !== OPCAO_TODOS)) {
      turmasCodigo = listaTurmas.filter(
        item => item.valor === turmaId.find(codigo => codigo === item.valor)
      );
      if (turmasCodigo?.length) {
        turmasCodigo = turmasCodigo.map(item => String(item.id));
      }
    }

    const retorno = await ServicoRelatorioAtribuicoes.gerar({
      anoLetivo: Number(anoLetivo),
      dreCodigo,
      ueCodigo,
      modalidade: Number(modalidadeId),
      semestre: Number(semestre) || 0,
      turmasCodigo,
      usuarioRf: usuarioRf || '',
      exibirAulas,
      exibirAtribuicoesExporadicas,
      tipoVisualizacao,
    })
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (retorno?.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
    }
  };

  const onChangeAnoLetivo = async valor => {
    setDreCodigo();
    setUeCodigo();
    setModalidadeId();
    setTurmaId([]);
    setAnoLetivo(valor);
  };

  const onChangeDre = valor => {
    setDreCodigo(valor);
    setUeCodigo();
    setModalidadeId();
    setTurmaId([]);
    setUeCodigo(undefined);
  };

  const onChangeUe = valor => {
    setModalidadeId();
    setTurmaId([]);
    setUeCodigo(valor);
  };

  const onChangeModalidade = valor => {
    setTurmaId([]);
    setModalidadeId(valor);
  };

  const onChangeSemestre = valor => {
    setSemestre(valor);
  };

  const onChangeTurma = valor => {
    setTurmaId(valor);
  };

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoGeral(true);
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

    if (!anosLetivos?.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    if (anosLetivos?.length) {
      const temAnoAtualNaLista = anosLetivos.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) setAnoLetivo(Number(anoAtual));
      else setAnoLetivo(Number(anosLetivos[0].valor));
    }

    const anosOrdenados = ordenarDescPor(anosLetivos, 'desc');
    setListaAnosLetivo(anosOrdenados);
    setCarregandoGeral(false);
  }, [anoAtual]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoGeral(true);
      const resposta = await ServicoFiltroRelatorio.obterDres()
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
          abrev: item.abreviacao,
        }));

        setListaDres(lista);

        if (lista?.length && lista?.length === 1) {
          setDreCodigo(lista[0].valor);
        }
      }
      return;
    }

    setListaDres([]);
    setDreCodigo(undefined);
  }, [anoLetivo]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const obterUes = useCallback(async dre => {
    if (dre) {
      setCarregandoGeral(true);
      const resposta = await ServicoFiltroRelatorio.obterUes(dre)
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista?.length && lista?.length === 1) {
          setUeCodigo(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, []);

  useEffect(() => {
    if (dreCodigo) {
      obterUes(dreCodigo, anoLetivo);
      return;
    }
    setUeCodigo();
    setListaUes([]);
  }, [dreCodigo, anoLetivo, obterUes]);

  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      setCarregandoGeral(true);
      const {
        data,
      } = await ServicoFiltroRelatorio.obterModalidadesPorAbrangencia(ue);

      if (data) {
        const lista = data.map(item => ({
          desc: item.descricao,
          valor: String(item.valor),
        }));

        if (lista?.length && lista?.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
      setCarregandoGeral(false);
    }
  };

  useEffect(() => {
    if (anoLetivo && ueCodigo) {
      obterModalidades(ueCodigo, anoLetivo);
      return;
    }
    setModalidadeId();
    setListaModalidades([]);
  }, [anoLetivo, ueCodigo]);

  const obterTurmas = useCallback(async (modalidadeSelecionada, ue, ano) => {
    if (ue && modalidadeSelecionada) {
      setCarregandoGeral(true);
      const { data } = await AbrangenciaServico.buscarTurmas(
        ue,
        modalidadeSelecionada,
        '',
        ano
      );
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
          setTurmaId([lista[0].valor]);
        }
      }
      setCarregandoGeral(false);
    }
  }, []);

  useEffect(() => {
    if (modalidadeId && ueCodigo) {
      obterTurmas(modalidadeId, ueCodigo, anoLetivo);
      return;
    }
    setTurmaId([]);
    setListaTurmas([]);
  }, [modalidadeId, ueCodigo, anoLetivo, obterTurmas]);

  useEffect(() => {
    const bi = [];
    bi.push({ desc: '1º', valor: '1' });
    bi.push({ desc: '2º', valor: '2' });

    if (String(modalidadeId) !== String(ModalidadeDTO.EJA)) {
      bi.push({ desc: '3º', valor: '3' });
      bi.push({ desc: '4º', valor: '4' });
    }

    bi.push({ desc: 'Final', valor: '0' });
    bi.push({ desc: 'Todos', valor: OPCAO_TODOS });
    setListaSemestres(bi);
  }, [modalidadeId]);

  useEffect(() => {
    const desabilitar = !anoLetivo || !dreCodigo || !ueCodigo;

    setDesabilitarBtnGerar(desabilitar);
  }, [anoLetivo, dreCodigo, ueCodigo]);

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

  return (
    <>
      <Cabecalho pagina="Relatório de atribuição CJ" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-md-12 ">
            <div className="row mb-5">
              <div className="col-md-12 d-flex justify-content-end">
                <Button
                  id="btn-voltar"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-3"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={onClickCancelar}
                />
                <Button
                  id="btn-gerar"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  onClick={onClickGerar}
                  disabled={desabilitarBtnGerar}
                />
              </div>
            </div>

            <div className="row mb-4">
              <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2 pl-0">
                <SelectComponent
                  id="drop-ano-letivo"
                  label="Ano letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo && listaAnosLetivo.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2 pl-0">
                <SelectComponent
                  id="drop-dre"
                  label="DRE"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!anoLetivo || (listaDres && listaDres.length === 1)}
                  onChange={onChangeDre}
                  valueSelect={dreCodigo}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2 p-0">
                <SelectComponent
                  id="drop-ue"
                  label="UE"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!dreCodigo || (listaUes && listaUes.length === 1)}
                  onChange={onChangeUe}
                  valueSelect={ueCodigo}
                  placeholder="Unidade Escolar (UE)"
                />
              </div>
            </div>

            <div className="row mb-4">
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2 pl-0">
                <SelectComponent
                  id="drop-modalidade"
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !ueCodigo ||
                    (listaModalidades && listaModalidades.length === 1)
                  }
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Modalidade"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2 pl-0">
                <SelectComponent
                  id="drop-semestre"
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
              </div>
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2 p-0">
                <SelectComponent
                  id="drop-turma"
                  lista={listaTurmas}
                  valueOption="valor"
                  valueText="desc"
                  label="Turma"
                  disabled={
                    !modalidadeId || (listaTurmas && listaTurmas.length === 1)
                  }
                  valueSelect={turmaId}
                  placeholder="Turma"
                  multiple
                  onChange={valores => {
                    onchangeMultiSelect(valores, turmaId, onChangeTurma);
                  }}
                />
              </div>
            </div>

            <div className="row mb-4">
              <Localizador
                labelRF="RF"
                placeholderRF="Procure pelo RF do usuário"
                placeholderNome="Procure pelo nome do usuário"
                labelNome="Nome"
                rfEdicao={usuarioRf}
                buscandoDados={setCarregandoGeral}
                dreId={dreCodigo}
                anoLetivo={anoAtual}
                showLabel
                onChange={valores => {
                  if (valores && valores.professorRf) {
                    setUsuarioRf(valores.professorRf);
                  } else {
                    setUsuarioRf(undefined);
                  }
                }}
                buscarOutrosCargos
                classesRF="p-0"
              />
            </div>

            <div className="row mb-4">
              <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2 pl-0">
                <RadioGroupButton
                  label="Exibir aulas"
                  opcoes={opcoesExibir}
                  valorInicial
                  onChange={e => {
                    setExibirAulas(e.target.value);
                  }}
                  value={exibirAulas}
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2 pl-0">
                <RadioGroupButton
                  label="Exibir atribuições esporádicas"
                  opcoes={opcoesExibir}
                  valorInicial
                  onChange={e => {
                    setExibirAtribuicoesExporadicas(e.target.value);
                  }}
                  value={exibirAtribuicoesExporadicas}
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-7 col-xl-7 mb-2 p-0">
                <RadioGroupButton
                  label="Tipo de visualização"
                  opcoes={opcoesTipoVisualizacao}
                  valorInicial
                  onChange={e => {
                    setTipoVisualizacao(e.target.value);
                  }}
                  value={tipoVisualizacao}
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default AtribuicaoCJ;
