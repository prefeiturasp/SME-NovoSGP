import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { Tooltip, Switch } from 'antd';
import shortid from 'shortid';

import {
  Base,
  Button,
  Card,
  Colors,
  Loader,
  Grid,
  SelectComponent,
  SelectAutocomplete,
} from '~/componentes';
import { FiltroHelper } from '~/componentes-sgp';
import Calendario from '~/componentes-sgp/calendarioEscolar/Calendario';

import {
  api,
  history,
  ServicoCalendarios,
  AbrangenciaServico,
} from '~/servicos';

import { store } from '~/redux';
import { zeraCalendario } from '~/redux/modulos/calendarioEscolar/actions';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;
const Icone = styled.i``;
const Label = styled.label``;

const CalendarioEscolar = () => {
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    undefined
  );
  const [diasLetivos, setDiasLetivos] = useState({});
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaTipoCalendario, setListaTipoCalendario] = useState([]);
  const [valorTipoCalendario, setValorTipoCalendario] = useState('');
  const [pesquisaTipoCalendario, setPesquisaTipoCalendario] = useState('');

  const eventoCalendarioEdicao = useSelector(
    state => state.calendarioEscolar.eventoCalendarioEdicao
  );

  const [eventoSme, setEventoSme] = useState(true);

  useEffect(() => {
    if (eventoCalendarioEdicao?.tipoCalendario) {
      if (eventoCalendarioEdicao.eventoSme) {
        setEventoSme(eventoCalendarioEdicao.eventoSme);
      }
    }
  }, [eventoCalendarioEdicao]);

  const [dreSelecionada, setDreSelecionada] = useState(undefined);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState(
    undefined
  );

  const consultarDiasLetivos = () => {
    api
      .post('v1/calendarios/dias-letivos', {
        tipoCalendarioId: tipoCalendarioSelecionado,
        dreId: dreSelecionada,
        ueId: unidadeEscolarSelecionada,
      })
      .then(resposta => {
        if (resposta.data) setDiasLetivos(resposta.data);
      })
      .catch(() => {
        setDiasLetivos({});
      });
  };

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState([]);

  const obterDres = () => {
    setCarregandoDres(true);
    api
      .get('v1/abrangencias/false/dres')
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(dre => {
              lista.push({
                desc: dre.nome,
                valor: dre.codigo,
                abrev: dre.abreviacao,
              });
            });
            setDres(lista.sort(FiltroHelper.ordenarLista('desc')));
            setCarregandoDres(false);
          }
        }
      })
      .catch(() => {
        setDres(dresStore);
        setCarregandoDres(false);
      });
  };

  const [filtros, setFiltros] = useState({
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
  });

  const [carregandoMeses, setCarregandoMeses] = useState(false);

  useEffect(() => {
    if (tipoCalendarioSelecionado) {
      consultarDiasLetivos();
      obterDres();
    } else {
      setDiasLetivos();
      setDreSelecionada();
      setUnidadeEscolarSelecionada();
    }
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tipoCalendarioSelecionado]);

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const aoTrocarEventoSme = valor => {
    setEventoSme(valor);
  };

  useEffect(() => {
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
    store.dispatch(zeraCalendario());
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [eventoSme]);

  useEffect(() => {
    if (!dreSelecionada && carregandoMeses) {
      setCarregandoDres(true);
      return;
    }
    setCarregandoDres(false);
    if (dres.length === 1) {
      setDreSelecionada(dres[0].valor);
    } else if (dres && eventoCalendarioEdicao && eventoCalendarioEdicao.dre) {
      setDreSelecionada(eventoCalendarioEdicao.dre);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dres, carregandoMeses]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);

  const obterUnidadesEscolares = dre => {
    setCarregandoUes(true);
    const calendario = listaTipoCalendario.find(
      item => item.id === tipoCalendarioSelecionado
    );

    const modalidade = ServicoCalendarios.converterModalidade(
      calendario.modalidade
    );
    AbrangenciaServico.buscarUes(dre, '', false, modalidade)
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(unidade => {
              lista.push({
                desc: unidade.nome,
                valor: unidade.codigo,
              });
            });
            setUnidadesEscolares(lista);
            setCarregandoUes(false);
          }
        }
      })
      .catch(() => {
        setUnidadesEscolares(unidadesEscolaresStore);
        setCarregandoUes(false);
      });
  };

  useEffect(() => {
    if (carregandoMeses && dreSelecionada && !unidadeEscolarSelecionada) {
      setCarregandoUes(true);
      return;
    }
    setCarregandoUes(false);
    if (unidadesEscolares.length === 1) {
      setUnidadeEscolarSelecionada(unidadesEscolares[0].valor);
    } else if (
      unidadesEscolares &&
      eventoCalendarioEdicao &&
      eventoCalendarioEdicao.unidadeEscolar
    ) {
      setDreSelecionada(eventoCalendarioEdicao.unidadeEscolar);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [unidadesEscolares, carregandoMeses]);

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
    setUnidadeEscolarSelecionada();
  };

  useEffect(() => {
    if (dreSelecionada) {
      consultarDiasLetivos();
      obterUnidadesEscolares(dreSelecionada);
    } else {
      setUnidadeEscolarSelecionada();
    }
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dreSelecionada]);

  const aoSelecionarUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
  };

  useEffect(() => {
    if (unidadeEscolarSelecionada) consultarDiasLetivos();
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
    store.dispatch(zeraCalendario());
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    let isSubscribed = true;
    (async () => {
      setCarregandoTipos(true);
      setCarregandoMeses(true);

      const {
        data,
      } = await ServicoCalendarios.obterTiposCalendarioAutoComplete(
        pesquisaTipoCalendario
      );

      if (isSubscribed) {
        setListaTipoCalendario(data);
        setCarregandoTipos(false);
        setCarregandoMeses(false);
      }
    })();

    return () => {
      isSubscribed = false;
    };
  }, [pesquisaTipoCalendario]);

  const selecionaTipoCalendario = descricao => {
    const tipo = listaTipoCalendario?.find(t => t.descricao === descricao);
    if (Number(tipo?.id) || !tipo?.id) {
      store.dispatch(zeraCalendario());
      setValorTipoCalendario(descricao);
    }
    setTipoCalendarioSelecionado(tipo?.id);
  };

  const handleSearch = descricao => {
    if (descricao.length > 3 || descricao.length === 0) {
      setPesquisaTipoCalendario(descricao);
    }
  };

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Calendário escolar</Titulo>
      </Grid>
      <Card className="rounded mb-4 mx-auto">
        <Grid cols={12} className="mb-4">
          <Div className="row">
            <Grid cols={4}>
              <Loader loading={carregandoTipos} tip="">
                <SelectAutocomplete
                  hideLabel
                  showList
                  isHandleSearch
                  placeholder="Tipo de calendário"
                  className="col-md-12"
                  name="tipoCalendarioId"
                  id="tipoCalendarioId"
                  lista={listaTipoCalendario}
                  valueField="id"
                  textField="descricao"
                  onSelect={selecionaTipoCalendario}
                  onChange={selecionaTipoCalendario}
                  handleSearch={handleSearch}
                  value={valorTipoCalendario}
                />
              </Loader>
            </Grid>
            <Grid cols={4}>
              {diasLetivos && diasLetivos.dias && (
                <Div>
                  <Button
                    id={shortid.generate()}
                    label={diasLetivos.dias.toString()}
                    color={
                      diasLetivos.estaAbaixoPermitido
                        ? Colors.Vermelho
                        : Colors.Verde
                    }
                    className="float-left"
                  />
                  <Div className="float-left w-50 ml-2 mt-1">
                    Nº de dias letivos no calendário
                  </Div>
                </Div>
              )}
              {diasLetivos && diasLetivos.estaAbaixoPermitido && (
                <Div
                  className="clearfix font-weight-bold pt-2"
                  style={{ clear: 'both', color: Base.Vermelho, fontSize: 12 }}
                >
                  <Icone className="fa fa-exclamation-triangle mr-2" />
                  Abaixo do mínimo estabelecido pela legislação
                </Div>
              )}
            </Grid>
            <Grid cols={4}>
              <Button
                id={shortid.generate()}
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                onClick={aoClicarBotaoVoltar}
                border
                className="ml-auto"
              />
            </Grid>
          </Div>
        </Grid>
        <Grid cols={12} className="mb-4">
          <Div className="row">
            <Grid cols={1} className="d-flex align-items-center pr-0">
              <Div className="w-100">
                <Tooltip
                  placement="top"
                  title={`${
                    eventoSme
                      ? 'Exibindo eventos da SME'
                      : 'Não exibindo eventos da SME'
                  }`}
                >
                  <Switch
                    onChange={aoTrocarEventoSme}
                    checked={eventoSme}
                    size="small"
                    className="mr-2"
                    disabled={!tipoCalendarioSelecionado}
                  />
                  <Label className="my-auto">SME</Label>
                </Tooltip>
              </Div>
            </Grid>
            <Grid cols={6}>
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  className="fonte-14"
                  onChange={aoSelecionarDre}
                  lista={dres}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={dreSelecionada}
                  placeholder="Diretoria Regional de Educação (DRE)"
                  disabled={!tipoCalendarioSelecionado || dres.length < 2}
                />
              </Loader>
            </Grid>
            <Grid cols={5}>
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  className="fonte-14"
                  onChange={aoSelecionarUnidadeEscolar}
                  lista={unidadesEscolares}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={unidadeEscolarSelecionada}
                  placeholder="Unidade Escolar (UE)"
                  disabled={!dreSelecionada}
                />
              </Loader>
            </Grid>
          </Div>
        </Grid>
        <Grid cols={12}>
          <Loader loading={carregandoMeses}>
            <Calendario filtros={filtros} />
          </Loader>
        </Grid>
      </Card>
    </Div>
  );
};

export default CalendarioEscolar;
