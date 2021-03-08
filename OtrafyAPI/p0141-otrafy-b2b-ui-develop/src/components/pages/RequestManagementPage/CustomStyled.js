import React from 'react'
import styled from 'styled-components'
import { Input } from 'antd'

const { Search } = Input

export const SearchBar = styled(Search)`
  min-width: 320px;
`
export const TableWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`