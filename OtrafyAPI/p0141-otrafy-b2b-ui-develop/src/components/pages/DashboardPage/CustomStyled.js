import React from 'react'
import { Input } from 'antd'
import styled from 'styled-components'

const { Search } = Input

export const StatisticCardWrapper = styled.div`
  display: flex;
  flex-wrap: wrap;
  .statistic-card {
    width: 32.5%;
    margin-right: 1.25%;
    margin-bottom: 1.25%;
    &:nth-child(3n) {
      margin-right: 0;
    }
  }
`
export const MainContentWrapper = styled.div`
   display: flex;
   justify-content: space-between;
   align-items: flex-start;
`
export const ChartWrapper = styled.div`
  width: 32.5%;
`
export const TableWrapper = styled.div`
  display: block;
  width: 66.25%;
  padding: 15px;
  background-color: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
`
export const TableHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 15px;
`
export const TableHeadingTitle = styled.div`
  color: #000;
  font-size: 14px;
  font-weight: 500;
`
export const StyledSearch = styled(Search)`
  max-width: 240px;
  margin-left: auto !important;
  input {
    font-size: 12px;
  }
`