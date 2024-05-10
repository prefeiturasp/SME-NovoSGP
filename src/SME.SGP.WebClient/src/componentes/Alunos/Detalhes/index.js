import { Card } from 'antd';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import { SinalizacaoAEE } from '~/componentes-sgp';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import FotoEstudanteObjectCard from './imagemEstudanteObjectCard';
import { Container, DadosAluno, FrequenciaGlobal } from './styles';

const DetalhesAluno = props => {
  const {
    dados,
    desabilitarImprimir,
    onClickImprimir,
    exibirBotaoImprimir,
    exibirFrequencia,
    exibirResponsavel,
    permiteAlterarImagem,
  } = props;

  const {
    nome,
    numeroChamada,
    dataNascimento,
    codigoEOL,
    situacao,
    dataSituacao,
    frequencia,
    nomeResponsavel,
    tipoResponsavel,
    celularResponsavel,
    dataAtualizacaoContato,
    turma,
  } = dados;

  const numeroLinhas = () => {
    if (
      nomeResponsavel &&
      exibirResponsavel &&
      (exibirBotaoImprimir || exibirFrequencia)
    ) {
      return 6;
    }

    if (!exibirResponsavel) {
      return 12;
    }

    return 8;
  };

  return (
    <Container>
      <Card
        type="inner"
        className="rounded"
        headStyle={{ borderBottomRightRadius: 0 }}
        bodyStyle={{ borderTopRightRadius: 0 }}
      >
        <DadosAluno className="row">
          <div
            className={`col-md-${numeroLinhas()} d-flex justify-content-start`}
            style={{
              borderRight:
                nomeResponsavel && exibirResponsavel
                  ? `1px solid ${Base.CinzaDesabilitado}`
                  : 'none',
            }}
          >
            <FotoEstudanteObjectCard
              codigoEOL={codigoEOL}
              permiteAlterarImagem={permiteAlterarImagem}
            />
            <div>
              <p>
                {nome} Nº {numeroChamada}
              </p>
              <p>
                Data de nascimento:{' '}
                {dataNascimento ? moment(dataNascimento).format('L') : ''}
              </p>
              <p>Código EOL: {codigoEOL}</p>
              <p>
                Situação: {situacao} em{' '}
                {dataSituacao ? moment(dataSituacao).format('L') : ''}{' '}
                {dataSituacao ? moment(dataSituacao).format('LT') : ''}
              </p>
              {turma ? <p>Turma: {turma}</p> : ''}
            </div>
          </div>
          {nomeResponsavel && exibirResponsavel ? (
            <div className="col-md-4">
              <div>
                <p>
                  Responsável: {nomeResponsavel}
                  <span
                    style={{ color: Base.CinzaDesabilitado, fontSize: '13px' }}
                  >{` (${tipoResponsavel})`}</span>
                </p>
                <p>
                  Telefone: {celularResponsavel}
                  <span
                    style={{ color: Base.CinzaDesabilitado, fontSize: '13px' }}
                  >{` (Atualizado - ${
                    dataAtualizacaoContato
                      ? moment(dataAtualizacaoContato).format('L')
                      : ''
                  })`}</span>
                </p>
              </div>
              <div>
                <SinalizacaoAEE exibirSinalizacao={dados?.ehAtendidoAEE} />
              </div>
            </div>
          ) : (
            ''
          )}
          {exibirBotaoImprimir || exibirFrequencia ? (
            <div
              className={`col-md-${
                nomeResponsavel ? '2' : '4'
              } d-flex justify-content-end display-block`}
            >
              {exibirBotaoImprimir ? (
                <Button
                  icon="print"
                  className="ml-auto mb-4"
                  color={Colors.Azul}
                  border
                  onClick={onClickImprimir}
                  disabled={desabilitarImprimir}
                  id="btn-imprimir-dados-aluno"
                />
              ) : (
                ''
              )}
              {exibirFrequencia ? (
                <FrequenciaGlobal>
                  Frequência Global: {frequencia || 0}%
                </FrequenciaGlobal>
              ) : (
                ''
              )}
            </div>
          ) : (
            ''
          )}
        </DadosAluno>
      </Card>
    </Container>
  );
};

DetalhesAluno.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  desabilitarImprimir: PropTypes.oneOfType([PropTypes.bool]),
  onClickImprimir: PropTypes.oneOfType([PropTypes.func]),
  exibirBotaoImprimir: PropTypes.oneOfType([PropTypes.bool]),
  exibirFrequencia: PropTypes.oneOfType([PropTypes.bool]),
  exibirResponsavel: PropTypes.bool,
  permiteAlterarImagem: PropTypes.bool,
};

DetalhesAluno.defaultProps = {
  dados: [],
  desabilitarImprimir: true,
  onClickImprimir: () => {},
  exibirBotaoImprimir: true,
  exibirFrequencia: true,
  exibirResponsavel: true,
  permiteAlterarImagem: true,
};

export default DetalhesAluno;
