import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { Tooltip, Switch } from 'antd';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Calendario from '~/componentes-sgp/calendarioEscolar/Calendario';
import { Base, Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import { store } from '~/redux';
import {
  zeraCalendario,
  atribuiEventosMes,
} from '~/redux/modulos/calendarioEscolar/actions';
import ModalidadeDTO from '~/dtos/modalidade';
import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import ServicoCalendarios from '~/servicos/Paginas/Calendario/ServicoCalendarios';
import { Loader } from '~/componentes';
import { erro } from '~/servicos/alertas';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;
const Icone = styled.i``;
const Label = styled.label``;

const CalendarioEscolar = () => {
  const [tiposCalendario, setTiposCalendario] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    undefined
  );

  const [diasLetivos, setDiasLetivos] = useState({});
  const turmaSelecionadaStore = useSelector(
    state => state.usuario.turmaSelecionada
  );

  const [controleTurmaSelecionada, setControleTurmaSelecionada] = useState();

  const modalidadesAbrangencia = useSelector(state => state.filtro.modalidades);
  const anosLetivosAbrangencia = useSelector(state => state.filtro.anosLetivos);

  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterTiposCalendario = async modalidades => {
    setCarregandoTipos(true);
    const lista = await ServicoCalendarios.obterTiposCalendario(
      turmaSelecionadaStore.anoLetivo
    );
    if (lista && lista.data) {
      const tiposCalendarioLista = [];
      if (lista.data) {
        const anos = [];
        anosLetivosAbrangencia.forEach(ano => {
          if (!anos.includes(ano.valor)) anos.push(ano.valor);
        });
        const tipos = lista.data
          .filter(tipo => {
            return anos.indexOf(tipo.anoLetivo) > -1;
          })
          .filter(tipo => {
            if (Object.entries(turmaSelecionadaStore).length)
              return modalidades.indexOf(tipo.modalidade) > -1;
            return true;
          });
        tipos.forEach(tipo => {
          tiposCalendarioLista.push({
            desc: tipo.nome,
            valor: tipo.id,
            modalidade: tipo.modalidade,
          });
        });
      }
      setCarregandoTipos(false);
      return tiposCalendarioLista;
    }
    setCarregandoTipos(false);
    return lista;
  };

  const listarModalidadesPorAbrangencia = () => {
    const modalidades = [];
    if (modalidadesAbrangencia) {
      modalidadesAbrangencia.forEach(modalidade => {
        if (
          (modalidade.valor === ModalidadeDTO.FUNDAMENTAL ||
            modalidade.valor === ModalidadeDTO.ENSINO_MEDIO) &&
          !modalidades.includes(1)
        )
          modalidades.push(1);
        if (modalidade.valor === ModalidadeDTO.EJA && !modalidades.includes(2))
          modalidades.push(2);
      });
    }
    return modalidades;
  };

  const listarTiposCalendarioPorTurmaSelecionada = async tiposLista => {
    if (Object.entries(turmaSelecionadaStore).length) {
      const modalidadeSelecionada =
        turmaSelecionadaStore.modalidade === ModalidadeDTO.EJA.toString()
          ? 2
          : 1;

      if (tiposLista && tiposLista.length) {
        setTiposCalendario(
          tiposLista.filter(tipo => {
            return tipo.modalidade === modalidadeSelecionada;
          })
        );
      } else {
        const tipos = await obterTiposCalendario(
          listarModalidadesPorAbrangencia()
        );

        if (!tipos || tipos.length === 0) {
          erro(
            'Nenhum tipo de calendário encontrado para o ano letivo e modalidade selecionada'
          );
          return;
        }

        setTiposCalendario(
          tipos.filter(tipo => {
            return tipo.modalidade === modalidadeSelecionada;
          })
        );
      }
    }
  };

  useEffect(() => {
    if (tiposCalendario.length && tiposCalendario.length === 1) {
      if (Object.entries(turmaSelecionadaStore).length) {
        const modalidadeSelecionada =
          turmaSelecionadaStore.modalidade === ModalidadeDTO.EJA.toString()
            ? 2
            : 1;
        const tipoCalendario = tiposCalendario.filter(tipo => {
          return tipo.modalidade === modalidadeSelecionada;
        })[0];
        if (tipoCalendario) {
          setTipoCalendarioSelecionado(tipoCalendario.valor.toString());
        }
      }
    }
  }, [tiposCalendario, turmaSelecionadaStore]);

  const listarTiposCalendario = async () => {
    listarTiposCalendarioPorTurmaSelecionada(
      await obterTiposCalendario(listarModalidadesPorAbrangencia())
    );
  };

  const eventoCalendarioEdicao = useSelector(
    state => state.calendarioEscolar.eventoCalendarioEdicao
  );

  useEffect(() => {
    listarTiposCalendario();
    return () => store.dispatch(zeraCalendario());
  }, []);

  const [eventoSme, setEventoSme] = useState(true);

  useEffect(() => {
    if (
      tiposCalendario &&
      eventoCalendarioEdicao &&
      eventoCalendarioEdicao.tipoCalendario
    ) {
      setTipoCalendarioSelecionado(eventoCalendarioEdicao.tipoCalendario);
      if (eventoCalendarioEdicao.eventoSme) {
        setEventoSme(eventoCalendarioEdicao.eventoSme);
      }
    }
  }, [tiposCalendario]);

  useEffect(() => {
    if (
      turmaSelecionadaStore &&
      controleTurmaSelecionada === turmaSelecionadaStore.turma
    )
      return;

    setControleTurmaSelecionada(turmaSelecionadaStore.turma);
    setTipoCalendarioSelecionado('');

    if (turmaSelecionadaStore.turma) listarTiposCalendarioPorTurmaSelecionada();
  }, [turmaSelecionadaStore]);

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

  const aoSelecionarTipoCalendario = tipo => {
    store.dispatch(zeraCalendario());
    setTipoCalendarioSelecionado(tipo);
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
  }, [tipoCalendarioSelecionado]);

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const aoTrocarEventoSme = () => {
    setEventoSme(!eventoSme);
  };

  useEffect(() => {
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
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
  }, [dres, carregandoMeses]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);

  const obterUnidadesEscolares = () => {
    setCarregandoUes(true);
    api
      .get(`v1/abrangencias/false/dres/${dreSelecionada}/ues`)
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(unidade => {
              lista.push({
                desc: `${tipoEscolaDTO[unidade.tipoEscola]} ${unidade.nome}`,
                valor: unidade.codigo,
              });
            });
            setUnidadesEscolares(lista.sort(FiltroHelper.ordenarLista('desc')));
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
  }, [unidadesEscolares, carregandoMeses]);

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
  };

  useEffect(() => {
    if (dreSelecionada) {
      consultarDiasLetivos();
      obterUnidadesEscolares();
    } else {
      setUnidadeEscolarSelecionada();
    }
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
    });
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
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    let estado = true;
    if (estado) {
      if (tipoCalendarioSelecionado) {
        setCarregandoMeses(true);
        api
          .get(
            `v1/calendarios/eventos/meses?EhEventoSme=${eventoSme}&${
              dreSelecionada ? `DreId=${dreSelecionada}&` : ''
            }${
              tipoCalendarioSelecionado
                ? `IdTipoCalendario=${tipoCalendarioSelecionado}&`
                : ''
            }${
              unidadeEscolarSelecionada
                ? `UeId=${unidadeEscolarSelecionada}`
                : ''
            }`
          )
          .then(resposta => {
            if (resposta.data) {
              resposta.data.forEach(item => {
                if (item && item.mes > 0) {
                  store.dispatch(atribuiEventosMes(item.mes, item.eventos));
                }
              });
            }
            setCarregandoMeses(false);
          });
      }
    }
    return () => {
      estado = false;
    };
  }, [
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
  ]);

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
                <SelectComponent
                  className="fonte-14"
                  onChange={aoSelecionarTipoCalendario}
                  lista={tiposCalendario}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={tipoCalendarioSelecionado}
                  placeholder="Tipo de Calendário"
                />
              </Loader>
            </Grid>
            <Grid cols={4}>
              {diasLetivos && diasLetivos.dias && (
                <Div>
                  <Button
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
                  disabled={!tipoCalendarioSelecionado}
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
