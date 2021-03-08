import React from 'react'
import styled from 'styled-components'
import { Link } from 'react-router-dom'

export const IconWrapper = styled.div`
  text-align: center;
  margin-bottom: 20px;
  img {
    width: 80px;
    height: 80px;
  }
`
export const Heading = styled.h1`
  font-size: 24px;
  color: #000;
  font-weight: 500;
  margin-bottom: 15px;
  text-align: center;
`
export const Message = styled.p`
  color: #666;
  font-size: 14px;
  margin-bottom: 25px;
  text-align: center;
  line-height: 1.8;
`
export const NaviLink = styled(Link)`
  display: block;
  text-align: center;
  &:hover {
    text-decoration: underline;
  }
`