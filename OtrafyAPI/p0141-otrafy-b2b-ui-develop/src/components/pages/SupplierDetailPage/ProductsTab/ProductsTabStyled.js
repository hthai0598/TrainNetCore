import React from 'react'
import { Input } from 'antd'
import styled from 'styled-components'

const { Search } = Input

export const TableWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`
export const TableHeading = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  margin-bottom: 15px;
  .title { 
    font-size: 14px;
    color: #000;
    font-weight: 500;
  }
  .action {
    display: flex;
    .ant-btn {
      height: 32px;
    }
  }
`
export const StyledSearch = styled(Search)`
  min-width: 240px;
  input {
    font-size: 12px;
  }
`
export const ModalLink = styled.div`
  &:hover {
    cursor: pointer;
    color: ${props => props.color};
  }
`
export const ModalContentWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  .col-left {
    width: 32.58%;
    border-right: 1px solid #E3E5E5;  
  }
  .col-right {
    width: calc(100% - 32.58%);
    > div {
      box-shadow: none;
    }
  }
`
export const ProductDetailCardWrapper = styled.div`
  padding: 15px;
  .heading {
    position: relative;
    padding-bottom: 10px;
    border-bottom: 1px solid #E3E5E5;
    .info {
      color: #000;
      font-size: 16px;
      font-weight: 500;
      text-transform: capitalize;
      padding-right: 30px;
    }
    .action {
      border-radius: 50%;
      color: white;
      position: absolute;
      top: 0;
      right: 0;
      width: 22px;
      height: 22px;
      line-height: 22px;
      text-align: center;
      &:hover {
        cursor: pointer;
      }
    }
  }
  .body {
    padding-top: 15px;
    .list {
      display: flex;
      flex-wrap: wrap;
      margin-bottom: 0;
      dt {
        position: relative;
        padding-left: 20px;
        width: 120px;
        .color-svg {
          position: absolute;
          top: 5px;
          left: 0;
        }
      }
      dd {
        width: calc(100% - 120px);
      }
      dt, dd {
        margin-bottom: 10px;
        &:last-of-type {
          margin-bottom: 0;
        }
      }
    }
  }
`
export const FormWrapper = styled.div`
  padding: 15px;
  .heading {
    font-size: 16px;
    color: #000;
    font-weight: 500;
    margin-bottom: 15px;
    padding-bottom: 10px;
    border-bottom: 1px solid #E3E5E5;
  }
`