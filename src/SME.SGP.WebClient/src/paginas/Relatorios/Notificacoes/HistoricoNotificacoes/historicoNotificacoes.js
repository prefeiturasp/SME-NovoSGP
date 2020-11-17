import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';

const HistoricoNotificacoes = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaSemestre, setListaSemestre] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [codigoDre, setCodigoDre] = useState(undefined);
  const [codigoUe, setCodigoUe] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [turmaId, setTurmaId] = useState(undefined);

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [desabilitarBtnGerar, setDesabilitarBtnGerar] = useState(true);

  const OPCAO_TODAS = '-99';

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
      const retorno = await ServicoFiltroRelatorio.obterModalidades(ue).catch(
        e => {
          erros(e);
          setCarregandoGeral(false);
        }
      );
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
    debugger;
    if (codigoUe === OPCAO_TODAS) {
      setListaTurmas([{ valor: '-99', desc: 'Todas' }]);
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
    const desabilitar =
      !anoLetivo || !codigoDre || !codigoUe || !modalidadeId || !turmaId;

    if (modalidadeId == modalidade.EJA) {
      setDesabilitarBtnGerar(!semestre || desabilitar);
    } else {
      setDesabilitarBtnGerar(desabilitar);
    }
  }, [anoLetivo, codigoDre, codigoUe, modalidadeId, semestre, turmaId]);

  useEffect(() => {
    obterAnosLetivos();
    obterDres();
  }, [obterAnosLetivos]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    setAnoLetivo(undefined);
    setCodigoDre(undefined);
    setListaAnosLetivo([]);
    setListaDres([]);

    obterAnosLetivos();
    obterDres();
  };

  const onClickGerar = async () => {
    const params = {
      dre: codigoDre,
      ue: codigoUe,
      anoLetivo,
      modalidadeTurma: modalidadeId,
      turmas: turmaId,
    };

    // {
    //   "dre": -99,
    //   "ue": -99,
    //   "anoLetivo": 2020,
    //   "modalidadeTurma": 5,
    //   "turmas": -99,
    //   "usuarioBuscaNome":"",
    //   "usuarioBuscaRf":"",
    //   "categorias":[1,3],
    //   "tipos":[1],
    //   "situacoes":[-99],
    //   "exibirDescricao":false,
    //   "exibirNotificacoesExcluidas":false
    // }

    console.log(params);

    // setCarregandoGeral(true);
    // const retorno = await ServicoFaltasFrequencia.gerar(params).catch(e => {
    //   erros(e);
    //   setCarregandoGeral(false);
    // });
    // if (retorno && retorno.status === 200) {
    //   sucesso(
    //     'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
    //   );
    //   setDesabilitarBtnGerar(true);
    // }
    // setCarregandoGeral(false);
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
              <div className="col-sm-12 col-md-6 col-lg-4 col-xl-5  mb-2">
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
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default HistoricoNotificacoes;
