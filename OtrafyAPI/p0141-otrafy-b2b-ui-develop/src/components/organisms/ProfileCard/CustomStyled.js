import React from 'react'
import styled from 'styled-components'

export const CardWrapper = styled.div`
  display: block;
  margin-left: auto;
  margin-right: auto;
  max-width: 860px;
`
export const CardHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-bottom: 30px;
  border-bottom: 1px solid #E3E5E5;
`
export const AvatarWrapper = styled.div`
  display: flex;
  align-items: center;
`
export const Avatar = styled.img`
  border-radius: 50%;
  width: 80px;
  height: 80px;
`
export const UserInfoTop = styled.div`
  margin-left: 30px;
  .name-row {
    color: #000;
    font-weight: 500;
    font-size: 20px;   
    margin-bottom: 5px;
  }
  .action-row {
    display: flex;
    align-items: center;
    color: #F44336;
    font-size: 14px;
    img {
      width: 14px;
      margin-left: 10px;
    }
    &:hover {
      cursor: pointer;
    }
  }
`
export const CardBody = styled.div`
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  flex-wrap: wrap;
  padding-top: 30px;
`
export const CardBodyContentCol = styled.div`
  width: 48%;
  @media screen and (max-width: 768px) {
    width: 100%;
    margin-bottom: 45px;
    &:last-child {
      margin-bottom: 0;
    }
  }
  .title {
    color: #000;
    font-size: 14px;
    font-weight: 500;
    margin-bottom: 25px;
  }
`
export const List = styled.dl`
  display: flex;
  flex-wrap: wrap;
  align-items: flex-start;
  dt {
    width: 140px;
    position: relative;
    padding-left: 20px;
    .color-svg {
      position: absolute;
      left: 0;
    }
  }
  dd {
    width: calc(100% - 140px);
    padding-left: 15px;
    word-break: break-all;
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
