import React, { useState } from 'react'
import styled from 'styled-components';
import { Base } from '../componentes/colors';

const BtnPerfil = () => {
  const [ocultaPerfis, setarOcultaPerfis] = useState(true);
  const [perfilSelecionado, setarPerfilSelecionado] = useState({
    id: "2",
    descricao: 'Professor'
  });

  const perfis = {
    perfilSelecionado: {
      id: "2",
      descricao: 'Professor'
    },
    data: [
      {
        id: "1",
        descricao: 'Diretor',
      },
      {
        id: "2",
        descricao: 'Professor'
      },
      {
        id: "3",
        descricao: 'Coordenador Pedagógico',
        abreviacao: 'CP'
      },
      {
        id: "4",
        descricao: 'Professor Orientador de Área',
        abreviacao: 'POA'
      }
    ]
  };

  const ItensPerfil = styled.div`
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    width:145px;
    height: auto;
    background: ${Base.Branco};
    border: solid ${Base.CinzaDesabilitado} 1px;
    position: absolute;
    right:16%;
  `;

  const Item = styled.tr`  
    text-align: left;
    width: 100%;
    height:100%;
    vertical-align: middle !important;

    &:not(:last-child){
      border-bottom: solid ${Base.CinzaDesabilitado} 1px !important;
    }

    &:hover{
      cursor: pointer;
      background: #e7e6f8;
      font-weight: bold !important;  
    }
    
    td{
      height: 35px;
      font-size: 10px;
      padding-left: 7px;
      width: 145px;
    }

    i{
      font-size: 14px;
      color: #707683;
    }
  `;

  const Botao = styled.a`
    display: block !important;
    text-align: center !important;
  `;

  const IconePerfil = styled.div`
    background: ${Base.Roxo};
    color: ${Base.Branco};
    font-size: 18px !important;
    height: 28px !important;
    width: 28px !important;
    vertical-align: middle;
    box-sizing: border-box;
    align-items: center !important;
    border-radius: 50%;
    display: inline-block;
    justify-content: center !important;

    i{
      margin-top:5px;
    }
  `;

  const gravarPerfilSelecionado = (perfil) => {
    if (perfil) {
      const perfilNovo = perfis.data.filter(item => item.id === perfil)
      setarPerfilSelecionado(perfilNovo[0]);
      setarOcultaPerfis(true);
    }
  }

  const onClickBotao = () => {
    if (perfis.data.length > 1) {
      setarOcultaPerfis(!ocultaPerfis);
    }
  };


  return (
    <div className="position-relative">
      <Botao className="text-center" onClick={onClickBotao} style={{ cursor: perfis.data.length > 1 ? 'pointer' : 'default' }}>
        <IconePerfil>
          <i className="fas fa-user-circle" />
        </IconePerfil>
        <span className={`d-block mt-1 ${ocultaPerfis ? '' : ' font-weight-bold'}`} >
          {perfilSelecionado.abreviacao ? perfilSelecionado.abreviacao : perfilSelecionado.descricao}
        </span>
      </Botao>
      <ItensPerfil hidden={ocultaPerfis} className="list-inline">
        <table>
          <tbody>
            {perfis.data.map(item =>
              <Item key={item.id}
                onClick={(e) => gravarPerfilSelecionado(e.currentTarget.accessKey)}
                accessKey={item.id}>
                <td style={{ width: '20px' }}>
                  <i value={item.id} className="fas fa-user-circle"></i>
                </td>
                <td style={{ width: '100%', fontWeight: item.id === perfilSelecionado.id ? 'bold' : 'initial' }}>
                  {item.descricao + (item.abreviacao ? "(" + item.abreviacao + ")" : "")}
                </td>
              </Item>
            )}
          </tbody>
        </table>
      </ItensPerfil>
    </div>
  )
}

export default BtnPerfil;
