import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.div`
  position: relative;
  display: flex;
  align-items: center;
`
export const Jumper = styled.div`
  color: #C6CACC;
  width: 32px;
  height: 32px;
  line-height: 32px;
  text-align: center;
  transition: ease .3s;
  border-radius: 5px;
  &:hover {
    cursor: ${props => props.allowed ? 'pointer' : 'not-allowed'};
    color: ${props => props.allowed ? props.theme.solidColor : '#C6CACC'};
    background-color: ${props => props.allowed ? props.theme.solidLightColor : 'transparent'};
    transition: ease .3s;
  }
  .anticon {
    vertical-align: -3px;
    font-size: 16px;
  }
`
