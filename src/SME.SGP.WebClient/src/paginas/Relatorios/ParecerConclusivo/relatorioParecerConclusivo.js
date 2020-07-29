import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import modalidade from '~/dtos/modalidade';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import ServicoRelatorioPendencias from '~/servicos/Paginas/Relatorios/Pendencias/ServicoRelatorioPendencias';
import FiltroHelper from '~componentes-sgp/filtro/helper';
import ServicoRelatorioParecerConclusivo from '~/servicos/Paginas/Relatorios/ParecerConclusivo/ServicoRelatorioParecerConclusivo';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';

const RelatorioParecerConclusivo = () => {
  const [carregandoGerar, setCarregandoGerar] = useState(false);
  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [listaDres, setListaDres] = useState([]);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaUes, setListaUes] = useState([]);
  const [carregandoModalidades, setCarregandoModalidades] = useState(false);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [carregandoSemestres, setCarregandoSemestres] = useState(false);
  const [listaSemestres, setListaSemestres] = useState([]);
  const [carregandoCiclos, setCarregandoCiclos] = useState(false);
  const [listaCiclos, setListaCiclos] = useState([]);
  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [listaAnos, setListaAnos] = useState([]);
  const [
    carregandoPareceresConclusivos,
    setCarregandoPareceresConclusivos,
  ] = useState(false);
  const [listaPareceresConclusivos, setListaPareceresConclusivos] = useState(
    []
  );
  const listaFormatos = [
    { valor: '1', desc: 'PDF' },
    { valor: '2', desc: 'Excel' },
  ];

  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);
  const [modalidadeId, setModalidadeId] = useState(undefined);
  const [semestre, setSemestre] = useState(undefined);
  const [ciclo, setCiclo] = useState(undefined);
  const [ano, setAno] = useState(undefined);
  const [parecerConclusivoId, setParecerConclusivoId] = useState(undefined);
  const [formato, setFormato] = useState('1');

  const onChangeAnoLetivo = valor => {
    setAnoLetivo(valor);
  };

  const onChangeDre = valor => {
    setDreId(valor);
  };

  const onChangeUe = valor => {
    setUeId(valor);
  };

  const onChangeModalidade = valor => {
    setModalidadeId(valor);
  };

  const onChangeSemestre = valor => {
    setSemestre(valor);
  };

  const onChangeCiclos = valor => {
    setCiclo(valor);
  };

  const onChangeAnos = valor => {
    setAno(valor);
  };

  const onChangeParecerConclusivo = valor => {
    setParecerConclusivoId(valor);
  };

  const onChangeFormato = valor => {
    setFormato(valor);
  };

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const { data } = await ServicoFiltroRelatorio.obterDres();
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

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const obterUes = useCallback(async dre => {
    if (dre) {
      setCarregandoUes(true);
      const { data } = await ServicoFiltroRelatorio.obterUes(dre);
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
      setCarregandoUes(false);
    }
  }, []);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId);
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, obterUes]);

  const obterModalidades = async ue => {
    if (ue) {
      setCarregandoModalidades(true);
      const { data } = await ServicoFiltroRelatorio.obterModalidades(ue);
      if (data) {
        if (data && data.length && data.length === 1) {
          setModalidadeId(data[0].valor);
        }
        setListaModalidades(data);
      }
      setCarregandoModalidades(false);
    }
  };

  useEffect(() => {
    if (ueId) {
      obterModalidades(ueId);
    } else {
      setModalidadeId();
      setListaModalidades([]);
    }
  }, [ueId]);

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnosLetivos(true);
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
    setCarregandoAnosLetivos(false);
  }, [anoAtual]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const obterSemestres = async (
    modalidadeSelecionada,
    anoLetivoSelecionado
  ) => {
    setCarregandoSemestres(true);
    const retorno = await api
      .get(
        `v1/abrangencias/false/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}`
      )
      .catch(e => erros(e))
      .finally(() => {
        setCarregandoSemestres(false);
      });
    if (retorno && retorno.data) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista && lista.length && lista.length === 1) {
        setSemestre(lista[0].valor);
      }
      setListaSemestres(lista);
    }
  };

  const obterCiclos = async () => {
    setCarregandoCiclos(true);
    const retorno = await ServicoRelatorioParecerConclusivo.buscarCiclos()
      .catch(e => erros(e))
      .finally(() => {
        setCarregandoCiclos(false);
      });
    if (retorno && retorno.data) {
      const lista = retorno.data;
      setListaCiclos(lista);
    }
  };

  const obterPareceresConclusivos = async () => {
    setCarregandoPareceresConclusivos(true);
    const retorno = await ServicoRelatorioParecerConclusivo.buscarPareceresConclusivos()
      .catch(e => erros(e))
      .finally(() => {
        setCarregandoPareceresConclusivos(false);
      });
    if (retorno && retorno.data) {
      const lista = retorno.data;
      setListaPareceresConclusivos(lista);
    }
  };

  const obterAnos = async (codigoUe, modalidadeIdSelecionada) => {
    setCarregandoAnos(true);
    const retorno = await ServicoFiltroRelatorio.obterAnosEscolares(
      codigoUe,
      modalidadeIdSelecionada
    ).finally(setCarregandoAnos(false));
    if (retorno && retorno.data) {
      const lista = retorno.data;
      setListaAnos(lista);
    }
  };

  useEffect(() => {
    if (modalidadeId && anoLetivo) {
      obterCiclos();
      obterPareceresConclusivos();
    }
    if (
      modalidadeId &&
      anoLetivo &&
      String(modalidadeId) === String(modalidade.EJA)
    ) {
      obterSemestres(modalidadeId, anoLetivo);
    } else {
      setSemestre();
      setListaSemestres([]);
    }
  }, [modalidadeId, anoLetivo]);

  useEffect(() => {
    if (modalidadeId && ueId) {
      obterAnos(ueId, modalidadeId);
    }
  }, [modalidadeId, ueId]);

  const cancelar = () => {
    setAnoLetivo(anoAtual);
  };

  const desabilitarGerar =
    !anoLetivo ||
    !dreId ||
    !ueId ||
    !modalidadeId ||
    (String(modalidadeId) === String(modalidade.EJA) ? !semestre : false) ||
    !ciclo ||
    !ano ||
    !parecerConclusivoId ||
    !formato;

  const gerar = async () => {
    setCarregandoGerar(true);
    const params = {
      anoLetivo,
      dreCodigo: dreId,
      ueCodigo: ueId,
      modalidade: modalidadeId,
      semestre,
      ciclo,
      anoEscolar: ano,
      parecerConclusivoId,
    };
    await ServicoRelatorioParecerConclusivo.gerar(params)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e))
      .finally(setCarregandoGerar(false));
  };

  return (
    <>
      <Cabecalho pagina="Relatório de pendências" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
              <Button
                id="btn-voltar-rel-parecer"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={() => {
                  history.push('/');
                }}
              />
              <Button
                id="btn-cancelar-rel-parecer"
                label="Cancelar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={() => {
                  cancelar();
                }}
              />
              <Loader
                loading={carregandoGerar}
                className="d-flex w-auto"
                tip=""
              >
                <Button
                  id="btn-gerar-rel-parecer"
                  icon="print"
                  label="Gerar"
                  color={Colors.Azul}
                  border
                  bold
                  className="mr-0"
                  onClick={gerar}
                  disabled={desabilitarGerar}
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoAnosLetivos} tip="">
                <SelectComponent
                  id="drop-ano-letivo-rel-parecer"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo && listaAnosLetivo.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Ano letivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  id="drop-dre-rel-parecer"
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
                  id="drop-ue-rel-parecer"
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
            <div
              className={`"col-sm-12 col-md-6 ${
                modalidadeId && String(modalidadeId) === String(modalidade.EJA)
                  ? `col-lg-3 col-xl-3`
                  : `col-lg-4 col-xl-4`
              } mb-2"`}
            >
              <Loader loading={carregandoModalidades} tip="">
                <SelectComponent
                  id="drop-modalidade-rel-parecer"
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="descricao"
                  disabled={
                    !ueId || (listaModalidades && listaModalidades.length === 1)
                  }
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeId}
                  placeholder="Modalidade"
                />
              </Loader>
            </div>
            {String(modalidadeId) === String(modalidade.EJA) ? (
              <div className="col-sm-12 col-md-6 col-lg-1 col-xl-1 mb-2">
                <Loader loading={carregandoSemestres} tip="">
                  <SelectComponent
                    id="drop-semestre-rel-parecer"
                    lista={listaSemestres}
                    valueOption="valor"
                    valueText="desc"
                    label="Semestre"
                    disabled={
                      !modalidadeId ||
                      (listaSemestres && listaSemestres.length === 1) ||
                      String(modalidadeId) === String(modalidade.FUNDAMENTAL)
                    }
                    valueSelect={semestre}
                    onChange={onChangeSemestre}
                    placeholder="Semestre"
                  />
                </Loader>
              </div>
            ) : null}
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoCiclos} tip="">
                <SelectComponent
                  id="drop-ciclos-rel-parecer"
                  label="Ciclo"
                  lista={listaCiclos}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaCiclos && listaCiclos.length === 1}
                  onChange={onChangeCiclos}
                  valueSelect={ciclo}
                  placeholder="Ciclo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoAnos} tip="">
                <SelectComponent
                  id="drop-ano-rel-parecer"
                  label="Ano"
                  lista={listaAnos}
                  valueOption="valor"
                  valueText="descricao"
                  disabled={listaAnos && listaAnos.length === 1}
                  onChange={onChangeAnos}
                  valueSelect={ano}
                  placeholder="Ano"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 mb-2">
              <Loader loading={carregandoPareceresConclusivos} tip="">
                <SelectComponent
                  id="drop-parecer-conclucivo-rel-parecer"
                  label="Parecer conclusivo"
                  lista={listaPareceresConclusivos}
                  valueOption="valor"
                  valueText="desc"
                  disabled={
                    listaPareceresConclusivos &&
                    listaPareceresConclusivos.length === 1
                  }
                  onChange={onChangeParecerConclusivo}
                  valueSelect={parecerConclusivoId}
                  placeholder="Parecer conclusivo"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 mb-2">
              <SelectComponent
                id="drop-formato-rel-parecer"
                label="Formato"
                lista={listaFormatos}
                valueOption="valor"
                valueText="desc"
                onChange={onChangeFormato}
                valueSelect={formato}
                placeholder="Formato"
              />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default RelatorioParecerConclusivo;
