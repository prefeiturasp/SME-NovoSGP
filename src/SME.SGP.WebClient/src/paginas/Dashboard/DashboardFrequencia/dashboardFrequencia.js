import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { CheckboxComponent, Loader, SelectComponent } from '~/componentes';
import { Cabecalho, FiltroHelper } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { ModalidadeDTO } from '~/dtos';
import { ServicoFiltroRelatorio } from '~/servicos';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import history from '~/servicos/history';
import GraficosFrequencia from './DadosDashboardFrequencia/graficosFrequencia';

const DashboardFrequencia = () => {
  const usuario = useSelector(store => store.usuario);

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaModalidades, setListaModalidades] = useState([]);
  const [listaSemestres, setListaSemestres] = useState([]);

  const [consideraHistorico, setConsideraHistorico] = useState(false);
  const [anoAtual] = useState(moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState(anoAtual);
  const [dre, setDre] = useState();
  const [ue, setUe] = useState();
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState();
  const [semestreSelecionado, setSemestreSelecionado] = useState();

  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [carregandoModalidades, setCarregandoModalidades] = useState(false);
  const [carregandoSemestres, setCarregandoSemestres] = useState(false);

  const OPCAO_TODOS = '-99';

  const validarValorPadraoAnoLetivo = (lista, atual) => {
    if (lista?.length) {
      const temAnoAtualNaLista = lista.find(
        item => String(item.valor) === String(atual)
      );
      if (temAnoAtualNaLista) {
        setAnoLetivo(atual);
      } else {
        setAnoLetivo(lista[0].valor);
      }
    } else {
      setAnoLetivo();
    }
  };

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnosLetivos(true);

    const anosLetivos = await FiltroHelper.obterAnosLetivos({
      consideraHistorico,
      anoMinimo: 2021,
    });

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    validarValorPadraoAnoLetivo(anosLetivos, anoAtual);

    setListaAnosLetivo(anosLetivos);
    setCarregandoAnosLetivos(false);
  }, [anoAtual, consideraHistorico]);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos, consideraHistorico]);

  const obterUes = useCallback(async () => {
    if (dre?.codigo) {
      if (dre?.codigo === OPCAO_TODOS) {
        const ueTodos = { nome: 'Todas', codigo: OPCAO_TODOS };
        setListaUes([ueTodos]);
        setUe(ueTodos);
        return;
      }

      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dre?.codigo,
        `v1/abrangencias/${consideraHistorico}/dres/${dre?.codigo}/ues?anoLetivo=${anoLetivo}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data?.length) {
        const lista = resposta.data;

        if (usuario.possuiPerfilSmeOuDre) {
          lista.unshift({ codigo: OPCAO_TODOS, nome: 'Todas' });
        }

        setListaUes(lista);

        if (lista?.length === 1) {
          setUe(lista[0]);
        }
      } else {
        setListaUes([]);
      }
    }
  }, [consideraHistorico, anoLetivo, dre, usuario.possuiPerfilSmeOuDre]);

  useEffect(() => {
    if (dre?.codigo) {
      obterUes();
    } else {
      setUe();
      setListaUes([]);
    }
  }, [dre, anoLetivo, consideraHistorico, obterUes]);

  const onChangeDre = codigoDre => {
    if (codigoDre) {
      const dreAtual = listaDres?.find(item => item.codigo === codigoDre);
      if (dreAtual) {
        setDre(dreAtual);
      }
    } else {
      setDre();
    }
    setListaUes([]);
    setUe(undefined);
  };

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivo}`,
        consideraHistorico
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data?.length) {
        const lista = resposta.data;
        if (usuario.possuiPerfilSme) {
          lista.unshift({ codigo: OPCAO_TODOS, nome: 'Todas' });
        }
        setListaDres(lista);

        if (resposta.data.length === 1) {
          setDre(resposta.data[0]);
        }
      } else {
        setListaDres([]);
        setDre();
      }
    }
  }, [usuario.possuiPerfilSme, anoLetivo, consideraHistorico]);

  useEffect(() => {
    obterDres();
  }, [obterDres, anoLetivo, consideraHistorico]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUe = codigoUe => {
    setModalidadeSelecionada();
    setListaModalidades([]);

    if (codigoUe) {
      const ueAtual = listaUes?.find(item => item.codigo === codigoUe);
      if (ueAtual) {
        setUe(ueAtual);
      }
    } else {
      setUe();
    }
  };

  const onChangeAnoLetivo = ano => {
    setDre();
    setUe();
    setListaDres([]);
    setListaUes([]);
    setAnoLetivo(ano);
  };

  const obterModalidades = useCallback(async () => {
    setCarregandoModalidades(true);

    const resultado = await ServicoFiltroRelatorio.obterModalidades(
      ue?.codigo,
      anoLetivo,
      consideraHistorico
    )
      .catch(e => erros(e))
      .finally(() => setCarregandoModalidades(false));

    if (resultado?.data?.length) {
      if (resultado.data.length === 1) {
        setModalidadeSelecionada(resultado.data[0].valor);
      }

      setListaModalidades(resultado.data);
    } else {
      setListaModalidades([]);
      setModalidadeSelecionada();
    }
  }, [ue, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (ue && OPCAO_TODOS !== ue?.codigo && anoLetivo) {
      obterModalidades();
    } else {
      setListaModalidades([]);
      setModalidadeSelecionada();
    }
  }, [ue, anoLetivo, consideraHistorico, obterModalidades]);

  const onChangeModalidade = valor => {
    setSemestreSelecionado();
    setListaSemestres([]);

    setModalidadeSelecionada(valor);
  };

  const obterSemestres = useCallback(async () => {
    setCarregandoSemestres(true);
    const retorno = await AbrangenciaServico.obterSemestres(
      consideraHistorico,
      anoLetivo,
      modalidadeSelecionada
    )
      .catch(e => erros(e))
      .finally(() => setCarregandoSemestres(false));

    if (retorno?.data?.length) {
      const lista = retorno.data.map(periodo => {
        return { desc: periodo, valor: periodo };
      });

      if (lista?.length === 1) {
        setSemestreSelecionado(lista[0].valor);
      }
      setListaSemestres(lista);
    } else {
      setSemestreSelecionado();
      setListaSemestres([]);
    }
  }, [consideraHistorico, anoLetivo, modalidadeSelecionada]);

  useEffect(() => {
    if (
      ue &&
      modalidadeSelecionada &&
      anoLetivo &&
      String(modalidadeSelecionada) === String(ModalidadeDTO.EJA)
    ) {
      obterSemestres();
    } else {
      setSemestreSelecionado();
      setListaSemestres([]);
    }
  }, [
    ue,
    modalidadeSelecionada,
    anoLetivo,
    consideraHistorico,
    obterSemestres,
  ]);

  const onChangeSemestre = valor => {
    setSemestreSelecionado(valor);
  };

  return (
    <>
      <Cabecalho pagina="Dashboard frequência" />

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
                onClick={onClickVoltar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
              <CheckboxComponent
                label="Exibir histórico?"
                onChangeCheckbox={e => {
                  setAnoLetivo();
                  setDre();
                  setUe();
                  setConsideraHistorico(e.target.checked);
                }}
                checked={consideraHistorico}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
              <Loader loading={carregandoAnosLetivos}>
                <SelectComponent
                  id="ano-letivo"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo?.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Selecione o ano"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-9 col-xl-5 mb-2">
              <Loader loading={carregandoDres}>
                <SelectComponent
                  id="dre"
                  label="DRE"
                  lista={listaDres}
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaDres?.length === 1}
                  onChange={onChangeDre}
                  valueSelect={dre?.codigo}
                  placeholder="Diretoria Regional de Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-5 mb-2">
              <Loader loading={carregandoUes}>
                <SelectComponent
                  id="ue"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaUes?.length === 1}
                  onChange={onChangeUe}
                  valueSelect={ue?.codigo}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
              <Loader loading={carregandoModalidades}>
                <SelectComponent
                  id="modalidade"
                  label="Modalidade"
                  lista={listaModalidades}
                  valueOption="valor"
                  valueText="descricao"
                  disabled={
                    listaModalidades?.length === 1 || OPCAO_TODOS === ue?.codigo
                  }
                  onChange={onChangeModalidade}
                  valueSelect={modalidadeSelecionada}
                  placeholder="Selecione uma modalidade"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-6 col-lg-6 col-xl-3 mb-2">
              <Loader loading={carregandoSemestres}>
                <SelectComponent
                  id="semestre"
                  label="Semestre"
                  lista={listaSemestres}
                  valueOption="valor"
                  valueText="descricao"
                  disabled={
                    listaSemestres?.length === 1 ||
                    String(modalidadeSelecionada) !== String(ModalidadeDTO.EJA)
                  }
                  onChange={onChangeSemestre}
                  valueSelect={semestreSelecionado}
                  placeholder="Selecione um semestre"
                />
              </Loader>
            </div>
          </div>
          <div className="row">
            <div className="col-md-12 mt-2">
              {anoLetivo && dre && ue ? (
                <GraficosFrequencia
                  anoLetivo={anoLetivo}
                  dreId={OPCAO_TODOS === dre?.codigo ? OPCAO_TODOS : dre?.id}
                  ueId={OPCAO_TODOS === ue?.codigo ? OPCAO_TODOS : ue?.id}
                  modalidade={modalidadeSelecionada}
                  semestre={semestreSelecionado}
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

export default DashboardFrequencia;
