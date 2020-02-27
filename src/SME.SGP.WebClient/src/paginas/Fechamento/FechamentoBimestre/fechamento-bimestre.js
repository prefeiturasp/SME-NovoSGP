import { Tabs } from 'antd';
import React, { useEffect, useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Colors, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import FechamentoBimestreLista from './fechamento-bimestre-lista/fechamento-bimestre-lista';
import RotasDto from '~/dtos/rotasDto';
import { Fechamento } from './fechamento-bimestre.css';
import ServicoFechamentoBimestre from '~/servicos/Paginas/Fechamento/ServicoFechamentoBimestre';
import { erro } from '~/servicos/alertas';

const FechamentoBismestre = () => {
  const { TabPane } = Tabs;
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada, permissoes } = usuario;
  const permissoesTela = permissoes[RotasDto.FechamentoBismestre];
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const [carregandoBimestres, setCarregandoBimestres] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(null);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(
    listaDisciplinas && listaDisciplinas.length === 1
  );
  const [modoEdicao] = useState(false);
  const [bimestreCorrente, setBimestreCorrente] = useState('1');
  const [dadosBimestre1, setDadosBimestre1] = useState(undefined);
  const [dadosBimestre2, setDadosBimestre2] = useState(undefined);
  const [dadosBimestre3, setDadosBimestre3] = useState(undefined);
  const [dadosBimestre4, setDadosBimestre4] = useState(undefined);
  const [ehRegencia, setEhRegencia] = useState(false);

  const onChangeDisciplinas = id => {
    const disciplina = listaDisciplinas.find(
      c => String(c.codigoComponenteCurricular) === id
    );
    setEhRegencia(disciplina.regencia);
    setDisciplinaIdSelecionada(id);
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {};

  const onClickSalvar = () => {};

  useEffect(() => {
    const obterDisciplinas = async () => {
      if (turmaSelecionada && turmaSelecionada.turma) {
        const lista = await ServicoDisciplina.obterDisciplinasPorTurma(
          turmaSelecionada.turma
        );
        setListaDisciplinas(lista.data);
        if (lista.data.length === 1) {
          setDisciplinaIdSelecionada(
            String(lista.data[0].codigoComponenteCurricular)
          );
          setEhRegencia(lista.data[0].regencia);
          setDesabilitarDisciplina(true);
        }
      }
    };
    obterDisciplinas();
  }, [turmaSelecionada]);

  const setDadosBimestre = (bimestre, dados) => {
    switch (bimestre) {
      case 1:
        setDadosBimestre1(undefined);
        setDadosBimestre1(dados);
        break;
      case 2:
        setDadosBimestre2(undefined);
        setDadosBimestre2(dados);
        break;
      case 3:
        setDadosBimestre3(undefined);
        setDadosBimestre3(dados);
        break;
      case 4:
        setDadosBimestre4(undefined);
        setDadosBimestre4(dados);
        break;
      default:
        break;
    }
  };

  const obterDados = useCallback(
    async (bimestre = 0) => {
      setCarregandoBimestres(true);

      try {
        const fechamento = await ServicoFechamentoBimestre.buscarDados(
          turmaSelecionada.turma,
          disciplinaIdSelecionada,
          bimestre
        );

        if (fechamento && fechamento.data) {
          const dadosFechamento = fechamento.data;
          setBimestreCorrente(`${dadosFechamento.bimestre}`);
          setDadosBimestre(dadosFechamento.bimestre, dadosFechamento);
        }
      } catch (error) {
        if (error.response) {
          const { data } = error.response;
          if (data && data.mensagens.length) {
            erro(data.mensagens[0]);
          }
        }
      }

      setCarregandoBimestres(false);
    },
    [disciplinaIdSelecionada, turmaSelecionada.turma]
  );

  useEffect(() => {
    if (disciplinaIdSelecionada) obterDados();
  }, [disciplinaIdSelecionada, obterDados]);

  const onChangeTab = async numeroBimestre => {
    setBimestreCorrente(numeroBimestre);
    if (numeroBimestre !== 'final') {
      obterDados(numeroBimestre);
    }
  };

  return (
    <>
      {!turmaSelecionada.turma ? (
        <Grid cols={12} className="p-0">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'AlertaTurmaFechamentoBimestre',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </Grid>
      ) : null}
      <Cabecalho pagina="Fechamento" />
      <Loader loading={carregandoBimestres}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id={shortid.generate()}
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  id={shortid.generate()}
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  className="mr-2"
                  onClick={onClickCancelar}
                  disabled={!modoEdicao || somenteConsulta}
                />
                <Button
                  id={shortid.generate()}
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={onClickSalvar}
                  disabled={!modoEdicao || somenteConsulta}
                />
              </div>
            </div>
          </div>
          <div className="col-md-12">
            <div className="row">
              <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-4">
                <SelectComponent
                  id="disciplina"
                  name="disciplinaId"
                  lista={listaDisciplinas}
                  valueOption="codigoComponenteCurricular"
                  valueText="nome"
                  valueSelect={disciplinaIdSelecionada}
                  onChange={onChangeDisciplinas}
                  placeholder="Selecione um componente curricular"
                  disabled={desabilitarDisciplina || !turmaSelecionada.turma}
                />
              </div>
            </div>
          </div>
          <div className="col-md-12">
            <div className="row">
              <Fechamento className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                <ContainerTabsCard
                  type="card"
                  onChange={onChangeTab}
                  activeKey={bimestreCorrente}
                >
                  <TabPane tab="1º Bimestre" key="1">
                    {dadosBimestre1 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre1}
                        ehRegencia={ehRegencia}
                      />
                    ) : null}
                  </TabPane>

                  <TabPane tab="2º Bimestre" key="2">
                    {dadosBimestre2 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre2}
                        ehRegencia={ehRegencia}
                      />
                    ) : null}
                  </TabPane>

                  <TabPane tab="3º Bimestre" key="3">
                    {dadosBimestre3 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre3}
                        ehRegencia={ehRegencia}
                      />
                    ) : null}
                  </TabPane>

                  <TabPane tab="4º Bimestre" key="4">
                    {dadosBimestre4 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre4}
                        ehRegencia={ehRegencia}
                      />
                    ) : null}
                  </TabPane>

                  <TabPane tab="Final" key="final" />
                </ContainerTabsCard>
              </Fechamento>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default FechamentoBismestre;
