import React, { useCallback, useEffect, useState } from 'react';
import { SelectComponent, ListaPaginada } from '~/componentes';
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

const HistoricoEscolar = () => {
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
  const [alunoLocalizadorSelect, setAlunoLocalizadorSelect] = useState();

  const [alunosSelecionados, setAlunosSelecionados] = useState([]);
  const [filtro] = useState({});

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
      dataIndex: 'numero',
    },
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
  ];

  const obterAnosLetivos = useCallback(async () => {
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
  }, [anoAtual]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const obterModalidades = async (ue, ano) => {
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
  };

  const obterUes = useCallback(async (dre, ano) => {
    if (dre) {
      const { data } = await AbrangenciaServico.buscarUes(
        dre,
        `v1/abrangencias/false/dres/${dre}/ues?anoLetivo=${ano}`,
        true
      );
      if (data) {
        const lista = data
          .map(item => ({
            desc: `${tipoEscolaDTO[item.tipoEscola]} ${item.nome}`,
            valor: String(item.codigo),
          }))
          .sort(FiltroHelper.ordenarLista('desc'));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, []);

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
      }
    }
  }, [anoLetivo]);

  const obterTurmas = useCallback(async (modalidadeSelecionada, ue) => {
    if (ue && modalidadeSelecionada) {
      const { data } = await AbrangenciaServico.buscarTurmas(
        ue,
        modalidadeSelecionada
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
    }
  }, []);

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
  }, [anoLetivo, ueId]);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId, anoLetivo);
    } else {
      setUeId(undefined);
      setListaUes([]);
    }
  }, [dreId, anoLetivo, obterUes]);

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
      if (String(modalidadeId) === String(modalidade.EJA)) {
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
      !anoLetivo || !dreId || !ueId || !modalidadeId || !turmaId;

    if (String(modalidadeId) === String(modalidade.EJA)) {
      setDesabilitarBtnGerar(!semestre || desabilitar);
    } else {
      setDesabilitarBtnGerar(desabilitar);
    }
  }, [anoLetivo, dreId, ueId, modalidadeId, turmaId, semestre]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

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
  };

  const onClickGerar = () => {
    sucesso(
      'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado'
    );
    setDesabilitarBtnGerar(true);

    const params = {
      anoLetivo,
      dreId,
      ueId,
      modalidadeId,
      semestre,
      turmaId,
      estudanteOpt,
      imprimirDadosResp,
      preencherDataImpressao,
      alunosCodigo: alunosSelecionados,
    };

    return params;
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
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaTurmas([]);
    setTurmaId(undefined);
  };

  const onChangeSemestre = valor => setSemestre(valor);
  const onChangeTurma = valor => setTurmaId(valor);

  const onChangeEstudanteOpt = valor => {
    setEstudanteOpt(valor);
  };

  const onChangeImprimirDadosResp = valor => setImprimirDadosResp(valor);
  const onChangePreencherDataImpressao = valor =>
    setPreencherDataImpressao(valor);

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno && (aluno.alunoCodigo || aluno.alunoNome)) {
      setAlunoLocalizadorSelect(aluno);
      setAnoLetivo(undefined);
      setModalidadeId(undefined);
      setTurmaId(undefined);
    } else {
      setAlunoLocalizadorSelect();
    }
  };

  const onSelecionarItems = items => {
    setAlunosSelecionados([...items.map(item => String(item.codigo))]);
  };

  return (
    <>
      <Cabecalho pagina="Histórico Escolar" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
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
                className="mr-3"
                onClick={() => onClickCancelar()}
              />
              <Button
                id="btn-gerar-historico-escolar"
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
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
              <SelectComponent
                label="Ano Letivo"
                lista={listaAnosLetivo}
                valueOption="valor"
                valueText="desc"
                disabled={
                  (listaAnosLetivo && listaAnosLetivo.length === 1) ||
                  alunoLocalizadorSelect
                }
                onChange={onChangeAnoLetivo}
                valueSelect={anoLetivo}
                placeholder="Ano letivo"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
              <div className="row">
                <LocalizadorEstudante
                  showLabel
                  onChange={onChangeLocalizadorEstudante}
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
              <SelectComponent
                label="Modalidade"
                lista={listaModalidades}
                valueOption="valor"
                valueText="desc"
                disabled={
                  !ueId ||
                  (listaModalidades && listaModalidades.length === 1) ||
                  alunoLocalizadorSelect
                }
                onChange={onChangeModalidade}
                valueSelect={modalidadeId}
                placeholder="Modalidade"
              />
            </div>
            {String(modalidadeId) === String(modalidade.EJA) ? (
              <div className="col-sm-12 col-md-12 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  lista={listaSemestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Semestre"
                  disabled={
                    !modalidadeId ||
                    String(modalidadeId) === String(modalidade.FUNDAMENTAL) ||
                    (listaSemestre && listaSemestre.length === 1)
                  }
                  valueSelect={semestre}
                  onChange={onChangeSemestre}
                  placeholder="Semestre"
                />
              </div>
            ) : null}
            <div
              className={`"col-sm-12 col-md-6 ${
                modalidadeId && String(modalidadeId) === String(modalidade.EJA)
                  ? `col-lg-3 col-xl-3`
                  : `col-lg-4 col-xl-4`
              } mb-2"`}
            >
              <SelectComponent
                lista={listaTurmas}
                valueOption="valor"
                valueText="desc"
                label="Turma"
                disabled={
                  !modalidadeId ||
                  (listaTurmas && listaTurmas.length === 1) ||
                  alunoLocalizadorSelect
                }
                valueSelect={turmaId}
                onChange={onChangeTurma}
                placeholder="Turma"
              />
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
              {listaEstudanteOpt === '1' ? (
                <ListaPaginada
                  url="v1/teste"
                  id="lista-alunos-historico-escolar"
                  colunaChave="id"
                  colunas={colunas}
                  filtro={filtro}
                  multiSelecao
                  selecionarItems={onSelecionarItems}
                />
              ) : (
                ''
              )}
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default HistoricoEscolar;
