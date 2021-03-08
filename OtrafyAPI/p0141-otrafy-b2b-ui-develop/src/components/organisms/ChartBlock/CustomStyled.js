import React from 'react'
import styled from 'styled-components'

export const ChartCard = styled.div`
  display: block;
  background-color: #fff;
  border-radius: 10px;
  padding: 15px;
  margin-bottom: 4%;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
`
export const CardHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  margin-bottom: 15px;
  .title {
    color: #000;
    font-weight: 500;
    font-size: 14px;
    margin: 5px 5px 5px 0;
  }
`