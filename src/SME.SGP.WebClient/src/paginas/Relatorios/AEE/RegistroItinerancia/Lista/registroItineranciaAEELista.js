import React, { useCallback, useEffect, useState } from 'react';

import {
  Button,
  CampoData,
  Card,
  CheckboxComponent,
  Colors,
  ListaPaginada,
  Loader,
  LocalizadorEstudante,
  SelectComponent,
} from '~/componentes';
import { Cabecalho, FiltroHelper } from '~/componentes-sgp';

import { URL_HOME } from '~/constantes';
import { RotasDto } from '~/dtos';
import {
  AbrangenciaServico,
  erros,
  history,
  ServicoRegistroItineranciaAEE,
} from '~/servicos';

const RegistroItineranciaAEELista = () => {
  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState();
  const [anoAtual] = useState(window.moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState();
  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoSituacao, setCarregandoSituacao] = useState(false);
  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [consideraHistorico, setConsideraHistorico] = useState(false);
  const [dataFinal, setDataFinal] = useState();
  const [dataInicial, setDataInicial] = useState();
  const [dreId, setDreId] = useState();
  const [filtro, setFiltro] = useState({});
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaSituacao, setListaSituacao] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [situacao, setSituacao] = useState();
  const [turmaId, setTurmaId] = useState();
  const [ueId, setUeId] = useState();

  const colunas = [
    {
      title: 'Data da visita',
      dataIndex: 'dataVisita',
    },
    {
      title: 'Unidade Escolar (UE)',
      dataIndex: 'ue',
    },
    {
      title: 'Crianças/Estudantes ',
      dataIndex: 'alunos',
    },
    {
      title: 'Turma (Regular)',
      dataIndex: 'Turma',
    },
    {
      title: 'Situação',
      dataIndex: 'situacao',
    },
  ];

  const filtrar = (dre, ue, turma, aluno, situa, dtInicial, dtFinal) => {
    if (anoLetivo && dre && listaDres?.length) {
      const dreSelecionada = listaDres.find(
        item => String(item.valor) === String(dre)
      );

      const ueSelecionada = listaUes.find(
        item => String(item.valor) === String(ue)
      );

      const turmaSelecionada = listaTurmas.find(
        item => String(item.codigo) === String(turma)
      );

      const params = {
        dreId: dreSelecionada ? dreSelecionada?.id : '',
        ueId: ueSelecionada ? ueSelecionada?.id : '',
        turmaId: turmaSelecionada ? turmaSelecionada?.id : '',
        alunoCodigo: aluno,
        situacao: situa,
        dataInicial: dtInicial,
        dataFinal: dtFinal,
      };
      setFiltro({ ...params });
    }
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    history.push(`${RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA}/novo`);
  };

  const limparFiltrosSelecionados = () => {
    setDreId();
    setListaDres([]);
    setUeId();
    setListaUes([]);
    setListaTurmas([]);
    setTurmaId();
    setAlunoLocalizadorSelecionado();
    setSituacao();
    dataInicial();
    dataFinal();
  };

  const onCheckedConsideraHistorico = e => {
    limparFiltrosSelecionados();
    setConsideraHistorico(e.target.checked);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    limparFiltrosSelecionados();
  };

  const validarValorPadraoAnoLetivo = useCallback(
    lista => {
      if (lista?.length) {
        const temAnoAtualNaLista = lista.find(
          item => String(item.valor) === String(anoAtual)
        );
        if (temAnoAtualNaLista) {
          setAnoLetivo(anoAtual);
          return;
        }
        setAnoLetivo(lista[0].valor);
        return;
      }
      setAnoLetivo();
    },
    [anoAtual]
  );

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);

    const anosLetivos = await FiltroHelper.obterAnosLetivos({
      consideraHistorico,
    })
      .catch(e => erros(e))
      .finally(() => setCarregandoAnos(false));

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    validarValorPadraoAnoLetivo(anosLetivos);

    setListaAnosLetivo(anosLetivos);
  }, [anoAtual, consideraHistorico, validarValorPadraoAnoLetivo]);

  useEffect(() => {
    if (!listaAnosLetivo?.length) {
      obterAnosLetivos();
    }
  }, [obterAnosLetivos, consideraHistorico, listaAnosLetivo]);

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

    filtrar(
      dre,
      ueId,
      turmaId,
      alunoLocalizadorSelecionado,
      situacao,
      dataInicial,
      dataFinal
    );
  };

  const onChangeUe = ue => {
    setUeId(ue);

    setListaTurmas([]);
    setTurmaId();

    filtrar(
      dreId,
      ue,
      turmaId,
      alunoLocalizadorSelecionado,
      situacao,
      dataInicial,
      dataFinal
    );
  };

  const onChangeTurma = valor => {
    setTurmaId(valor);
    setAlunoLocalizadorSelecionado();
    filtrar(dreId, ueId, valor, '', situacao);
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

  const obterTurmas = useCallback(async () => {
    if (anoLetivo && ueId) {
      setCarregandoTurmas(true);
      const resposta = await AbrangenciaServico.buscarTurmas(
        ueId,
        0,
        '',
        anoLetivo,
        consideraHistorico
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTurmas(false));

      if (resposta?.data) {
        setListaTurmas(resposta.data);

        if (resposta?.data?.length === 1) {
          setTurmaId(resposta.data[0].codigo);
        }
      }
    }
  }, [anoLetivo, ueId, consideraHistorico]);

  useEffect(() => {
    if (ueId) {
      obterTurmas();
      return;
    }
    setTurmaId();
    setListaTurmas([]);
  }, [ueId, obterTurmas]);

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno?.alunoCodigo);
      filtrar(dreId, ueId, turmaId, aluno?.alunoCodigo, situacao);
      return;
    }
    setAlunoLocalizadorSelecionado();
    filtrar(dreId, ueId, turmaId, '', situacao);
  };

  const obterSituacoes = useCallback(async () => {
    setCarregandoSituacao(true);
    const resposta = await ServicoRegistroItineranciaAEE.obterSituacoes()
      .catch(e => erros(e))
      .finally(() => setCarregandoSituacao(false));
    if (resposta?.data?.length) {
      setListaSituacao(resposta.data);
      return;
    }
    setListaSituacao([]);
  }, []);

  useEffect(() => {
    obterSituacoes();
  }, [obterSituacoes]);

  const onChangeSituacao = valor => {
    setSituacao(valor);
    filtrar(
      dreId,
      ueId,
      turmaId,
      alunoLocalizadorSelecionado,
      valor,
      dataInicial,
      dataFinal
    );
  };

  const mudarDataInicial = async data => {
    setDataInicial(data);
    filtrar(dreId, ueId, turmaId, alunoLocalizadorSelecionado, data, dataFinal);
  };

  const mudarDataFinal = async data => {
    setDataFinal(data);
    filtrar(
      dreId,
      ueId,
      turmaId,
      alunoLocalizadorSelecionado,
      dataInicial,
      data
    );
  };

  const onClickEditar = item => {
    history.push(
      `${RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA}/editar/${item.id}`
    );
  };

  return (
    <>
      <Cabecalho pagina="Registro de itinerância" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row mb-4">
            <div className="col-sm-12 d-flex justify-content-end">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Novo"
                color={Colors.Roxo}
                bold
                onClick={onClickNovo}
                // disabled={
                //   !permissoesTela.podeIncluir ||
                //   !turmaInfantil ||
                //   !listaComponenteCurriculares ||
                //   !componenteCurricularSelecionado
                // }
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
              />
            </div>
          </div>
          <div className="row mb-4">
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 pr-0">
              <Loader loading={carregandoAnos} tip="">
                <SelectComponent
                  id="ano-letivo"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo?.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 pr-0">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  id="dre"
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!anoLetivo || listaDres?.length === 1}
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5">
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
            <div className="col-sm-12 col-md-6 pr-0">
              <Loader loading={carregandoTurmas} tip="">
                <SelectComponent
                  id="turma (Regular)"
                  lista={listaTurmas}
                  valueOption="codigo"
                  valueText="modalidadeTurmaNome"
                  label="Turma"
                  disabled={listaTurmas?.length === 1}
                  valueSelect={turmaId}
                  onChange={onChangeTurma}
                  placeholder="Turma"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 p-0">
              <LocalizadorEstudante
                id="estudante"
                showLabel
                ueId={ueId}
                onChange={onChangeLocalizadorEstudante}
                anoLetivo={anoLetivo}
                desabilitado={!dreId || !ueId}
                exibirCodigoEOL={false}
                codigoTurma={turmaId}
                placeholder="Procure pelo nome da Criança/Estudante"
                labelAlunoNome="Crianças/Estudantes"
              />
            </div>
          </div>
          <div className="row mb-4">
            <div className="col-sm-12 col-md-6 pr-0">
              <Loader loading={carregandoSituacao} tip="">
                <SelectComponent
                  id="situacao"
                  label="Situação"
                  lista={listaSituacao}
                  valueOption="codigo"
                  valueText="descricao"
                  disabled={listaSituacao?.length === 1}
                  onChange={onChangeSituacao}
                  valueSelect={situacao}
                  placeholder="Situação"
                />
              </Loader>
            </div>
            <div className="col-sm-3 col-md-3 pr-0">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataInicial"
                valor={dataInicial}
                onChange={mudarDataInicial}
                placeholder="Data inícial"
                label="Data da visita"
              />
            </div>
            <div className="col-sm-3 col-md-3 pt-4">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataFinal"
                valor={dataFinal}
                onChange={mudarDataFinal}
                placeholder="Data final"
              />
            </div>
          </div>
          <div className="row mb-4">
            {anoLetivo && dreId && listaDres?.length && (
              <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                <ListaPaginada
                  url="v1/encaminhamento-aee"
                  id="lista-alunos"
                  colunas={colunas}
                  filtro={filtro}
                  filtroEhValido={
                    !!(anoLetivo && dreId && filtro.dreId && listaDres?.length)
                  }
                  temPaginacao
                  onClick={onClickEditar}
                />
              </div>
            )}
          </div>
        </div>
      </Card>
    </>
  );
};

export default RegistroItineranciaAEELista;
