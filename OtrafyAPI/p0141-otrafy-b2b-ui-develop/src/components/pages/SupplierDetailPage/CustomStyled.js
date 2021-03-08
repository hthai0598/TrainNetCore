import React from 'react'
import styled from 'styled-components'
import { Input } from 'antd'

const { Search } = Input

export const LeftColumnContent = styled.div`
  width: 422px;
`
export const RightColumnContent = styled.div`
  width: calc(100% - 422px - 15px);
`
export const CardWrapper = styled.div`
  margin-bottom: 15px;
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 0 15px;
  height: 100%;
  .heading {
    padding: 15px 0;
    display: flex;
    align-items: center;
    justify-content: space-between;
    border-bottom: 1px solid #E3E5E5;
    .info {
      display: flex;
      align-items: center;
      img {
        width: 40px;
        margin-right: 15px;
      }
      span {
        font-size: 16px;
        color: #000;
        font-weight: 500;
      }
    }
    .action {
      border-radius: 50%;
      color: white;
      width: 22px;
      height: 22px;
      text-align: center;
      &:hover {
        cursor: pointer;
      }
    }
  }
  .body {
    padding: 20px 0;
    .title {
      font-size: 14px;
      color: #000;
      font-weight: 500;
      margin-bottom: 15px;
    }
    .list {
      display: flex;
      flex-wrap: wrap;
      margin-bottom: 0;
      dt {
        position: relative;
        padding-left: 20px;
        width: 160px;
        .color-svg {
          position: absolute;
          top: 5px;
          left: 0;
        }
      }
      dd {
        width: calc(100% - 160px);
        word-break: break-all;
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
  margin-bottom: 15px;
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px 15px 20px;
  height: 100%;
  .heading {
    margin-bottom: 15px;
    color: #000;
    font-size: 14px;
    font-weight: 500;
  }
`
export const TableWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`
export const TableHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  margin-bottom: 15px;
  .info {
    color: #000;
    font-size: 14px;
    font-weight: 500;  
    padding-right: 30px;
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
export const FormButtonGroup = styled.div`
  display: flex;
  justify-content: space-between;
  .ant-btn {
    width: 48%;
  }
`