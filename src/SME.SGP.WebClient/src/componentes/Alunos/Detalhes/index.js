import { Avatar, Card } from 'antd';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { Container, DadosAluno, FrequenciaGlobal } from './styles';

const DetalhesAluno = props => {
  const { dados, desabilitarImprimir, onClickImprimir } = props;

  const {
    avatar,
    nome,
    numeroChamada,
    dataNascimento,
    codigoEOL,
    situacao,
    dataSituacao,
    frequencia,
  } = dados;

  return (
    <Container>
      <Card
        type="inner"
        className="rounded"
        headStyle={{ borderBottomRightRadius: 0 }}
        bodyStyle={{ borderTopRightRadius: 0 }}
      >
        <DadosAluno className="row">
          <div className="col-md-8 d-flex justify-content-start">
            <Avatar className="mr-2" size={80} icon="user" src={avatar} />
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
            </div>
          </div>
          <div className="col-md-4 d-flex justify-content-end display-block">
            <Button
              icon="print"
              className="ml-auto mb-4"
              color={Colors.Azul}
              border
              onClick={onClickImprimir}
              disabled={desabilitarImprimir}
              id="btn-imprimir-dados-aluno"
            />
            <FrequenciaGlobal>
              Frequência Global: {frequencia || 0}%
            </FrequenciaGlobal>
          </div>
        </DadosAluno>
      </Card>
    </Container>
  );
};

DetalhesAluno.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  desabilitarImprimir: PropTypes.oneOfType([PropTypes.bool]),
  onClickImprimir: PropTypes.oneOfType([PropTypes.func]),
};

DetalhesAluno.defaultProps = {
  dados: [],
  desabilitarImprimir: true,
  onClickImprimir: () => {},
};

export default DetalhesAluno;
