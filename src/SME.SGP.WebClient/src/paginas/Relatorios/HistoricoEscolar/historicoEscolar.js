import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { SelectComponent, ListaPaginada, Loader } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import AbrangenciaServico from '~/servicos/Abrangencia';
import api from '~/servicos/api';
import history from '~/servicos/history';
import FiltroHelper from '~componentes-sgp/filtro/helper';
import { sucesso } from '~/servicos/alertas';
import LocalizadorEstudante from '~/componentes/LocalizadorEstudante';
import ServicoHistoricoEscolar from '~/servicos/Paginas/HistoricoEscolar/ServicoHistoricoEscolar';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import RotasDto from '~/dtos/rotasDto';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';

const HistoricoEscolar = () => {
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const codigosAlunosSelecionados = useSelector(
    state => state.localizadorEstudante.codigosAluno
  );

  useEffect(() => {
    setSomenteConsulta(
      verificaSomenteConsulta(permissoesTela[RotasDto.HISTORICO_ESCOLAR])
    );
  }, [permissoesTela]);

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaSemestre, setListaSemestre] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);
  const [estudanteOpt, setEstudanteOpt] = useState(undefined);
  const [imprimirDadosResp, setImprimirDadosResp] = useState('0');
  const [preencherDataImpressao, setPreencherDataImpressao] = useState('0');

  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);
  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState();

  const [alunosSelecionados, setAlunosSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});

  const vaidaDesabilitarBtnGerar = useCallback(
    desabilitar => {
      if (String(modalidadeId) === String(modalidade.INFANTIL)) {
        setDesabilitarBtnGerar(true);
      } else {
        setDesabilitarBtnGerar(desabilitar);
      }
    },
    [modalidadeId]
  );

  useEffect(() => {
    if (codigosAlunosSelecionados?.length > 0) {
      setAlunosSelecionados([]);
      setEstudanteOpt('0');
      setTurmaId();
    }
  }, [codigosAlunosSelecionados]);

  const listaEstudanteOpt = [
    { valor: '0', desc: 'Todos' },
    { valor: '1', desc: 'Estudantes selecionados' },
  ];

  const listaSimNao = [
    { valor: '0', desc: 'Sim' },
    { valor: '1', desc: 'Não' },
  ];

  const colunas = [
    {
      title: 'Número',
      dataIndex: 'numeroChamada',
    },
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
  ];

  const [carregandoAnos, setCarregandoAnos] = useState(false);

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);
    let anosLetivo = [];

    const anosLetivoComHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: true,
    });
    const anosLetivoSemHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: false,
    });

    anosLetivo = anosLetivoComHistorico.concat(anosLetivoSemHistorico);

    if (!anosLetivo.length) {
      anosLetivo.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    if (anosLetivo && anosLetivo.length) {
      const temAnoAtualNaLista = anosLetivo.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) setAnoLetivo(anoAtual);
      else setAnoLetivo(anosLetivo[0].valor);
    }

    setListaAnosLetivo(anosLetivo);
    setCarregandoAnos(false);
  }, [anoAtual]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const [carregandoModalidades, setCarregandoModalidades] = useState(false);

  const obterModalidades = async (ue, ano) => {
    if (ue && ano) {
      setCarregandoModalidades(true);
      const { data } = await api.get(`/v1/ues/${ue}/modalidades?ano=${ano}`);
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.id),
        }));

        if (lista && lista.length && lista.length === 1) {
          setModalidadeId(lista[0].valor);
        }
        setListaModalidades(lista);
      }
      setCarregandoModalidades(false);
    }
  };

  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterUes = useCallback(async (dre, ano) => {
    if (dre) {
      setCarregandoUes(true);
      const { data } = await AbrangenciaServico.buscarUes(
        dre,
        `v1/abrangencias/false/dres/${dre}/ues?anoLetivo=${ano}`,
        true
      );
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
      setCarregandoUes(false);
    }
  }, []);

  const onChangeDre = dre => {
    setDreId(dre);

    setListaUes([]);
    setUeId();

    setListaModalidades([]);
    setModalidadeId();

    setListaSemestre([]);
    setSemestre();

    setListaTurmas([]);
    setTurmaId();
  };

  const [carregandoDres, setCarregandoDres] = useState(false);

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const { data } = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`
      );
      if (data && data.length) {
        const lista = data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
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
      setCarregandoDres(false);
    }
  }, [anoLetivo]);

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);

  const obterTurmas = useCallback(async (modalidadeSelecionada, ue, ano) => {
    if (ue && modalidadeSelecionada) {
      setCarregandoTurmas(true);
      const { data } = await AbrangenciaServico.buscarTurmas(
        ue,
        modalidadeSelecionada,
        '',
        ano
      );
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: item.codigo,
        }));
        setListaTurmas(lista);

        if (lista && lista.length && lista.length === 1) {
          setTurmaId(lista[0].valor);
        }
      }
      setCarregandoTurmas(false);
    }
  }, []);

  const [carregandoSemestres, setCarregandoSemestres] = useState(false);

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
      setListaSemestre(lista);
    }
    setCarregandoSemestres(false);
  };

  useEffect(() => {
    if (anoLetivo && ueId) {
      obterModalidades(ueId, anoLetivo);
    } else {
      setModalidadeId();
      setListaModalidades([]);
    }
  }, [anoLetivo, ueId]);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId, anoLetivo);
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, anoLetivo, obterUes]);

  useEffect(() => {
    if (modalidadeId && ueId && anoLetivo) {
      obterTurmas(modalidadeId, ueId, anoLetivo);
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [modalidadeId, ueId, anoLetivo, obterTurmas]);

  useEffect(() => {
    if (modalidadeId && anoLetivo) {
      if (String(modalidadeId) === String(modalidade.EJA)) {
        obterSemestres(modalidadeId, anoLetivo);
      } else {
        setSemestre();
        setListaSemestre([]);
      }
    } else {
      setSemestre();
      setListaSemestre([]);
    }
  }, [modalidadeId, anoLetivo, obterTurmas]);

  useEffect(() => {
    const desabilitar =
      !alunoLocalizadorSelecionado &&
      (!codigosAlunosSelecionados || codigosAlunosSelecionados?.length === 0) &&
      (!anoLetivo || !dreId || !ueId || !modalidadeId || !turmaId);

    if (String(modalidadeId) === String(modalidade.EJA)) {
      vaidaDesabilitarBtnGerar(!semestre || desabilitar);
    } else {
      vaidaDesabilitarBtnGerar(desabilitar);
    }
  }, [
    alunoLocalizadorSelecionado,
    codigosAlunosSelecionados,
    anoLetivo,
    dreId,
    ueId,
    modalidadeId,
    turmaId,
    semestre,
    vaidaDesabilitarBtnGerar,
  ]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo();
    setDreId();
    setListaAnosLetivo([]);
    setListaDres([]);

    setEstudanteOpt();

    obterAnosLetivos();
  };

  const gerarHistorico = async params => {
    const requisicao = await ServicoHistoricoEscolar.gerar(params);
    return requisicao.status === 200 && requisicao.data;
  };

  const [carregandoGerar, setCarregandoGerar] = useState(false);

  const onClickGerar = () => {
    setCarregandoGerar(true);

    const params = {
      anoLetivo,
      dreCodigo: dreId,
      ueCodigo: ueId,
      modalidade: modalidadeId,
      semestre,
      turmaCodigo: turmaId,
      imprimirDadosResponsaveis: imprimirDadosResp === '0',
      preencherDataImpressao: preencherDataImpressao === '0',
      alunosCodigo:
        codigosAlunosSelecionados?.length > 0
          ? codigosAlunosSelecionados
          : alunosSelecionados,
    };

    if (gerarHistorico(params)) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado'
      );
    }

    setCarregandoGerar(false);
  };

  const onChangeUe = ue => {
    setUeId(ue);

    setListaModalidades([]);
    setModalidadeId();

    setListaSemestre([]);
    setSemestre();

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeModalidade = novaModalidade => {
    setModalidadeId(novaModalidade);

    setListaSemestre([]);
    setSemestre();

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    setListaModalidades([]);
    setModalidadeId();

    setListaSemestre([]);
    setSemestre();

    setListaTurmas([]);
    setTurmaId();
  };

  const onChangeSemestre = valor => setSemestre(valor);
  const onChangeTurma = valor => {
    setTurmaId(valor);
    setEstudanteOpt('0');
  };

  const onChangeEstudanteOpt = valor => {
    if (valor === '1') {
      setFiltro({
        anoLetivo,
        modalidade: modalidadeId,
        dreCodigo: dreId,
        ueCodigo: ueId,
        turmaCodigo: turmaId,
        semestre,
      });
    } else {
      setAlunosSelecionados([]);
    }
    setEstudanteOpt(valor);
  };

  const onChangeImprimirDadosResp = valor => setImprimirDadosResp(valor);
  const onChangePreencherDataImpressao = valor =>
    setPreencherDataImpressao(valor);

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno);
      setModalidadeId();
      setTurmaId();
      vaidaDesabilitarBtnGerar(false);
    } else {
      setAlunoLocalizadorSelecionado();
      if (listaModalidades && listaModalidades.length === 1)
        setModalidadeId(String(listaModalidades[0].valor));
    }
  };

  const onSelecionarItems = items => {
    setAlunosSelecionados([...items.map(item => String(item.codigo))]);
  };

  return (
    <>
      <AlertaModalidadeInfantil
        exibir={String(modalidadeId) === String(modalidade.INFANTIL)}
        validarModalidadeFiltroPrincipal={false}
      />
      <Cabecalho pagina="Histórico Escolar" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
              <Button
                id="btn-voltar-historico-escolar"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                id="btn-cancelar-historico-escolar"
                label="Cancelar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={() => onClickCancelar()}
              />
              <Loader
                loading={carregandoGerar}
                className="d-flex w-auto"
                tip=""
              >
                <Button
                  id="btn-gerar-historico-escolar"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  className="mr-0"
                  onClick={() => onClickGerar()}
                  disabled={desabilitarBtnGerar || somenteConsulta}
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoAnos} tip="">
                <SelectComponent
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    (listaAnosLetivo && listaAnosLetivo.length === 1) ||
                    codigosAlunosSelecionados?.length > 0
                  }
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!anoLetivo || (listaDres && listaDres.length === 1)}
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!dreId || (listaUes && listaUes.length === 1)}
                  onChange={onChangeUe}
                  valueSelect={ueId}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              <div className="row">
                <LocalizadorEstudante
                  showLabel
                  ueId={ueId}
                  onChange={onChangeLocalizadorEstudante}
                  anoLetivo={anoLetivo}
                  desabilitado={!dreId || !ueId}
                />
              </div>
            </div>
            <div
              className={`"col-sm-12 col-md-6 ${
                modalidadeId && String(modalidadeId) === String(modalidade.EJA)
                  ? `col-lg-3 col-xl-3`
                  : `col-lg-4 col-xl-4`
              } mb-2"`}
            >
              <Loader loading={carregandoModalidades} tip="">
                <SelectComponent
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !ueId ||
                    alunoLocalizadorSelecionado?.length ||
                    (listaModalidades && listaModalidades.length === 1) ||
                    codigosAlunosSelecionados?.length > 0
                  }
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Modalidade"
                />
              </Loader>
            </div>
            {String(modalidadeId) === String(modalidade.EJA) ? (
              <div className="col-sm-12 col-md-12 col-lg-3 col-xl-3 mb-2">
                <Loader loading={carregandoSemestres} tip="">
                  <SelectComponent
                    lista={listaSemestre}
                    valueOption="valor"
                    valueText="desc"
                    label="Semestre"
                    disabled={
                      !modalidadeId ||
                      (listaSemestre && listaSemestre.length === 1) ||
                      String(modalidadeId) === String(modalidade.FUNDAMENTAL)
                    }
                    valueSelect={semestre}
                    onChange={onChangeSemestre}
                    placeholder="Semestre"
                  />
                </Loader>
              </div>
            ) : null}
            <div
              className={`"col-sm-12 col-md-6 ${
                modalidadeId && String(modalidadeId) === String(modalidade.EJA)
                  ? `col-lg-3 col-xl-3`
                  : `col-lg-4 col-xl-4`
              } mb-2"`}
            >
              <Loader loading={carregandoTurmas} tip="">
                <SelectComponent
                  lista={listaTurmas}
                  valueOption="valor"
                  valueText="desc"
                  label="Turma"
                  disabled={
                    !modalidadeId ||
                    alunoLocalizadorSelecionado?.length ||
                    (listaTurmas && listaTurmas.length === 1) ||
                    codigosAlunosSelecionados?.length > 0
                  }
                  valueSelect={turmaId}
                  onChange={onChangeTurma}
                  placeholder="Turma"
                />
              </Loader>
            </div>
            <div
              className={`"col-sm-12 col-md-6 ${
                modalidadeId && String(modalidadeId) === String(modalidade.EJA)
                  ? `col-lg-3 col-xl-3`
                  : `col-lg-4 col-xl-4`
              } mb-2"`}
            >
              <SelectComponent
                label="Estudantes"
                lista={listaEstudanteOpt}
                valueOption="valor"
                valueText="desc"
                valueSelect={estudanteOpt}
                onChange={onChangeEstudanteOpt}
                placeholder="Estudantes"
                disabled={
                  !turmaId ||
                  alunoLocalizadorSelecionado ||
                  codigosAlunosSelecionados?.length > 0
                }
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                label="Imprimir dados dos responsáveis"
                lista={listaSimNao}
                valueOption="valor"
                valueText="desc"
                valueSelect={imprimirDadosResp}
                onChange={onChangeImprimirDadosResp}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                label="Preencher a data de impressão"
                lista={listaSimNao}
                valueOption="valor"
                valueText="desc"
                valueSelect={preencherDataImpressao}
                onChange={onChangePreencherDataImpressao}
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              {estudanteOpt === '1' ? (
                <ListaPaginada
                  url="v1/boletim/alunos"
                  id="lista-alunos-historico-escolar"
                  idLinha="codigo"
                  colunaChave="codigo"
                  colunas={colunas}
                  filtro={filtro}
                  multiSelecao
                  temPaginacao={false}
                  selecionarItems={onSelecionarItems}
                />
              ) : null}
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default HistoricoEscolar;
