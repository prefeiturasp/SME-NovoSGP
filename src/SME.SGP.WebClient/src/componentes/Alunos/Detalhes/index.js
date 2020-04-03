import React from 'react';
import PropTypes from 'prop-types';

// Componentes
import { Card, Avatar } from 'antd';
import Row from '~/componentes/row';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';

// Styles
import { Botoes, Container } from './styles';

const DetalhesAluno = ({ dados, onAnterior, onProximo, print, onPrint }) => {
  const Titulo = () => {
    return (
      <Row className="font-weight-bold">
        <Grid cols={6} className="d-flex align-items-center">
          Detalhes do estudante
        </Grid>
        <Botoes cols={6} className="d-flex justify-content-end">
          <Button
            label="Anterior"
            color={Colors.Roxo}
            className="ml-auto attached left"
            height="48px"
            width="100px"
            bold
            border
            onClick={onAnterior}
          />
          <Button
            label="Próximo"
            color={Colors.Roxo}
            className="ml-0 attached border-left-0 right"
            height="48px"
            width="100px"
            bold
            border
            onClick={onProximo}
          />
        </Botoes>
      </Row>
    );
  };

  // Exemplo de dados do componente
  // dados={{
  //   nome: 'ALANA FERREIRA DE OLIVEIRA',
  //   numero: 1,
  //   dataNascimento: '02/02/2020',
  //   codigoEOL: 4241513,
  //   situacao: 'Matriculado',
  //   dataSituacao: '04/02/2019',
  //   frequencia: 96,
  // }}

  const {
    avatar,
    nome,
    numero,
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
        title={Titulo()}
      >
        <Grid cols={12}>
          <Row className="fonte-14">
            <Grid cols={6}>
              <Row className="fonte-12">
                <Grid cols={2} className="pr-0 mr-0">
                  <Avatar
                    size={64}
                    icon="user"
                    style={{ verticalAlign: 0 }}
                    src={avatar}
                  />
                </Grid>
                <Grid cols={10} className="pl-0 ml-0">
                  <div>
                    {nome} Nº {numero}
                  </div>
                  <div>Data de nascimento: {dataNascimento}</div>
                  <div>Código EOL: {codigoEOL}</div>
                  <div>
                    Situação: {situacao} em {dataSituacao}
                  </div>
                </Grid>
              </Row>
            </Grid>
            <Grid cols={6} className="text-right">
              {print && (
                <Button
                  icon="print"
                  className="ml-auto mb-4"
                  color={Colors.Azul}
                  border
                  onClick={onPrint}
                />
              )}
              <div className="d-block font-weight-bold fonte-12">
                Frequência Global: {frequencia}%
              </div>
            </Grid>
          </Row>
        </Grid>
      </Card>
    </Container>
  );
};

DetalhesAluno.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onAnterior: PropTypes.oneOfType([PropTypes.func]),
  onProximo: PropTypes.oneOfType([PropTypes.func]),
  print: PropTypes.oneOfType([PropTypes.bool]),
  onPrint: PropTypes.oneOfType([PropTypes.func]),
};

DetalhesAluno.defaultProps = {
  dados: [],
  onAnterior: () => {},
  onProximo: () => {},
  print: true,
  onPrint: () => {},
};

export default DetalhesAluno;
