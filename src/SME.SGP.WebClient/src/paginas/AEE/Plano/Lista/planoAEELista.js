import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Cabecalho, NomeEstudanteLista } from '~/componentes-sgp';
import AbrangenciaServico from '~/servicos/Abrangencia';
import {
  CheckboxComponent,
  ListaPaginada,
  Loader,
  SelectComponent,
} from '~/componentes';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import LocalizadorEstudante from '~/componentes/LocalizadorEstudante';
import { URL_HOME } from '~/constantes/url';
import { RotasDto } from '~/dtos';
import { setDadosIniciaisLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import { erros, verificaSomenteConsulta } from '~/servicos';
import history from '~/servicos/history';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const PlanoAEELista = () => {
  const dispatch = useDispatch();

  const [consideraHistorico, setConsideraHistorico] = useState(false);
  const [filtro, setFiltro] = useState({});

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaSituacao, setListaSituacao] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState();
  const [dreId, setDreId] = useState();
  const [ueId, setUeId] = useState();
  const [turmaId, setTurmaId] = useState();
  const [situacao, setSituacao] = useState();

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [carregandoSituacao, setCarregandoSituacao] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);

  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);

  const colunas = [
    {
      title: 'Nº',
      dataIndex: 'numero',
    },
    {
      title: 'Nome',
      dataIndex: 'nome',
      render: (_, record) => (
        <NomeEstudanteLista
          nome={record?.nome}
          exibirSinalizacao={record?.ehAtendidoAEE}
        />
      ),
    },
    {
      title: 'Turma',
      dataIndex: 'turma',
    },
    {
      title: 'Situação',
      dataIndex: 'situacao',
    },
    {
      title: 'Data de cadastro',
      dataIndex: 'criadoEm',
      render: data => {
        let dataFormatada = '';
        if (data) {
          dataFormatada = moment(data).format('DD/MM/YYYY HH:mm');
        }
        return <span> {dataFormatada}</span>;
      },
    },
    {
      title: 'Versão',
      dataIndex: 'versao',
    },
  ];

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
      } else {
        setListaUes([]);
      }
    }
  }, [dreId, anoLetivo, consideraHistorico]);

  const filtrar = (dre, ue, turma, aluno, situa) => {
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
      };
      setFiltro({ ...params });
    }
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
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    if (!somenteConsulta && permissoesTela.podeIncluir) {
      history.push(`${RotasDto.RELATORIO_AEE_PLANO}/novo`);
    }
  };

  const onCheckedConsideraHistorico = e => {
    limparFiltrosSelecionados();
    setConsideraHistorico(e.target.checked);
  };

  const validarValorPadraoAnoLetivo = lista => {
    if (lista?.length) {
      const temAnoAtualNaLista = lista.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) {
        setAnoLetivo(anoAtual);
      } else {
        setAnoLetivo(lista[0].valor);
      }
    } else {
      setAnoLetivo();
    }
  };

  useEffect(() => {
    validarValorPadraoAnoLetivo(listaAnosLetivo);
  }, [consideraHistorico, listaAnosLetivo]);

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);

    const anosLetivos = await FiltroHelper.obterAnosLetivos({
      consideraHistorico,
    });

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    validarValorPadraoAnoLetivo(anosLetivos);

    setListaAnosLetivo(anosLetivos);
    setCarregandoAnos(false);
  }, [anoAtual, consideraHistorico]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos, consideraHistorico]);

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    limparFiltrosSelecionados();
  };

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
      } else {
        setListaDres([]);
        setDreId(undefined);
      }
    }
  }, [anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (dreId) {
      obterUes();
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, obterUes]);

  const onChangeDre = dre => {
    setDreId(dre);
    dispatch(setDadosIniciaisLocalizarEstudante({ ueId, dreId: dre }));

    setListaUes([]);
    setUeId();

    setListaTurmas([]);
    setTurmaId();
  };

  useEffect(() => {
    if (anoLetivo) {
      obterDres();
    }
  }, [anoLetivo, obterDres]);

  const onChangeUe = ue => {
    setTurmaId();
    setListaTurmas([]);

    setUeId(ue);
    dispatch(setDadosIniciaisLocalizarEstudante({ ueId: ue, dreId }));
  };

  const onChangeTurma = valor => {
    setTurmaId(valor);
    setAlunoLocalizadorSelecionado();
  };

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno?.alunoCodigo);
    } else {
      setAlunoLocalizadorSelecionado();
    }
  };

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
  }, [anoLetivo, ueId]);

  useEffect(() => {
    if (ueId) {
      obterTurmas();
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [ueId, obterTurmas]);

  const obterSituacoes = useCallback(async () => {
    setCarregandoSituacao(true);
    const resposta = await ServicoPlanoAEE.obterSituacoes()
      .catch(e => erros(e))
      .finally(() => setCarregandoSituacao(false));
    if (resposta?.data?.length) {
      setListaSituacao(resposta.data);
    } else {
      setListaSituacao([]);
    }
  }, []);

  useEffect(() => {
    obterSituacoes();
  }, [obterSituacoes]);

  const onChangeSituacao = valor => {
    setSituacao(valor);
  };

  const onClickEditar = item => {
    history.push(`${RotasDto.RELATORIO_AEE_PLANO}/editar/${item.id}`);
  };

  useEffect(() => {
    if (dreId && ueId && listaDres?.length && listaUes?.length) {
      filtrar(dreId, ueId, turmaId, alunoLocalizadorSelecionado, situacao);
    }
  }, [
    ueId,
    listaDres,
    listaUes,
    turmaId,
    alunoLocalizadorSelecionado,
    situacao,
  ]);

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  return (
    <>
      <Cabecalho pagina="Plano AEE" />
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
                onClick={onClickVoltar}
              />
              <Button
                id="btn-novo"
                label="Novo"
                color={Colors.Roxo}
                border
                bold
                onClick={onClickNovo}
                disabled={somenteConsulta || !permissoesTela.podeIncluir}
              />
            </div>
            <div className="col-sm-12 mb-4">
              <CheckboxComponent
                id="exibir-historico"
                label="Exibir histórico?"
                onChangeCheckbox={onCheckedConsideraHistorico}
                checked={consideraHistorico}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
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
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-2">
              <Loader loading={carregandoTurmas} tip="">
                <SelectComponent
                  id="turma"
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
            <div className="col-sm-12 col-md-8 col-lg-6 col-xl-6 mb-2">
              <div className="row">
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
                />
              </div>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
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
            {anoLetivo &&
            dreId &&
            listaDres?.length &&
            ueId &&
            listaUes?.length ? (
              <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                <ListaPaginada
                  url="v1/plano-aee"
                  id="lista-alunos"
                  colunas={colunas}
                  filtro={filtro}
                  filtroEhValido={
                    !!(
                      anoLetivo &&
                      dreId &&
                      filtro.dreId &&
                      listaDres?.length &&
                      filtro.ueId &&
                      ueId &&
                      listaUes?.length
                    )
                  }
                  temPaginacao
                  onClick={onClickEditar}
                />
              </div>
            ) : (
              ''
            )}
          </div>
        </div>
      </Card>
    </>
  );
};

export default PlanoAEELista;
