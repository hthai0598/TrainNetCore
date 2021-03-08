import React from 'react'
import {Input} from 'antd'
import styled from 'styled-components'

const { Search } = Input

export const Toolsbox = styled.div`
  display: flex;
`
export const StyledSearch = styled(Search)`
  min-width: 320px;
  margin-right: 10px !important;
  @media screen and (max-width: 1024px) {
    min-width: 240px;
  }
`
export const TableWrapper = styled.div`
  display: block;
  padding: 15px;
  background-color: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  position: relative;
  margin-bottom: 20px;
  overflow-x: auto;
`
export const CompanyAvatar = styled.div`
  width: 40px;
  height: 40px;
  position: relative;
  margin-right: 15px;
  img {
    width: 100%;
    border-radius: 50%;
  }
  &:before {
    display: block;
    content: '';
    width: 11px;
    height: 11px;
    border: 1px solid white;
    position: absolute;
    top: 0;
    right: 0;
    border-radius: 50%;
    background-color: ${props => props.active ? '#3DBEA3' : '#F44336'};
  }
  + span {
    width: calc(100% - 40px);
  }
`