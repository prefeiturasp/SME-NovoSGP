import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { SelectComponent, CheckboxComponent, Loader } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import RotasDto from '~/dtos/rotasDto';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoConselhoAtaFinal from '~/servicos/Paginas/ConselhoAtaFinal/ServicoConselhoAtaFinal';
import FiltroHelper from '~componentes-sgp/filtro/helper';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';

const AtaFinalResultados = () => {
  const usuarioStore = useSelector(store => store.usuario);
  const permissoesTela = usuarioStore.permissoes[RotasDto.ATA_FINAL_RESULTADOS];

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
  const [formato, setFormato] = useState('1');
  const [consideraHistorico, setConsideraHistorico] = useState(false);

  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);
  const [desabilitarBtnFormato, setDesabilitarBtnFormato] = useState(true);
  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaVisualizacao, setListaVisualizacao] = useState([]);
  const [desabilitaVisualizacao, setDesabilitaVisualizacao] = useState(true);
  const [visualizacao, setVisualizacao] = useState('');
  const [listaTurmasCompletas, setListaTurmasCompletas] = useState([]);

  const listaFormatos = [
    { valor: '1', desc: 'PDF' },
    { valor: '4', desc: 'EXCEL' },
  ];

  const obterAnosLetivos = useCallback(async consideraHistorico => {
    setCarregandoAnosLetivos(true);
    const anosLetivo = await FiltroHelper.obterAnosLetivos({
      consideraHistorico,
    }).catch(e => erros(e));
    if (anosLetivo) {
      setListaAnosLetivo(anosLetivo);
      setAnoLetivo(anosLetivo[0].valor);
      setDreId();
    } else {
      setListaAnosLetivo([]);
    }
    setCarregandoAnosLetivos(false);
  }, []);

  const obterModalidades = useCallback(
    async (ue, ano) => {
      if (ue && ano) {
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
      }
    },
    [ueId]
  );

  const obterVisualizacoes = useCallback(async () => {
    const { data } = await api.get(
      `/v1/relatorios/filtros/ata-final/tipos-visualizacao`
    );
    if (data) {
      const lista = data.map(item => ({
        desc: item.desc,
        valor: item.valor,
      }));
      setListaVisualizacao(lista);
    }
  }, []);

  const obterUes = useCallback(
    async dre => {
      setCarregandoUes(true);
      if (dre) {
        const { data } = await AbrangenciaServico.buscarUes(
          dre,
          '',
          false,
          undefined,
          consideraHistorico
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
      }
      setCarregandoUes(false);
    },
    [dreId]
  );

  const onChangeDre = dre => {
    setDreId(dre);

    setListaUes([]);
    setUeId(undefined);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId(undefined);
  };

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const { data } = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivo}`,
        consideraHistorico
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
      }
      setCarregandoDres(false);
    }
  }, [anoLetivo]);

  const obterTurmas = useCallback(
    async (modalidadeSelecionada, ue) => {
      if (ue && modalidadeSelecionada) {
        const { data } = await AbrangenciaServico.buscarTurmas(
          ue,
          modalidadeSelecionada,
          '',
          anoLetivo,
          consideraHistorico,
          false,
          [1, 7]
        );
        if (data) {
          setListaTurmasCompletas(data);
          const lista = data.map(item => ({
            desc: item.nome,
            valor: item.codigo,
          }));

          lista.unshift({ desc: 'Todas', valor: '-99' });

          setListaTurmas(lista);

          if (lista && lista.length && lista.length === 1) {
            setTurmaId(lista[0].valor);
          }
        } else {
          setListaTurmasCompletas([]);
        }
      }
    },
    [modalidadeId]
  );

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
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
  };

  useEffect(() => {
    if (anoLetivo && ueId) {
      obterModalidades(ueId, anoLetivo);
    } else {
      setModalidadeId(undefined);
      setListaModalidades([]);
    }
  }, [ueId]);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId);
    } else {
      setUeId(undefined);
      setListaUes([]);
    }
  }, [dreId, obterUes]);

  useEffect(() => {
    if (modalidadeId && ueId) {
      obterTurmas(modalidadeId, ueId);
    } else {
      setTurmaId(undefined);
      setListaTurmas([]);
    }
  }, [modalidadeId, ueId, obterTurmas]);

  useEffect(() => {
    if (modalidadeId && anoLetivo) {
      if (modalidadeId === modalidade.EJA) {
        obterSemestres(modalidadeId, anoLetivo);
      } else {
        setSemestre(undefined);
        setListaSemestre([]);
      }
    } else {
      setSemestre(undefined);
      setListaSemestre([]);
    }
  }, [modalidadeId, anoLetivo, obterTurmas]);

  useEffect(() => {
    const desabilitar =
      !anoLetivo ||
      !dreId ||
      !ueId ||
      !modalidadeId ||
      !turmaId?.length ||
      !formato;

    let desabilitado = desabilitar;

    if (Number(modalidadeId) === Number(modalidade.EJA)) {
      desabilitado = semestre || desabilitar;
    } else if (Number(modalidadeId) === Number(modalidade.ENSINO_MEDIO)) {
      desabilitado = desabilitar || !visualizacao;
    }
    setDesabilitarBtnGerar(desabilitado);
  }, [
    anoLetivo,
    dreId,
    ueId,
    modalidadeId,
    turmaId,
    formato,
    semestre,
    visualizacao,
  ]);

  useEffect(() => {
    obterAnosLetivos(consideraHistorico);
  }, [obterAnosLetivos, consideraHistorico]);

  useEffect(() => {
    obterDres();
    obterVisualizacoes();
  }, [obterDres, obterVisualizacoes]);

  const compararTurmaAno = (stringComparacao, valorComparacao, arrayTurmas) => {
    return listaTurmasCompletas.filter(
      turmaCompleta =>
        arrayTurmas.find(arrayTurma => turmaCompleta.codigo === arrayTurma) &&
        parseInt(turmaCompleta[stringComparacao], 10) ===
          parseInt(valorComparacao, 10)
    );
  };

  const checarTipoTurma = arrayTurmas => {
    setVisualizacao('');
    const turmaItinerancia = compararTurmaAno('tipoTurma', 7, arrayTurmas);
    const turmaPrimeiro = compararTurmaAno('ano', 1, arrayTurmas);
    const turmaSegundo = compararTurmaAno('ano', 2, arrayTurmas);
    const turmaTerceiro = compararTurmaAno('ano', 3, arrayTurmas);
    const ehModalidadeMedio =
      parseInt(modalidadeId, 10) === parseInt(modalidade.ENSINO_MEDIO, 10);

    if (
      turmaItinerancia.length === arrayTurmas?.length ||
      (ehModalidadeMedio &&
        !turmaSegundo.length &&
        (turmaPrimeiro.length || turmaTerceiro.length))
    ) {
      setVisualizacao(
        `${
          listaVisualizacao.find(a => a.desc?.toUpperCase() === 'TURMA')?.valor
        }`
      );
      return true;
    }
    if (!turmaSegundo.length) {
      return true;
    }
    return false;
  };

  useEffect(() => {
    let turmaExcecao = false;
    if (turmaId?.length && turmaId[0] !== '-99') {
      turmaExcecao = checarTipoTurma(turmaId);
    }
    const desabilita =
      !modalidadeId ||
      !turmaId ||
      (turmaId.length === 1 && turmaId[0] !== '-99' && turmaExcecao) ||
      parseInt(modalidadeId, 10) !== parseInt(modalidade.ENSINO_MEDIO, 10) ||
      !turmaId.length ||
      turmaExcecao;
    setDesabilitaVisualizacao(desabilita);
  }, [turmaId, modalidadeId, listaTurmasCompletas]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo(undefined);
    setDreId(undefined);
    setListaAnosLetivo([]);
    setListaDres([]);

    obterAnosLetivos();
    obterDres();

    setFormato('PDF');
  };

  const onClickGerar = async () => {
    if (permissoesTela.podeConsultar) {
      const params = {
        turmasCodigos: [],
        anoLetivo,
        tipoFormatoRelatorio: formato,
        visualizacao,
      };
      if (turmaId.find(t => t === '-99')) {
        params.turmasCodigos = listaTurmas.map(item => String(item.valor));
      } else {
        params.turmasCodigos = turmaId;
      }
      const retorno = await ServicoConselhoAtaFinal.gerar(params).catch(e =>
        erros(e)
      );
      if (retorno && retorno.status === 200) {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
        setDesabilitarBtnGerar(true);
      }
    }
  };

  const onChangeUe = ue => {
    setUeId(ue);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId(undefined);
  };

  const onChangeModalidade = novaModalidade => {
    setModalidadeId(novaModalidade);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId(undefined);
    setVisualizacao('');
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);
    setDreId();

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId(undefined);
  };

  const resetFormato = valor => {
    if (valor) {
      setFormato('1');
    }
  };

  const habilitarSelecaoFormato = valor => {
    const turmaSelecionada = listaTurmas?.find(item => item.valor === valor);
    const ehDesabilitado = turmaSelecionada?.desc === 'Todas';
    setDesabilitarBtnFormato(ehDesabilitado);
    resetFormato(ehDesabilitado);
  };

  const onChangeSemestre = valor => setSemestre(valor);

  const onChangeTurma = valor => {
    const todosSetado = turmaId?.find(a => a === '-99');
    const todos = valor.find(a => a === '-99' && !todosSetado);
    const novoValor = todosSetado && valor.length === 2 ? [valor[1]] : valor;
    setTurmaId(todos ? [todos] : novoValor);
    habilitarSelecaoFormato(valor);
    setVisualizacao('');
  };
  const onChangeFormato = valor => setFormato(valor);

  const onChangeVisualizacao = valor => {
    if (visualizacao !== valor) setVisualizacao(valor);
  };

  function onCheckedConsideraHistorico(e) {
    setConsideraHistorico(e.target.checked);
  }

  return (
    <>
      <AlertaModalidadeInfantil
        exibir={String(modalidadeId) === String(modalidade.INFANTIL)}
        validarModalidadeFiltroPrincipal={false}
      />
      <Cabecalho pagina="Ata de resultados finais" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <Button
                id="btn-voltar-ata-final-resultado"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                id="btn-cancelar-ata-final-resultado"
                label="Cancelar"
                color={Colors.Roxo}
                border
                bold
                className="mr-3"
                onClick={() => onClickCancelar()}
              />
              <Button
                id="btn-gerar-ata-final-resultado"
                icon="print"
                label="Gerar"
                color={Colors.Azul}
                border
                bold
                className="mr-2"
                onClick={() => onClickGerar()}
                disabled={
                  String(modalidadeId) === String(modalidade.INFANTIL) ||
                  desabilitarBtnGerar ||
                  !permissoesTela.podeConsultar
                }
              />
            </div>
            <div className="col-sm-12 mb-4">
              <CheckboxComponent
                label="Exibir histórico?"
                onChangeCheckbox={onCheckedConsideraHistorico}
                checked={consideraHistorico}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoAnosLetivos} tip="">
                <SelectComponent
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !permissoesTela.podeConsultar ||
                    (listaAnosLetivo && listaAnosLetivo.length === 1)
                  }
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-5 col-xl-5 mb-2">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !permissoesTela.podeConsultar ||
                    (listaDres && listaDres.length === 1) ||
                    !anoLetivo
                  }
                  onChange={onChangeDre}
                  valueSelect={dreId}
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-5 col-xl-5 mb-2">
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    !permissoesTela.podeConsultar ||
                    (listaUes && listaUes.length === 1) ||
                    !dreId
                  }
                  onChange={onChangeUe}
                  valueSelect={ueId}
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-5 col-xl-3 mb-2">
              <SelectComponent
                label="Modalidade"
                lista={listaModalidades}
                valueOption="valor"
                valueText="desc"
                disabled={
                  !permissoesTela.podeConsultar ||
                  (listaModalidades && listaModalidades.length === 1) ||
                  !ueId
                }
                onChange={onChangeModalidade}
                valueSelect={modalidadeId}
              />
            </div>
            <div className="col-sm-12 col-md-3 col-lg-2 col-xl-2 mb-2">
              <SelectComponent
                lista={listaSemestre}
                valueOption="valor"
                valueText="desc"
                label="Semestre"
                disabled={
                  !permissoesTela.podeConsultar ||
                  !modalidadeId ||
                  modalidadeId !== modalidade.EJA ||
                  (listaSemestre && listaSemestre.length === 1)
                }
                valueSelect={semestre}
                onChange={onChangeSemestre}
              />
            </div>
            <div className="col-sm-12 col-md-3 col-lg-5 col-xl-2 mb-2">
              <SelectComponent
                lista={listaTurmas}
                valueOption="valor"
                valueText="desc"
                label="Turma"
                disabled={
                  !permissoesTela.podeConsultar ||
                  (listaTurmas && listaTurmas.length === 1) ||
                  !modalidadeId
                }
                valueSelect={turmaId}
                onChange={onChangeTurma}
                multiple
              />
            </div>
            <div className="col-sm-12 col-md-3 col-lg-5 col-xl-3 mb-2">
              <SelectComponent
                label="Visualização"
                lista={listaVisualizacao}
                valueOption="valor"
                valueText="desc"
                valueSelect={visualizacao}
                onChange={onChangeVisualizacao}
                disabled={desabilitaVisualizacao}
              />
            </div>
            <div className="col-sm-12 col-md-3 col-lg-2 col-xl-2 mb-2">
              <SelectComponent
                label="Formato"
                lista={listaFormatos}
                valueOption="valor"
                valueText="desc"
                valueSelect={formato}
                onChange={onChangeFormato}
                disabled={desabilitarBtnFormato}
              />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default AtaFinalResultados;
