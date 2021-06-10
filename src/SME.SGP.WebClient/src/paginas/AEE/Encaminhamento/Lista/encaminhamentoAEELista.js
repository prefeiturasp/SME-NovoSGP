import React, { useCallback, useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  CheckboxComponent,
  ListaPaginada,
  Loader,
  SelectComponent,
} from '~/componentes';
import { Cabecalho, NomeEstudanteLista } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import LocalizadorEstudante from '~/componentes/LocalizadorEstudante';
import { URL_HOME } from '~/constantes/url';
import { RotasDto } from '~/dtos';
import { setDadosIniciaisLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import { verificaSomenteConsulta } from '~/servicos';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import FiltroHelper from '~componentes-sgp/filtro/helper';
import ModalAvisoNovoEncaminhamentoAEE from './Componentes/AvisoCadastro/modalAvisoCadastro';

const EncaminhamentoAEELista = () => {
  const dispatch = useDispatch();

  const codigosAlunosSelecionados = useSelector(
    state => state.localizadorEstudante.codigosAluno
  );

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.RELATORIO_AEE_ENCAMINHAMENTO];

  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);

  const [consideraHistorico, setConsideraHistorico] = useState(false);
  const [anoAtual] = useState(window.moment().format('YYYY'));

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [listaSituacao, setListaSituacao] = useState([]);
  const [listaResponsavel, setListaResponsavel] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState();
  const [dre, setDre] = useState();
  const [ue, setUe] = useState();
  const [turma, setTurma] = useState();
  const [situacao, setSituacao] = useState();
  const [responsavel, setResponsavel] = useState();

  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState();

  const [filtro, setFiltro] = useState({});

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [carregandoSituacao, setCarregandoSituacao] = useState(false);
  const [carregandoResponsavel, setCarregandoResponsavel] = useState(false);

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  useEffect(() => {
    if (codigosAlunosSelecionados?.length > 0) {
      setTurma();
    }
  }, [codigosAlunosSelecionados]);

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
      title: 'Responsável',
      dataIndex: 'responsavel',
    },
    {
      title: 'Situação',
      dataIndex: 'situacao',
    },
  ];

  const filtrar = (dreId, ueId, turmaId, aluno, situa, responsa) => {
    if (anoLetivo && dreId) {
      const params = {
        dreId,
        ueId,
        turmaId,
        alunoCodigo: aluno,
        situacao: situa,
        responsavelRf: responsa,
        anoLetivo,
      };
      setFiltro({ ...params });
    }
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

  const obterSituacoes = useCallback(async () => {
    setCarregandoSituacao(true);
    const resposta = await ServicoEncaminhamentoAEE.obterSituacoes()
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

  const obterResponsaveis = useCallback(async () => {
    if (anoLetivo && ue) {
      setCarregandoResponsavel(true);

      const resposta = await ServicoEncaminhamentoAEE.obterResponsaveis(
        dre?.id,
        ue?.id,
        turma?.id,
        alunoLocalizadorSelecionado,
        situacao,
        anoLetivo
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoResponsavel(false));
      if (resposta?.data?.length) {
        const lista = resposta.data.map(item => {
          return { ...item, codigoRf: String(item.codigoRf) };
        });
        setListaResponsavel(lista);
      } else {
        setListaResponsavel([]);
      }
    }
  }, [anoLetivo, dre, ue, turma, alunoLocalizadorSelecionado, situacao]);

  useEffect(() => {
    if (ue?.id && listaUes.length) {
      obterResponsaveis();
    }
  }, [obterResponsaveis, ue, listaUes]);

  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterUes = useCallback(async () => {
    if (anoLetivo && dre?.codigo) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dre.codigo,
        `v1/abrangencias/${consideraHistorico}/dres/${dre.codigo}/ues?anoLetivo=${anoLetivo}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          codigo: String(item.codigo),
          id: item.id,
        }));

        if (lista?.length === 1) {
          setUe(lista[0]);
          filtrar(
            dre?.id,
            lista[0]?.id,
            turma?.id,
            alunoLocalizadorSelecionado,
            situacao,
            responsavel
          );
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, [dre, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (dre) {
      obterUes();
    } else {
      setUe();
      setListaUes([]);
    }
  }, [dre, obterUes]);

  const onChangeDre = valor => {
    if (valor) {
      const dreSelecionada = listaDres.find(
        item => String(item.codigo) === String(valor)
      );

      setDre(dreSelecionada);
    } else {
      setDre();
    }

    dispatch(
      setDadosIniciaisLocalizarEstudante({ ueId: ue?.codigo, dreId: valor })
    );

    setListaUes([]);
    setUe();

    setListaTurmas([]);
    setTurma();
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
            codigo: String(item.codigo),
            abrev: item.abreviacao,
            id: item.id,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDre(lista[0]);
        }
      } else {
        setListaDres([]);
        setDre();
      }
    }
  }, [anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (anoLetivo) {
      obterDres();
    }
  }, [anoLetivo, obterDres]);

  const obterTurmas = useCallback(async () => {
    if (anoLetivo && ue) {
      setCarregandoTurmas(true);
      const resposta = await AbrangenciaServico.buscarTurmas(
        ue?.codigo,
        0,
        '',
        anoLetivo,
        consideraHistorico
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTurmas(false));

      if (resposta?.data?.length) {
        setListaTurmas(resposta.data);

        if (resposta?.data?.length === 1) {
          setTurma(resposta.data[0]);
          filtrar(
            dre?.id,
            ue.id,
            resposta.data[0]?.id,
            alunoLocalizadorSelecionado,
            situacao,
            responsavel
          );
        }
      } else {
        setListaTurmas([]);
      }
    }
  }, [anoLetivo, ue]);

  useEffect(() => {
    if (ue) {
      obterTurmas();
    } else {
      setTurma();
      setListaTurmas([]);
    }
  }, [ue, obterTurmas]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    if (!somenteConsulta && permissoesTela.podeIncluir) {
      ServicoEncaminhamentoAEE.obterAvisoModal();
    }
  };

  const onChangeUe = valor => {
    const ueSelecionada = listaUes.find(
      item => String(item.codigo) === String(valor)
    );

    if (ueSelecionada) {
      setUe(ueSelecionada);
    } else {
      setUe();
    }

    dispatch(
      setDadosIniciaisLocalizarEstudante({ ueId: valor, dreId: dre?.codigo })
    );
    setListaTurmas([]);
    setTurma();

    filtrar(
      dre?.id,
      ueSelecionada?.id,
      turma?.id,
      alunoLocalizadorSelecionado,
      situacao,
      responsavel
    );
  };

  const limparFiltrosSelecionados = () => {
    setAnoLetivo();

    setDre();
    setListaDres([]);

    setUe();
    setListaUes([]);

    setListaTurmas([]);
    setTurma();

    setAlunoLocalizadorSelecionado();

    setSituacao();
  };

  const onChangeAnoLetivo = ano => {
    limparFiltrosSelecionados();
    setAnoLetivo(ano);
  };

  const onChangeTurma = valor => {
    const turmaSelecionada = listaTurmas?.find(
      item => String(item.codigo) === String(valor)
    );

    if (turmaSelecionada) {
      setTurma(turmaSelecionada);
    } else {
      setTurma();
    }

    setAlunoLocalizadorSelecionado();
    filtrar(dre?.id, ue?.id, turmaSelecionada?.id, '', situacao, responsavel);
  };

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno?.alunoCodigo);
      filtrar(
        dre?.id,
        ue?.id,
        turma?.id,
        aluno?.alunoCodigo,
        situacao,
        responsavel
      );
    } else {
      setAlunoLocalizadorSelecionado();
      filtrar(dre?.id, ue?.id, turma?.id, '', situacao, responsavel);
    }
  };

  const onChangeSituacao = valor => {
    setSituacao(valor);
    filtrar(
      dre?.id,
      ue?.id,
      turma?.id,
      alunoLocalizadorSelecionado,
      valor,
      responsavel
    );
  };

  const onChangeResponsavel = valor => {
    setResponsavel(valor);
    filtrar(
      dre?.id,
      ue?.id,
      turma?.id,
      alunoLocalizadorSelecionado,
      situacao,
      valor
    );
  };

  const onClickEditar = item => {
    history.push(`${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}/editar/${item.id}`);
  };

  const onCheckedConsideraHistorico = e => {
    limparFiltrosSelecionados();
    setConsideraHistorico(e.target.checked);
  };

  return (
    <>
      <ModalAvisoNovoEncaminhamentoAEE />
      <Cabecalho pagina="Encaminhamento AEE" />
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
                id="btn-novo-encaminhamento"
                label="Novo Encaminhamento"
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
                  valueOption="codigo"
                  valueText="desc"
                  disabled={!anoLetivo || listaDres?.length === 1}
                  onChange={onChangeDre}
                  valueSelect={dre?.codigo}
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
                  valueOption="codigo"
                  valueText="desc"
                  disabled={!dre?.codigo || listaUes?.length === 1}
                  onChange={onChangeUe}
                  valueSelect={ue?.codigo}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
              <Loader loading={carregandoTurmas} tip="">
                <SelectComponent
                  id="turma"
                  lista={listaTurmas}
                  valueOption="codigo"
                  valueText="modalidadeTurmaNome"
                  label="Turma"
                  disabled={listaTurmas?.length === 1}
                  valueSelect={turma?.codigo}
                  onChange={onChangeTurma}
                  placeholder="Turma"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
              <div className="row">
                <LocalizadorEstudante
                  id="estudante"
                  showLabel
                  ueId={ue?.codigo}
                  onChange={onChangeLocalizadorEstudante}
                  anoLetivo={anoLetivo}
                  desabilitado={!dre?.codigo || !ue?.codigo}
                  exibirCodigoEOL={false}
                  codigoTurma={turma?.codigo}
                  placeholder="Procure pelo nome da Criança/Estudante"
                />
              </div>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
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
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-4">
              <Loader loading={carregandoResponsavel} tip="">
                <SelectComponent
                  id="responsavel"
                  label="Responsável"
                  lista={listaResponsavel}
                  valueOption="codigoRf"
                  valueText="nomeServidor"
                  onChange={onChangeResponsavel}
                  valueSelect={responsavel}
                  placeholder="Responsável"
                />
              </Loader>
            </div>
            {anoLetivo &&
            dre?.codigo &&
            listaDres?.length &&
            ue?.codigo &&
            listaUes?.length ? (
              <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                <ListaPaginada
                  url="v1/encaminhamento-aee"
                  id="lista-alunos"
                  colunas={colunas}
                  filtro={filtro}
                  filtroEhValido={
                    !!(
                      anoLetivo &&
                      dre?.codigo &&
                      filtro.dreId &&
                      listaDres?.length &&
                      ue?.codigo &&
                      filtro.ueId &&
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

export default EncaminhamentoAEELista;
