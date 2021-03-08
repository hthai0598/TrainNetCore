import React from 'react'
import styled from 'styled-components'
import { Link } from 'react-router-dom'

export const AppSidebar = styled.div`
  border-radius: 10px;
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  overflow-x: hidden;
  overflow-y: auto;
`
export const LogoWrapper = styled(Link)`
  padding: 14px 6px;
  img {
    width: 58px;
  }
`
export const UtilityMenu = styled.div`
  margin-top: auto;
  width: 100%;
`
export const UserMenu = styled.div`
  display: flex;
  align-items: center;
  padding: 25px 0;
  transition: ease .3s;
  &:hover {
    cursor: pointer;
    background-color: rgba(0, 0, 0, 0.05);
    transition: ease .3s;
  }
`
export const Avatar = styled.div`
  position: relative;
  padding-left: 70px;
  img {
    width: 46px;
    height: 46px;
    border-radius: 50%;
    position: absolute;
    left: 12px;
    top: 50%;
    transform: translateY(-50%);
  }
  div {
    text-transform: capitalize;
    color: white;
    font-size: 14px;
    text-align: left;
    width: 130px;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    transition: opacity 0.3s cubic-bezier(0.645, 0.045, 0.355, 1), max-width 0.3s cubic-bezier(0.645, 0.045, 0.355, 1)
  }
`
export const CollapseButtonWrapper = styled.div`
  background: #C6CACC;
  border-radius: 0 4px 4px 0;
  position: absolute;;
  width: 12px;
  height: 14px;
  text-align: center;
  line-height: 12px;
  right: 0;
  top: 50%;
  transform: translate(100%, -50%);    
  &:hover {
    cursor: pointer;
  }
`