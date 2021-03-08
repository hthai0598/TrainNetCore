import React from 'react'
import styled from 'styled-components'

export const BlockWrapper = styled.div`
  display: block;
  position: fixed;
  width: 100vw;
  height: 100vh;
  top: 0;
  left: 0;
  z-index: 9999;
`
export const SectionWrapper = styled.div`
  display: flex;
  text-align: center;
  justify-content: center;
  align-items: center;
  padding: 32px 0;
`
export const Spinner = styled.div`
  border-radius: 50%;
  width: 60px;
  height: 60px;
  border: 8px solid transparent;
  border-bottom-color: ${props => props.theme.solidColor};
  border-top-color: ${props => props.theme.solidColor};
  animation: loading-spinner 0.5s infinite linear;
`