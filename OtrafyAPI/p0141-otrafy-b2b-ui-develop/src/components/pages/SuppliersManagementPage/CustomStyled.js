import React from 'react'
import { Input } from 'antd'
import styled from 'styled-components'

const { Search } = Input

export const AdvanceFilterTriggerButton = styled.div`
  background: #FFFFFF;
  border: 1px solid #E3E5E5;
  border-radius: 4px;
  margin-left: 10px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0 20px;
  transition: ease .3s;
  &:hover {
    transition: ease .3s;
    cursor: pointer;
    &.green {
      border-color: #3DBEA3 !important;
    }
    &.red {
      border-color: rgb(244, 67, 54) !important;    
    }
    &.blue {
      border-color: rgb(33, 150, 243) !important;
    }
    &.yellow {
      border-color: rgb(255, 152, 0) !important;
    }
  }
`
export const TableWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`
export const InputGroup = styled.div`
  display: flex;
`
export const StyledSearch = styled(Search)`
  min-width: 320px;
  max-width: 320px;
  @media screen and (max-width: 1024px) {
    input {
      height: 40px;
      border: 1px solid #E3E5E5;
    }
  }
`
export const FilterZoneWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;  
  margin-bottom: 10px;
  position: relative;
  padding: 30px 15px 15px;
`
export const CloseFilterButton = styled.div`
  position: absolute;
  top: 13px;
  right: 13px;
  width: 14px;
  height: 14px;
  border-radius: 50%;
  background-color: #919699;
  line-height: 12px;
  text-align: center;
  img {
    width: 6px;
    height: 6px;
  }
  &:hover {
    cursor: pointer;
  }
`
export const FilterFormWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  .form-col {
    flex-grow: 1;
    padding-right: 20px;
    .ant-row.ant-form-item {
      margin-bottom: 0;
    }
  }
`
export const ActionRow = styled.div`
  display: flex;
`
