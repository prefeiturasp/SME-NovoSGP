import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import CampoNumero from '~/componentes/campoNumero';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import modalidade from '~/dtos/modalidade';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoRelatorioNotasConceitos from '~/servicos/Paginas/Relatorios/NotasConceitos/servicoRelatorioNotasConceitos';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import tipoNota from '~/dtos/tipoNota';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';

const RelatorioNotasConceitosFinais = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaSemestre, setListaSemestre] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaAnosEscolares, setListaAnosEscolares] = useState([]);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [listaBimestre, setListaBimestre] = useState([]);

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [codigoDre, setCodigoDre] = useState(undefined);
  const [codigoUe, setCodigoUe] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [anosEscolares, setAnosEscolares] = useState(undefined);
  const [componentesCurriculares, setComponentesCurriculares] = useState(
    undefined
  );
  const [bimestres, setBimestres] = useState(undefined);
  const [valorCondicao, setValorCondicao] = useState(undefined);
  const [tipoNotaSelecionada, setTipoNotaSelecionada] = useState(undefined);
  const [listaConceitos, setListaConceitos] = useState([]);
  const [campoBloqueado, setCampoBloqueado] = useState(false);

  const listaCondicaoInicial = [
    { valor: '0', desc: 'Todas' },
    { valor: '1', desc: 'Igual' },
    { valor: '2', desc: 'Maior ' },
    { valor: '3', desc: 'Menor' },
  ];
  const [listaCondicao, setListaCondicao] = useState(listaCondicaoInicial);
  const [condicao, setCondicao] = useState(undefined);

  const listaFormatos = [
    { valor: '1', desc: 'PDF' },
    { valor: '4', desc: 'Excel' },
  ];
  const listaTipoNota = [
    { valor: tipoNota.todas, desc: 'Todas' },
    { valor: tipoNota.nota, desc: 'Nota' },
    { valor: tipoNota.conceito, desc: 'Conceito' },
    { valor: tipoNota.sintese, desc: 'Síntese' },
  ];
  const [formato, setFormato] = useState('1');

  const [carregandoGeral, setCarregandoGeral] = useState(false);

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
      const retorno = await ServicoFiltroRelatorio.obterModalidadesAnoLetivo(
        ue,
        anoLetivo
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

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
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
  };

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

  const obterAnosEscolares = useCallback(
    async (mod, ue, anoLetivoSelecionado) => {
      if (String(mod) === String(modalidade.EJA)) {
        setListaAnosEscolares([{ descricao: 'Todos', valor: '-99' }]);
        setAnosEscolares(['-99']);
      } else {
        setCarregandoGeral(true);
        const anoAtual = window.moment().format('YYYY');
        const respota = await AbrangenciaServico.buscarAnosEscolares(
          ue,
          mod,
          String(anoLetivoSelecionado) !== String(anoAtual)
        ).catch(e => {
          erros(e);
          setCarregandoGeral(false);
        });

        if (respota && respota.data && respota.data.length) {
          setListaAnosEscolares(
            [{ descricao: 'Todos', valor: '-99' }].concat(respota.data)
          );

          if (
            respota.data &&
            respota.data.length &&
            respota.data.length === 1
          ) {
            setAnosEscolares(respota.data[0].valor);
          }
        } else {
          setListaAnosEscolares([]);
        }
        setCarregandoGeral(false);
      }
    },
    []
  );

  useEffect(() => {
    if (modalidadeId && codigoUe && anoLetivo) {
      obterAnosEscolares(modalidadeId, codigoUe, anoLetivo);
    } else {
      setAnosEscolares(undefined);
      setListaAnosEscolares([]);
    }
  }, [modalidadeId, codigoUe, anoLetivo, obterAnosEscolares]);

  const obterCodigoTodosAnosEscolares = useCallback(() => {
    let todosAnosEscolares = anosEscolares;
    const selecionouTodos = [].concat(anosEscolares).find(ano => ano === '-99');
    if (selecionouTodos) {
      todosAnosEscolares = listaAnosEscolares.map(item => item.valor);
    }
    return todosAnosEscolares;
  }, [anosEscolares, listaAnosEscolares]);

  const obterComponenteCurricular = useCallback(async () => {
    const codigoTodosAnosEscolares = obterCodigoTodosAnosEscolares();
    if (anoLetivo) {
      setCarregandoGeral(true);
      const retorno = await ServicoFiltroRelatorio.obterComponetensCuriculares(
        codigoUe,
        modalidadeId,
        anoLetivo,
        codigoTodosAnosEscolares
      ).catch(e => {
        erros(e);
        setCarregandoGeral(false);
      });
      if (retorno && retorno.data && retorno.data.length) {
        const lista = retorno.data.map(item => {
          return { desc: item.descricao, valor: String(item.codigo) };
        });

        setListaComponenteCurricular(lista);
        if (lista && lista.length && lista.length === 1) {
          setComponentesCurriculares(lista[0].valor);
        }
      } else {
        setListaComponenteCurricular([]);
      }
      setCarregandoGeral(false);
    }
  }, [modalidadeId, anoLetivo, obterCodigoTodosAnosEscolares]);

  useEffect(() => {
    if (anosEscolares && anosEscolares.length) {
      obterComponenteCurricular();
    } else {
      setComponentesCurriculares(undefined);
      setListaComponenteCurricular([]);
    }
  }, [anosEscolares, obterComponenteCurricular]);

  const obterBimestres = useCallback(() => {
    const bi = [];
    bi.push({ desc: '1º', valor: 1 });
    bi.push({ desc: '2º', valor: 2 });

    if (modalidadeId !== modalidade.EJA) {
      bi.push({ desc: '3º', valor: 3 });
      bi.push({ desc: '4º', valor: 4 });
    }

    bi.push({ desc: 'Final', valor: 0 });
    bi.push({ desc: 'Todos', valor: -99 });
    setListaBimestre(bi);
  }, [modalidadeId]);

  useEffect(() => {
    if (modalidadeId) {
      obterBimestres();
    } else {
      setListaBimestre([]);
      setBimestres(undefined);
    }
  }, [modalidadeId, obterBimestres]);

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
  }, [modalidadeId, anoLetivo]);

  const obterConceitos = async anoLetivoSelecionado => {
    setCarregandoGeral(true);
    const conceitos = await ServicoNotaConceito.obterTodosConceitos(
      `01-01-${anoLetivoSelecionado}`
    )
      .catch(e => erros(e))
      .finally(setCarregandoGeral(false));
    setListaConceitos(conceitos?.data);
  };

  const obterSinteses = async anoLetivoSelecionado => {
    setCarregandoGeral(true);
    const conceitos = await ServicoNotaConceito.obterTodasSinteses(
      anoLetivoSelecionado
    )
      .catch(e => erros(e))
      .finally(setCarregandoGeral(false));
    setListaConceitos(conceitos?.data);
  };

  useEffect(() => {
    if (anoLetivo && tipoNotaSelecionada) {
      if (tipoNotaSelecionada === tipoNota.conceito) obterConceitos(anoLetivo);
      else obterSinteses(anoLetivo);
    }
  }, [tipoNotaSelecionada, anoLetivo]);

  const valorCondicaoDesabilitar =
    tipoNotaSelecionada === tipoNota.todas || condicao === '0'
      ? false
      : valorCondicao === undefined || valorCondicao === '';

  const desabilitarGerar =
    !anoLetivo ||
    !codigoDre ||
    !codigoUe ||
    !modalidadeId ||
    !anosEscolares ||
    !componentesCurriculares ||
    !bimestres ||
    !condicao ||
    !tipoNota ||
    valorCondicaoDesabilitar ||
    (String(modalidadeId) === String(modalidade.EJA) ? !semestre : false) ||
    !formato;

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
    setCondicao(undefined);
    setValorCondicao(undefined);
    setListaAnosLetivo([]);
    setListaDres([]);
    setTipoNotaSelecionada(undefined);

    obterAnosLetivos();
    obterDres();

    setFormato('PDF');
  };

  const onClickGerar = async () => {
    setCarregandoGeral(true);
    const params = {
      anoLetivo,
      dreCodigo: codigoDre === '-99' ? '' : codigoDre,
      ueCodigo: codigoUe === '-99' ? '' : codigoUe,
      modalidade: modalidadeId,
      semestre,
      anos: anosEscolares.toString() !== '-99' ? [].concat(anosEscolares) : [],
      componentesCurriculares:
        componentesCurriculares.toString() !== '-99'
          ? [].concat(componentesCurriculares)
          : [],
      bimestres: [bimestres],
      condicao,
      valorCondicao,
      tipoNota: tipoNotaSelecionada,
      tipoFormatoRelatorio: formato,
    };
    setCarregandoGeral(true);
    const retorno = await ServicoRelatorioNotasConceitos.gerar(params)
      .catch(e => {
        erros(e);
      })
      .finally(setCarregandoGeral(false));
    if (retorno && retorno.status === 200) {
      sucesso(
        'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
      );
    }
  };

  const onChangeUe = ue => {
    setCodigoUe(ue);

    setListaModalidades([]);
    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeModalidade = novaModalidade => {
    setModalidadeId(novaModalidade);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);

    setModalidadeId(undefined);

    setListaSemestre([]);
    setSemestre(undefined);

    setListaAnosEscolares([]);
    setAnosEscolares(undefined);
  };

  const onChangeAnos = valor => {
    setAnosEscolares(valor);

    setListaComponenteCurricular([]);
    setComponentesCurriculares(undefined);
  };
  const onChangeSemestre = valor => setSemestre(valor);
  const onChangeComponenteCurricular = valor =>
    setComponentesCurriculares(valor);
  const onChangeBimestre = valor => setBimestres(valor);

  const onChangeCondicao = valor => {
    if (valor === '0') {
      setCampoBloqueado(true);
    }
    setCondicao(valor);
  };

  const onChangeComparacao = valor => {
    setValorCondicao(valor);
  };

  const onChangeTipoNota = valor => {
    let valorCampoCondicao;
    let bloqueado = false;
    let novaListaCondicao = listaCondicaoInicial;
    const valorOpcaoMaior = 2;

    if (valor === tipoNota.todas) {
      valorCampoCondicao = '0';
      bloqueado = true;
    }
    if (valor === tipoNota.conceito) {
      valorCampoCondicao = '1';
      novaListaCondicao = listaCondicaoInicial
        .map(item => {
          if (String(item.valor) === '0') {
            return {
              ...item,
              desc: 'Todos',
            };
          }
          return item;
        })
        .filter(item => Number(item.valor) < valorOpcaoMaior);
    }
    if (valor === tipoNota.sintese) {
      valorCampoCondicao = '1';
      novaListaCondicao = listaCondicaoInicial.filter(
        item => Number(item.valor) < valorOpcaoMaior
      );
    }
    setListaCondicao(novaListaCondicao);
    setValorCondicao(undefined);
    setCondicao(valorCampoCondicao);
    setTipoNotaSelecionada(valor);
    setCampoBloqueado(bloqueado);
  };
  const onChangeFormato = valor => setFormato(valor);

  const removeAdicionaOpcaoTodos = (
    valoresJaSelcionados,
    valoresParaSelecionar
  ) => {
    const todosEhUnicoJaSelecionado =
      valoresJaSelcionados &&
      valoresJaSelcionados.length === 1 &&
      valoresJaSelcionados[0] === '-99';

    if (todosEhUnicoJaSelecionado) {
      if (
        valoresParaSelecionar &&
        valoresParaSelecionar.length > 1 &&
        valoresParaSelecionar.includes('-99')
      ) {
        valoresParaSelecionar = valoresParaSelecionar.filter(
          item => item !== '-99'
        );
      }
    }

    if (
      !todosEhUnicoJaSelecionado &&
      valoresParaSelecionar &&
      valoresParaSelecionar.length &&
      valoresParaSelecionar.length > 1 &&
      valoresParaSelecionar.includes('-99')
    ) {
      valoresParaSelecionar = valoresParaSelecionar.filter(
        item => item === '-99'
      );
    }

    return valoresParaSelecionar;
  };

  return (
    <>
      <AlertaModalidadeInfantil
        exibir={String(modalidadeId) === String(modalidade.INFANTIL)}
        validarModalidadeFiltroPrincipal={false}
      />
      <Cabecalho pagina="Notas e conceitos" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id="btn-voltar-frequencia-faltas"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar-frequencia-faltas"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={() => onClickCancelar()}
                />
                <Button
                  id="btn-gerar-frequencia-faltas"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  className="mr-2"
                  onClick={() => onClickGerar()}
                  disabled={desabilitarGerar}
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
              <div className="col-sm-12 col-md-6 col-lg-9 col-xl-5 mb-2">
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
              <div className="col-sm-12 col-md-6 col-lg-9 col-xl-5 mb-2">
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
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
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
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  lista={listaSemestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Semestre"
                  disabled={
                    !modalidadeId ||
                    String(modalidadeId) !== String(modalidade.EJA) ||
                    (listaSemestre && listaSemestre.length === 1)
                  }
                  valueSelect={semestre}
                  onChange={onChangeSemestre}
                  placeholder="Selecione o semestre"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  lista={listaAnosEscolares}
                  valueOption="valor"
                  valueText="descricao"
                  label="Ano"
                  disabled={
                    listaAnosEscolares && listaAnosEscolares.length === 1
                  }
                  valueSelect={anosEscolares}
                  onChange={valoresNovos => {
                    valoresNovos = removeAdicionaOpcaoTodos(
                      anosEscolares,
                      valoresNovos
                    );
                    onChangeAnos(valoresNovos);
                  }}
                  placeholder="Selecione o ano"
                  multiple
                />
              </div>
              <div className="col-sm-12 col-md-9 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  lista={listaComponenteCurricular}
                  valueOption="valor"
                  valueText="desc"
                  label="Componente Curricular"
                  disabled={
                    listaComponenteCurricular &&
                    listaComponenteCurricular.length === 1
                  }
                  valueSelect={componentesCurriculares}
                  onChange={valoresNovos => {
                    valoresNovos = removeAdicionaOpcaoTodos(
                      componentesCurriculares,
                      valoresNovos
                    );
                    onChangeComponenteCurricular(valoresNovos);
                  }}
                  placeholder="Selecione o componente curricular"
                  multiple
                />
              </div>
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaBimestre}
                  valueOption="valor"
                  valueText="desc"
                  label="Bimestre"
                  disabled={listaBimestre && listaBimestre.length === 1}
                  valueSelect={bimestres}
                  onChange={onChangeBimestre}
                  placeholder="Selecione o bimestre"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaTipoNota}
                  valueOption="valor"
                  valueText="desc"
                  label="Tipo de nota"
                  disabled={!anoLetivo}
                  valueSelect={tipoNotaSelecionada}
                  onChange={onChangeTipoNota}
                  placeholder="Selecione o tipo de nota"
                />
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                <SelectComponent
                  lista={listaCondicao}
                  valueOption="valor"
                  valueText="desc"
                  label="Condição"
                  disabled={
                    (listaCondicao && listaCondicao.length === 1) ||
                    campoBloqueado
                  }
                  valueSelect={condicao}
                  onChange={onChangeCondicao}
                  placeholder="Selecione a condição"
                />
              </div>

              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                {tipoNotaSelecionada === tipoNota.nota ? (
                  <CampoNumero
                    onChange={onChangeComparacao}
                    value={valorCondicao}
                    min={0}
                    max={10}
                    step={0.5}
                    label="Valor"
                    className="w-100"
                    placeholder="Selecione o valor"
                    disabled={!tipoNotaSelecionada || campoBloqueado}
                  />
                ) : (
                  <SelectComponent
                    label="Valor"
                    lista={listaConceitos}
                    valueOption="id"
                    valueText="valor"
                    valueSelect={valorCondicao}
                    onChange={onChangeComparacao}
                    placeholder="Selecione o valor"
                    disabled={!tipoNotaSelecionada || campoBloqueado}
                  />
                )}
              </div>
              <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
                <SelectComponent
                  label="Formato"
                  lista={listaFormatos}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={formato}
                  onChange={onChangeFormato}
                  placeholder="Selecione o formato"
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default RelatorioNotasConceitosFinais;
