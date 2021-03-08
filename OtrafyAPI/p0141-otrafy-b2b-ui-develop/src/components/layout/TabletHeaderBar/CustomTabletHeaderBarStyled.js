import React from 'react'
import styled from 'styled-components'
import { Link } from 'react-router-dom'

export const HeaderBarWrapper = styled.div`
  background: ${props => props.theme.gradientColor};
  height: 82px;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  z-index: 100;
  box-shadow: ${props => props.theme.shadowColor};
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 15px;
`
export const LogoWrapper = styled(Link)``
export const DrawerToggler = styled.div`
  &:hover {
    cursor: pointer;
  }
`
export const ToolsboxWrapper = styled.div`
  display: flex;
  align-items: center;
`
export const AvatarWrapper = styled.div`
  width: 46px;
  height: 46px;
  margin-left: 40px;
  &:hover {
    cursor: pointer;
  }
  img {
    border-radius: 50%;
  }
`