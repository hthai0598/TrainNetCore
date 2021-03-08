import React from 'react'
import styled from 'styled-components'

export const FormPropertiesWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 10px 15px 0;
  width: 16%;
`
export const HeadingWraper = styled.div`
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 10px;
  .ant-checkbox-wrapper {
    margin-top: 5px;
    margin-bottom: 5px;
    .ant-checkbox + span {
      padding-right: 0;
    }
  }
`
export const Heading = styled.div`
  font-size: 14px;
  color: #000;
  font-weight: 500;
  margin: 5px 0;
  padding-right: 10px;
`
