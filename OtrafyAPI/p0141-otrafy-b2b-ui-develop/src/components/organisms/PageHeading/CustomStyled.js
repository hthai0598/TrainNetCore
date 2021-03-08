import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  padding: 20px 0;
  @media screen and (max-width: 1024px) {
    padding: 0 0 15px;
  }
`
export const Heading = styled.div`
  h1 {
    font-weight: 500;
    font-size: 20px;
    line-height: 24px;
    margin-top: 10px;
    margin-bottom: 10px;
    margin-right: 30px;
    @media screen and (max-width: 1024px) {
      font-size: 18px;
    } 
  }
`
export const Tools = styled.div``