import React from 'react'
import styled from 'styled-components';
import { Base } from '../componentes/colors';

const BtnPerfil = () => {
  const perfilSelecionado = {
    id: 4,
    descricao: 'Professor Orientador de Área',
    abreviacao: 'POA'
  }
  const perfis = {
    perfilSelecionado: {
      id: 4,
      descricao: 'Professor Orientador de Área',
      abreviacao: 'POA'
    },
    data: [
      {
        id: 1,
        descricao: 'Diretor',
      },
      {
        id: 2,
        descricao: 'Professor'
      },
      {
        id: 3,
        descricao: 'Coordenador Pedagógico',
        abreviacao: 'CP'
      },
      {
        id: 4,
        descricao: 'Professor Orientador de Área',
        abreviacao: 'POA'
      }
    ]
  };
  const Perfil = styled.div`
    margin: 0 5px 0 5px;
  `;

  const IconePerfil = styled.a`
    padding-bottom: 10px;

    i{
      align-items: center !important;
      background: ${Base.Roxo} !important;
      border-radius: 50% !important;
      color: ${Base.Branco} !important;
      display: flex !important;
      justify-content: center !important;
      font-size: 18px !important;
      height: 28px !important;
      width: 28px !important;
    }
    .btn{
      background: ${Base.Roxo};
      color: ${Base.Branco};
    }
  `
  const ItensPerfil = styled.div`
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    width:145px;
    height: auto;
    background: ${Base.Branco};
    border: solid ${Base.CinzaDesabilitado} 1px;
    position: absolute;
    right:100px;
    text-align: left;
    a{
      width: 100%;
    }
  `;

  const Li = styled.li`
      height: 35px;
      list-style-type: none;
      font-size: 10px;

    &:hover{
      background: #e7e6f8;
      font-weight: bold !important;
    }

    &:not(:last-child){
      border-bottom: solid ${Base.CinzaDesabilitado} 1px !important;
    }
  `;

  return (
    <Perfil>
      <IconePerfil>
        <i className="fas fa-user-circle btn fa-lg mb-1"></i>
      </IconePerfil>
      {perfilSelecionado.abreviacao ? perfilSelecionado.abreviacao : perfilSelecionado.descricao}
      <ItensPerfil>
        <ul>
          {perfis.data.map(item =>
            <a>
              <Li style={{ fontWeight: item.id === perfilSelecionado.id ? 'bold' : 'initial' }}>
                <i className="fas fa-user-circle"></i>
                {item.descricao + (item.abreviacao ? "(" + item.abreviacao + ")" : "")}
              </Li>
            </a>
          )}
        </ul>
      </ItensPerfil>
    </Perfil>
  )
}

export default BtnPerfil;
