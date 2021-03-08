import React from 'react'
import styled from 'styled-components'

export const CompanyDetailCardWrapper = styled.div``
export const CardHeader = styled.header`
  display: flex;
  background-color: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  align-items: center;
  padding: 20px 15px 23px;
  border-top-left-radius: 10px;
  border-top-right-radius: 10px;
  .avatar {
    width: 95px;
    img {
      width: 100%;
    }
  }
  .info {
    width: calc(100% - 95px);
    padding-left: 10px;
    .name {
      color: #000;
      font-size: 16px;
      font-weight: 500;
      position: relative;
      margin-bottom: 5px;
      display: flex;
      align-items: center;
      justify-content: space-between;
      span {
        width: calc(100% - 45px);
        overflow: hidden;
        text-overflow: ellipsis;
      }
      .action {
        width: 22px;
        height: 22px;
        border-radius: 50%;
        line-height: 22px;
        font-size: 10px;
        text-align: center;
        color: white;
        &:hover {
          cursor: pointer;
        }
      }
    }
  }
`
export const CardFooter = styled.footer`
  padding: 15px 15px 20px;
  background-color: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-bottom-left-radius: 10px;
  border-bottom-right-radius: 10px;
  position: relative;
  &:before{ 
    display: block;
    content: '';
    height: 1px;
    background-color: white;
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    padding-top: 10px;
    transform: translateY(-100%);
  }
  &:after {
    display: block;
    content: '';
    height: 1px;
    background-color: #E3E5E5;  
    position: absolute;
    top: 0;
    left: 15px;
    width: calc(100% - 30px);
  }
`
export const List = styled.dl`
  display: flex;
  flex-wrap: wrap;
  align-items: flex-start;
  dt {
    width: 130px;
    position: relative;
    padding-left: 20px;
    .color-svg {
      position: absolute;
      left: 0;
    }
  }
  dd {
    width: calc(100% - 130px);
    padding-left: 15px;
    span {
      overflow: hidden;
      display: block;
      width: 100%;
      text-overflow: ellipsis;
    }
  }
  dt, dd {
    color: #666;
    font-size: 14px;
    margin-bottom: 20px;
    &:last-of-type {
      margin-bottom: 0;
    }
  }
`
export const CompanyDetailCardEditWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 20px 15px;
`
export const MainContent = styled.div`
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
`
export const InfoCardWrapper = styled.div`
  display: block;
  width: 437px;
`
export const TableWrapper = styled.div`
  display: block;
  width: calc(100% - 437px - 15px);
  background-color: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
  position: relative;
`
export const TableHeading = styled.div`
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  justify-content: space-between;
  margin-bottom: 10px;
  .info, .action {
    display: flex;
    align-items: center;
    justify-content: flex-start;
  }
  .info {
    flex-grow: 1;
    padding-right: 15px;
    padding-top: 8px;
    padding-bottom: 8px;
    span {
      color: #000;
      font-size: 16px;
      font-weight: 500;
      margin-right: 30px;    
    }
  }
  .ant-input-affix-wrapper-lg {
    margin-left: auto;
    margin-right: 10px;
    width: 280px;
    input {
      font-size: 14px;
      height: 41px;
      line-height: 41px;
    }  
  }
`