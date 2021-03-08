import React from 'react'
import styled from 'styled-components'
import { Link } from 'react-router-dom'

export const Heading = styled.h1`
  font-size: 24px;
  color: #000;
  font-weight: 500;
  margin-bottom: 5px;
`
export const Message = styled.p`
  color: #666;
  font-size: 14px;
  margin-bottom: 25px;
`
export const NaviLink = styled(Link)`
  margin-top: 5px;
  text-align: center;
  display: block;
  &:hover {
    text-decoration: underline;
  }
`