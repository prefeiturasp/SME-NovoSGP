import React, { useState, useRef } from 'react';
import styled from 'styled-components';
import { Radio } from 'antd';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import { Colors, Base } from '~/componentes/colors';
import history from '~/servicos/history';
import SelectComponent from '~/componentes/select';

const CadastroTipoEventos = () => {
  const [concomitancia, setConcomitancia] = useState(true);
  const [tipoData, setTipoData] = useState(true);
  const [dependencia, setDependencia] = useState(false);
  const [situacao, setSituacao] = useState(true);

  const [letivoSelecionado, setLetivoSelecionado] = useState();
  const [
    localOcorrenciaSelecionado,
    setLocalOcorrenciaSelecionado,
  ] = useState();

  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '02/05/2019 às 20:28',
    alteradoPor: 'ELISANGELA DOS SANTOS ARRUDA (RF 1234567)',
    criadoEm: '02/05/2019 às 20:28',
    criadoPor: 'ELISANGELA DOS SANTOS ARRUDA (RF 1234567)',
  });

  const Div = styled.div`
    .ant-radio-checked .ant-radio-inner {
      border-color: ${Base.Roxo};
    }
    .ant-radio-inner::after {
      background-color: ${Base.Roxo};
    }
  `;

  const Titulo = styled(Div)`
    color: ${Base.CinzaMako};
    font-size: 24px;
  `;

  const Rotulo = styled.label`
    color: ${Base.CinzaMako};
    font-size: 14px;
    font-weight: bold;
  `;

  const CampoTexto = styled.input`
    color: ${Base.CinzaBotao};
    font-size: 14px;
  `;

  const InseridoAlterado = styled(Div)`
    color: ${Base.CinzaMako};
    font-size: 10px;
    font-weight: bold;
    p {
      margin: 0;
    }
  `;

  const botaoCadastrarRef = useRef();

  const clicouBotaoVoltar = () => {
    history.push('/calendario/tipo-eventos-lista');
  };

  const clicouBotaoCancelar = () => {
    history.push('/');
  };

  const clicouBotaoCadastrar = () => {
    history.push('/');
  };

  const aoSelecionarLocalOcorrencia = local => {
    setLocalOcorrenciaSelecionado(local);
  };

  const aoSelecionarLetivo = letivo => {
    setLetivoSelecionado(letivo);
  };

  const aoSelecionarConcomitancia = () => {
    setConcomitancia(!concomitancia);
  };

  const aoSelecionarTipoData = () => {
    setTipoData(!tipoData);
  };

  const aoSelecionarDependencia = () => {
    setDependencia(!dependencia);
  };

  const aoSelecionarSituacao = () => {
    setSituacao(!situacao);
  };

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Cadastro de tipo de evento</Titulo>
      </Grid>
      <Card className="rounded" mx="mx-auto">
        <Grid cols={12} className="d-flex justify-content-end mb-3">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            onClick={clicouBotaoVoltar}
            border
            className="mr-3"
          />
          <Button
            label="Cancelar"
            color={Colors.Roxo}
            onClick={clicouBotaoCancelar}
            border
            bold
            className="mr-3"
          />
          <Button
            label="Cadastrar"
            color={Colors.Roxo}
            onClick={clicouBotaoCadastrar}
            disabled
            border
            bold
            ref={botaoCadastrarRef}
          />
        </Grid>
        <Grid cols={12}>
          <Div className="row mb-4">
            <Div className="col-6">
              <Rotulo>Nome do tipo de evento</Rotulo>
              <CampoTexto
                className="form-control form-control-lg"
                placeholder="Nome do evento"
                maxLength={100}
              />
            </Div>
            <Div className="col-4">
              <Rotulo>Local de ocorrência</Rotulo>
              <SelectComponent
                placeholder="Local de ocorrência"
                valueOption="valor"
                valueText="descricao"
                lista={[
                  { valor: 0, descricao: 'SME' },
                  { valor: 1, descricao: 'DRE' },
                  { valor: 2, descricao: 'SME/UE' },
                  { valor: 3, descricao: 'Todos' },
                ]}
                valueSelect={localOcorrenciaSelecionado}
                onChange={aoSelecionarLocalOcorrencia}
              />
            </Div>
            <Div className="col-2">
              <Rotulo>Letivo</Rotulo>
              <SelectComponent
                placeholder="Tipo"
                valueOption="valor"
                valueText="descricao"
                lista={[
                  { valor: 0, descricao: 'Sim' },
                  { valor: 1, descricao: 'Não' },
                  { valor: 2, descricao: 'Opcional' },
                ]}
                valueSelect={letivoSelecionado}
                onChange={aoSelecionarLetivo}
              />
            </Div>
          </Div>
          <Div className="row">
            <Div className="col-3">
              <Rotulo>Permite concomitância</Rotulo>
            </Div>
            <Div className="col-3">
              <Rotulo>Tipo de data</Rotulo>
            </Div>
            <Div className="col-3">
              <Rotulo>Dependência</Rotulo>
            </Div>
            <Div className="col-3">
              <Rotulo>Situação</Rotulo>
            </Div>
          </Div>
          <Div className="row">
            <Div className="col-3">
              <Radio.Group
                value={concomitancia}
                onChange={aoSelecionarConcomitancia}
              >
                <Div className="form-check form-check-inline">
                  <Radio value>Sim</Radio>
                </Div>
                <Div className="form-check form-check-inline">
                  <Radio value={false}>Não</Radio>
                </Div>
              </Radio.Group>
            </Div>
            <Div className="col-3">
              <Radio.Group value={tipoData} onChange={aoSelecionarTipoData}>
                <Div className="form-check form-check-inline">
                  <Radio value>Única</Radio>
                </Div>
                <Div className="form-check form-check-inline">
                  <Radio value={false}>Início e fim</Radio>
                </Div>
              </Radio.Group>
            </Div>
            <Div className="col-3">
              <Radio.Group
                value={dependencia}
                onChange={aoSelecionarDependencia}
              >
                <Div className="form-check form-check-inline">
                  <Radio value>Sim</Radio>
                </Div>
                <Div className="form-check form-check-inline">
                  <Radio value={false}>Não</Radio>
                </Div>
              </Radio.Group>
            </Div>
            <Div className="col-3">
              <Radio.Group value={situacao} onChange={aoSelecionarSituacao}>
                <Div className="form-check form-check-inline">
                  <Radio value>Ativo</Radio>
                </Div>
                <Div className="form-check form-check-inline">
                  <Radio value={false}>Inativo</Radio>
                </Div>
              </Radio.Group>
            </Div>
          </Div>
        </Grid>
        <Grid cols={12}>
          <InseridoAlterado className="mt-4">
            {inseridoAlterado.criadoPor && inseridoAlterado.criadoEm ? (
              <p className="pt-2">
                INSERIDO por {inseridoAlterado.criadoPor} em{' '}
                {inseridoAlterado.criadoEm}
              </p>
            ) : (
              ''
            )}

            {inseridoAlterado.alteradoPor && inseridoAlterado.alteradoEm ? (
              <p>
                ALTERADO por {inseridoAlterado.alteradoPor} em{' '}
                {inseridoAlterado.alteradoEm}
              </p>
            ) : (
              ''
            )}
          </InseridoAlterado>
        </Grid>
      </Card>
    </Div>
  );
};

export default CadastroTipoEventos;
