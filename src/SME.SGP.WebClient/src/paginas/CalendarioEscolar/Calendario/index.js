import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Calendario from '~/componentes-sgp/Calendario/Calendario';
import { Base, Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import { erro } from '~/servicos/alertas';
import { store } from '~/redux';
import { zeraCalendario } from '~/redux/modulos/calendarioEscolar/actions';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;
const Icone = styled.i``;

const CalendarioEscolar = () => {
  const [tiposCalendario, setTiposCalendario] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState();

  const [diasLetivos, setDiasLetivos] = useState({});

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);

  useEffect(() => {
    if (turmaSelecionada) {
      const tiposCalendarioLista = [];
      api
        .get('v1/calendarios/tipos')
        .then(resposta => {
          if (resposta.data) {
            resposta.data
              .filter(
                tipo =>
                  tipo.anoLetivo === turmaSelecionada.anoLetivo &&
                  ((tipo.modalidade === 1 &&
                    (turmaSelecionada.modalidade === '5' ||
                      turmaSelecionada.modalidade === '6')) ||
                    (tipo.modalidade === 2 &&
                      turmaSelecionada.modalidade === '3'))
              )
              .forEach(tipo => {
                tiposCalendarioLista.push({ desc: tipo.nome, valor: tipo.id });
              });
            setTiposCalendario(tiposCalendarioLista);
          } else setTiposCalendario([]);
        })
        .catch(() => {
          setTiposCalendario([]);
        });
    } else {
      erro('Você precisa escolher uma turma');
    }
    return () => store.dispatch(zeraCalendario());
  }, []);

  useEffect(() => {
    let estado = true;
    if (estado) {
      if (tipoCalendarioSelecionado) {
        api
          .get(
            `https://demo0765509.mockable.io/api/v1/calendarios/${tipoCalendarioSelecionado}/dias-letivos`
          )
          .then(resposta => {
            if (resposta.data) setDiasLetivos(resposta.data);
          })
          .catch(() => {
            setDiasLetivos({});
          });
      }
      setFiltros({ ...filtros, tipoCalendarioSelecionado });
    }
    return () => (estado = false);
  }, [tipoCalendarioSelecionado]);

  const aoSelecionarTipoCalendario = tipo => {
    setTipoCalendarioSelecionado(tipo);
  };

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const [eventoSme, setEventoSme] = useState(true);

  const aoClicarEventoSme = () => {
    setEventoSme(!eventoSme);
    setFiltros({ ...filtros, eventoSme: !eventoSme });
  };

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState();

  useEffect(() => {
    if (dresStore) setDres(dresStore);
  }, [dresStore]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState();

  useEffect(() => {
    if (unidadesEscolaresStore) setUnidadesEscolares(unidadesEscolaresStore);
  }, [unidadesEscolaresStore]);

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
    setFiltros({ ...filtros, dreSelecionada: dre });
  };

  const aoSelecionarUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
    setFiltros({ ...filtros, unidadeEscolarSelecionada: unidade });
  };

  const [filtros, setFiltros] = useState({
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
  });

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">
          Consulta de calendário escolar
        </Titulo>
      </Grid>
      <Card className="rounded mb-4">
        <Grid cols={12} className="mb-4">
          <Div className="row">
            <Grid cols={4}>
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarTipoCalendario}
                lista={tiposCalendario}
                valueOption="valor"
                valueText="desc"
                valueSelect={tipoCalendarioSelecionado}
                placeholder="Tipo de Calendário"
              />
            </Grid>
            <Grid cols={4}>
              {diasLetivos && diasLetivos.DiasLetivos && (
                <Div>
                  <Button
                    label={diasLetivos.DiasLetivos.toString()}
                    color={
                      diasLetivos.EstaAbaixoPermitido
                        ? Colors.Vermelho
                        : Colors.Verde
                    }
                    className="float-left"
                  />
                  <Div className="float-left w-50 ml-2 mt-1">
                    Nº de Dias Letivos no Calendário
                  </Div>
                </Div>
              )}
              {diasLetivos && diasLetivos.EstaAbaixoPermitido && (
                <Div
                  className="clearfix font-weight-bold pt-2"
                  style={{ clear: 'both', color: Base.Vermelho, fontSize: 12 }}
                >
                  <Icone className="fa fa-exclamation-triangle mr-2" />
                  Abaixo do mínimo estabelecido pela legislação.
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
            <Grid cols={2}>
              <Button
                label="SME"
                color={eventoSme ? Colors.Verde : Colors.Vermelho}
                onClick={aoClicarEventoSme}
              />
            </Grid>
            <Grid cols={5}>
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
            </Grid>
            <Grid cols={5}>
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarUnidadeEscolar}
                lista={unidadesEscolares}
                valueOption="valor"
                valueText="desc"
                valueSelect={unidadeEscolarSelecionada}
                placeholder="Unidade Escolar (UE)"
                disabled={!tipoCalendarioSelecionado}
              />
            </Grid>
          </Div>
        </Grid>
        <Grid cols={12}>
          <Calendario filtros={filtros} />
        </Grid>
      </Card>
    </Div>
  );
};

export default CalendarioEscolar;
