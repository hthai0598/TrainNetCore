import React from 'react'
import styled from 'styled-components'
import { Input } from 'antd'

const { Search } = Input

export const Toolsbox = styled.div`
  display: flex;
  @media screen and (max-width: 1024px) {
    flex-wrap: wrap;
  }
`
export const StyledSearch = styled(Search)`
  min-width: 320px;
  margin-right: 10px !important;
  @media screen and (max-width: 1024px) {
    margin-bottom: 10px !important;
    input {
      height: 40px;
    }
  }
`
export const TableWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`