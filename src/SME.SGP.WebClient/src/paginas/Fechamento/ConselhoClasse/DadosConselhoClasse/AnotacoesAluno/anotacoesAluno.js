import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erros } from '~/servicos/alertas';
import PropTypes from 'prop-types';
import AnotacoesAlunoLista from './anotacaoAlunoLista';

const AnotacoesAluno = props => {
  const { codigoTurma, codigoAluno, numeroBimestre } = props;

  const [exibirCardAnotacaoAluno, setExibirCardAnotacaoAluno] = useState(false);
  const [anotacoes, setAnotacoes] = useState([
    {
      componente: 'Língua Inglesa',
      professor: 'Prof. Maria do Cargo da Silva',
      professorRf: '123413',
      data: '01/01/2020',
      anotacao:
        'Lorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelitLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLoVLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLo',
    },
    {
      componente: 'Informática educativa',
      professor: 'Prof. João da Silva Carvalho',
      professorRf: '9874545',
      data: '20/05/2019',
      anotacao:
        'Lorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelitLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLoVLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLorem ipsum dolor sit amet, elitconsectetur adipiscing elitelitelit...VLo',
    },
  ]);

  const onClickAnotacaoAluno = async () => {
    // TODO
    // const retorno = await ServicoConselhoClasse.obterAnotacaoAluno(
    //   codigoTurma,
    //   codigoAluno,
    //   numeroBimestre
    // ).catch(e => erros(e));

    // if (retorno && retorno.data) {
    // }

    setExibirCardAnotacaoAluno(!exibirCardAnotacaoAluno);
  };

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="anotacao-aluno-collapse"
        onClick={onClickAnotacaoAluno}
        titulo="Anotações do aluno"
        indice="anotacao-aluno-collapse"
        show={exibirCardAnotacaoAluno}
        alt="card-collapse-anotacao-aluno"
      >
        {exibirCardAnotacaoAluno && anotacoes && anotacoes.length ? (
          <AnotacoesAlunoLista anotacoes={anotacoes} />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

AnotacoesAluno.propTypes = {
  onChangeAnotacaoAluno: PropTypes.func,
  turma: PropTypes.string,
  bimestre: PropTypes.string,
  alunoCodigo: PropTypes.string,
};

AnotacoesAluno.defaultProps = {
  onChangeAnotacaoAluno: () => {},
  turma: '',
  bimestre: '0',
  alunoCodigo: '',
};

export default AnotacoesAluno;
